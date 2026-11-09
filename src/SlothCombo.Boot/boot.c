/*
 * SlothCombo.Boot - the native library that is loaded into the game.
 *
 * The game is a plain native program with no runtime of its own, so this
 * library hosts the .NET runtime itself and hands control to the managed
 * assembly. It is built ahead of time with clang (llvm-mingw), so it needs
 * neither MSVC nor the Windows SDK and carries no runtime dependency.
 *
 * The contract with the host program:
 *   - the injector calls the export "SlothComboBoot" as a thread entry;
 *   - rundll32 calls the export "SlothComboBootRundll32";
 *   - both read %TEMP%\XIVSlothCombo\boot.json (a flat object of strings);
 *   - both report progress in boot-status.json (State / Message / Timestamp).
 *
 * The library starts the runtime of the system and hands it the loader of
 * the program, not the runtime of the game. The loader keeps the runtime in
 * a context that can be unloaded, so a later boot can replace the runtime
 * without restarting the game.
 */
#include <windows.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <wchar.h>

#define BOOT_EXPORT __declspec(dllexport)

/* The delegate that loads an assembly and returns a function of it. */
#define HDT_LOAD_ASSEMBLY_AND_GET_FUNCTION_POINTER 5
/* The marker that asks for an UnmanagedCallersOnly entry point. */
#define UNMANAGEDCALLERSONLY_METHOD ((const wchar_t *)(intptr_t)-1)

typedef int32_t (*hostfxr_initialize_for_runtime_config_fn)(
    const wchar_t *runtime_config_path, const void *parameters, void **context);
typedef int32_t (*hostfxr_get_runtime_delegate_fn)(
    void *context, int type, void **delegate);
typedef int (*load_assembly_and_get_function_pointer_fn)(
    const wchar_t *assembly_path, const wchar_t *type_name,
    const wchar_t *method_name, const wchar_t *delegate_type_name,
    void *reserved, void **entry);
typedef int (*component_entry_fn)(const wchar_t *runtime_path,
                                  const wchar_t *data_folder,
                                  const wchar_t *game_install_path);

/* ------------------------------------------------------------------ */
/* Small growable byte buffer.                                         */
/* ------------------------------------------------------------------ */
typedef struct {
    char *data;
    size_t len;
    size_t cap;
} Buf;

static void buf_init(Buf *b) {
    b->cap = 256;
    b->len = 0;
    b->data = (char *)malloc(b->cap);
    if (b->data) b->data[0] = '\0';
}

static void buf_append(Buf *b, const char *s, size_t n) {
    if (!b->data) return;
    if (b->len + n + 1 > b->cap) {
        size_t cap = b->cap;
        while (b->len + n + 1 > cap) cap *= 2;
        char *grown = (char *)realloc(b->data, cap);
        if (!grown) return;
        b->data = grown;
        b->cap = cap;
    }
    memcpy(b->data + b->len, s, n);
    b->len += n;
    b->data[b->len] = '\0';
}

static void buf_append_str(Buf *b, const char *s) {
    if (s) buf_append(b, s, strlen(s));
}

static void buf_free(Buf *b) {
    free(b->data);
    b->data = NULL;
    b->len = b->cap = 0;
}

/* ------------------------------------------------------------------ */
/* Encoding helpers.                                                   */
/* ------------------------------------------------------------------ */
static wchar_t *utf8_to_wide(const char *s) {
    if (!s || !*s) return NULL;
    int n = MultiByteToWideChar(CP_UTF8, 0, s, -1, NULL, 0);
    if (n <= 0) return NULL;
    wchar_t *w = (wchar_t *)malloc((size_t)n * sizeof(wchar_t));
    if (!w) return NULL;
    if (MultiByteToWideChar(CP_UTF8, 0, s, -1, w, n) <= 0) {
        free(w);
        return NULL;
    }
    return w;
}

static char *wide_to_utf8(const wchar_t *w) {
    if (!w) return NULL;
    int n = WideCharToMultiByte(CP_UTF8, 0, w, -1, NULL, 0, NULL, NULL);
    if (n <= 0) return NULL;
    char *s = (char *)malloc((size_t)n);
    if (!s) return NULL;
    if (WideCharToMultiByte(CP_UTF8, 0, w, -1, s, n, NULL, NULL) <= 0) {
        free(s);
        return NULL;
    }
    return s;
}

/* ------------------------------------------------------------------ */
/* Paths in the temporary folder of the user.                          */
/* ------------------------------------------------------------------ */
static wchar_t *temp_root(void) {
    wchar_t buf[32768];
    DWORD n = GetTempPathW((DWORD)(sizeof(buf) / sizeof(buf[0])), buf);
    if (n == 0 || n >= sizeof(buf) / sizeof(buf[0])) return NULL;
    return _wcsdup(buf);
}

static wchar_t *path_join(const wchar_t *a, const wchar_t *b) {
    if (!a) return NULL;
    size_t la = wcslen(a);
    size_t lb = wcslen(b);
    int sep = (la > 0 && a[la - 1] != L'\\' && a[la - 1] != L'/');
    wchar_t *out = (wchar_t *)malloc((la + lb + 2) * sizeof(wchar_t));
    if (!out) return NULL;
    memcpy(out, a, la * sizeof(wchar_t));
    size_t pos = la;
    if (sep) out[pos++] = L'\\';
    memcpy(out + pos, b, (lb + 1) * sizeof(wchar_t));
    return out;
}

/* %TEMP%\XIVSlothCombo - the folder the settings file is read from. */
static wchar_t *config_folder(void) {
    wchar_t *root = temp_root();
    wchar_t *folder = path_join(root, L"XIVSlothCombo");
    free(root);
    return folder;
}

/* ------------------------------------------------------------------ */
/* Reading and writing files.                                          */
/* ------------------------------------------------------------------ */
static char *read_file_utf8(const wchar_t *path) {
    HANDLE h = CreateFileW(path, GENERIC_READ, FILE_SHARE_READ, NULL,
                           OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
    if (h == INVALID_HANDLE_VALUE) return NULL;
    LARGE_INTEGER size;
    if (!GetFileSizeEx(h, &size) || size.QuadPart < 0 ||
        size.QuadPart > 64 * 1024 * 1024) {
        CloseHandle(h);
        return NULL;
    }
    size_t n = (size_t)size.QuadPart;
    char *buf = (char *)malloc(n + 1);
    if (!buf) {
        CloseHandle(h);
        return NULL;
    }
    DWORD read = 0;
    BOOL ok = ReadFile(h, buf, (DWORD)n, &read, NULL);
    CloseHandle(h);
    if (!ok) {
        free(buf);
        return NULL;
    }
    buf[read] = '\0';
    /* Tolerate a UTF-8 BOM if one was written. */
    if (read >= 3 && (unsigned char)buf[0] == 0xEF &&
        (unsigned char)buf[1] == 0xBB && (unsigned char)buf[2] == 0xBF) {
        memmove(buf, buf + 3, read - 3 + 1);
    }
    return buf;
}

static void ensure_dir_recursive(const wchar_t *dir) {
    wchar_t tmp[32768];
    size_t len = wcslen(dir);
    if (len == 0 || len >= sizeof(tmp) / sizeof(tmp[0])) return;
    memcpy(tmp, dir, (len + 1) * sizeof(wchar_t));
    for (size_t i = 1; i < len; i++) {
        if (tmp[i] == L'\\' || tmp[i] == L'/') {
            wchar_t save = tmp[i];
            tmp[i] = L'\0';
            CreateDirectoryW(tmp, NULL);
            tmp[i] = save;
        }
    }
    CreateDirectoryW(tmp, NULL);
}

static void ensure_parent_dir(const wchar_t *path) {
    wchar_t tmp[32768];
    size_t len = wcslen(path);
    if (len == 0 || len >= sizeof(tmp) / sizeof(tmp[0])) return;
    memcpy(tmp, path, (len + 1) * sizeof(wchar_t));
    wchar_t *slash = wcsrchr(tmp, L'\\');
    wchar_t *fwd = wcsrchr(tmp, L'/');
    if (fwd && (!slash || fwd > slash)) slash = fwd;
    if (!slash) return;
    *slash = L'\0';
    ensure_dir_recursive(tmp);
}

static int write_file_utf8(const wchar_t *path, const char *data, size_t len) {
    ensure_parent_dir(path);
    HANDLE h = CreateFileW(path, GENERIC_WRITE, 0, NULL, CREATE_ALWAYS,
                           FILE_ATTRIBUTE_NORMAL, NULL);
    if (h == INVALID_HANDLE_VALUE) return 0;
    DWORD written = 0;
    BOOL ok = WriteFile(h, data, (DWORD)len, &written, NULL);
    CloseHandle(h);
    return ok && written == (DWORD)len;
}

/* ------------------------------------------------------------------ */
/* A focused reader for the flat string object the host writes.        */
/* ------------------------------------------------------------------ */
typedef struct {
    char *hostFxrPath;
    char *runtimeConfigPath;
    char *loaderPath;
    char *runtimePath;
    char *dataFolder;
    char *gameInstallPath;
    char *statusPath;
} BootConfig;

static void config_free(BootConfig *c) {
    free(c->hostFxrPath);
    free(c->runtimeConfigPath);
    free(c->loaderPath);
    free(c->runtimePath);
    free(c->dataFolder);
    free(c->gameInstallPath);
    free(c->statusPath);
    memset(c, 0, sizeof(*c));
}

static void skip_ws(const char **p) {
    while (**p == ' ' || **p == '\t' || **p == '\n' || **p == '\r') (*p)++;
}

static int hexval(int c) {
    if (c >= '0' && c <= '9') return c - '0';
    if (c >= 'a' && c <= 'f') return c - 'a' + 10;
    if (c >= 'A' && c <= 'F') return c - 'A' + 10;
    return -1;
}

static void utf8_encode(unsigned cp, char *out, int *n) {
    if (cp >= 0xD800 && cp <= 0xDFFF) cp = 0xFFFD; /* lone surrogate */
    if (cp < 0x80) {
        out[0] = (char)cp;
        *n = 1;
    } else if (cp < 0x800) {
        out[0] = (char)(0xC0 | (cp >> 6));
        out[1] = (char)(0x80 | (cp & 0x3F));
        *n = 2;
    } else if (cp < 0x10000) {
        out[0] = (char)(0xE0 | (cp >> 12));
        out[1] = (char)(0x80 | ((cp >> 6) & 0x3F));
        out[2] = (char)(0x80 | (cp & 0x3F));
        *n = 3;
    } else {
        out[0] = (char)(0xF0 | (cp >> 18));
        out[1] = (char)(0x80 | ((cp >> 12) & 0x3F));
        out[2] = (char)(0x80 | ((cp >> 6) & 0x3F));
        out[3] = (char)(0x80 | (cp & 0x3F));
        *n = 4;
    }
}

/* Reads a JSON string, decoding escapes, and returns it as UTF-8. */
static char *parse_string(const char **pp) {
    const char *p = *pp;
    if (*p != '"') return NULL;
    p++;
    Buf b;
    buf_init(&b);
    while (*p && *p != '"') {
        unsigned char c = (unsigned char)*p;
        if (c != '\\') {
            buf_append(&b, (const char *)&c, 1);
            p++;
            continue;
        }
        p++;
        char e = *p;
        if (!e) {
            buf_free(&b);
            return NULL;
        }
        p++;
        unsigned cp = 0;
        switch (e) {
            case '"': cp = '"'; break;
            case '\\': cp = '\\'; break;
            case '/': cp = '/'; break;
            case 'b': cp = '\b'; break;
            case 'f': cp = '\f'; break;
            case 'n': cp = '\n'; break;
            case 'r': cp = '\r'; break;
            case 't': cp = '\t'; break;
            case 'u': {
                unsigned h1 = 0;
                for (int i = 0; i < 4; i++) {
                    int h = hexval((unsigned char)*p);
                    if (h < 0) { buf_free(&b); return NULL; }
                    h1 = (h1 << 4) | (unsigned)h;
                    p++;
                }
                cp = h1;
                if (h1 >= 0xD800 && h1 <= 0xDBFF && p[0] == '\\' && p[1] == 'u') {
                    p += 2;
                    unsigned h2 = 0;
                    for (int i = 0; i < 4; i++) {
                        int h = hexval((unsigned char)*p);
                        if (h < 0) { buf_free(&b); return NULL; }
                        h2 = (h2 << 4) | (unsigned)h;
                        p++;
                    }
                    if (h2 >= 0xDC00 && h2 <= 0xDFFF)
                        cp = 0x10000 + ((h1 - 0xD800) << 10) + (h2 - 0xDC00);
                }
                break;
            }
            default:
                buf_free(&b);
                return NULL;
        }
        char enc[4];
        int en = 0;
        utf8_encode(cp, enc, &en);
        buf_append(&b, enc, (size_t)en);
    }
    if (*p != '"') {
        buf_free(&b);
        return NULL;
    }
    p++;
    *pp = p;
    if (!b.data) return NULL;
    return b.data; /* ownership handed to the caller */
}

static void config_assign(BootConfig *c, const char *key, char *value) {
    char **slot = NULL;
    if (strcmp(key, "HostFxrPath") == 0) slot = &c->hostFxrPath;
    else if (strcmp(key, "RuntimeConfigPath") == 0) slot = &c->runtimeConfigPath;
    else if (strcmp(key, "LoaderPath") == 0) slot = &c->loaderPath;
    else if (strcmp(key, "RuntimePath") == 0) slot = &c->runtimePath;
    else if (strcmp(key, "DataFolder") == 0) slot = &c->dataFolder;
    else if (strcmp(key, "GameInstallPath") == 0) slot = &c->gameInstallPath;
    else if (strcmp(key, "StatusPath") == 0) slot = &c->statusPath;
    if (!slot) {
        free(value);
        return;
    }
    free(*slot);
    *slot = value;
}

static int config_parse(const char *json, BootConfig *c) {
    const char *p = json;
    skip_ws(&p);
    if (*p != '{') return 0;
    p++;
    skip_ws(&p);
    if (*p == '}') return 1;
    for (;;) {
        skip_ws(&p);
        char *key = parse_string(&p);
        if (!key) return 0;
        skip_ws(&p);
        if (*p != ':') { free(key); return 0; }
        p++;
        skip_ws(&p);
        char *value = parse_string(&p);
        if (!value) { free(key); return 0; }
        config_assign(c, key, value);
        free(key);
        skip_ws(&p);
        if (*p == ',') { p++; continue; }
        if (*p == '}') return 1;
        return 0;
    }
}

/* ------------------------------------------------------------------ */
/* Writing the state of the boot.                                      */
/* ------------------------------------------------------------------ */
static void append_json_string(Buf *b, const char *s) {
    buf_append_str(b, "\"");
    for (const unsigned char *p = (const unsigned char *)s; p && *p; p++) {
        unsigned char c = *p;
        switch (c) {
            case '"': buf_append_str(b, "\\\""); break;
            case '\\': buf_append_str(b, "\\\\"); break;
            case '\b': buf_append_str(b, "\\b"); break;
            case '\f': buf_append_str(b, "\\f"); break;
            case '\n': buf_append_str(b, "\\n"); break;
            case '\r': buf_append_str(b, "\\r"); break;
            case '\t': buf_append_str(b, "\\t"); break;
            default:
                if (c < 0x20) {
                    char esc[7];
                    snprintf(esc, sizeof(esc), "\\u%04x", c);
                    buf_append_str(b, esc);
                } else {
                    buf_append(b, (const char *)&c, 1);
                }
        }
    }
    buf_append_str(b, "\"");
}

static void append_timestamp(Buf *b) {
    SYSTEMTIME st;
    GetSystemTime(&st);
    char ts[40];
    snprintf(ts, sizeof(ts), "%04u-%02u-%02uT%02u:%02u:%02u.%03uZ",
             st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond,
             st.wMilliseconds);
    append_json_string(b, ts);
}

static wchar_t *default_status_path(void) {
    wchar_t *folder = config_folder();
    wchar_t *path = path_join(folder, L"boot-status.json");
    free(folder);
    return path;
}

static wchar_t *fallback_status_path(void) {
    wchar_t *root = temp_root();
    wchar_t *path = path_join(root, L"SlothCombo.Boot.status.json");
    free(root);
    return path;
}

static void write_status(BootConfig *c, const char *state, const char *message) {
    Buf b;
    buf_init(&b);
    buf_append_str(&b, "{\n  \"State\": ");
    append_json_string(&b, state);
    buf_append_str(&b, ",\n  \"Message\": ");
    append_json_string(&b, message);
    buf_append_str(&b, ",\n  \"Timestamp\": ");
    append_timestamp(&b);
    buf_append_str(&b, "\n}\n");

    wchar_t *path = NULL;
    if (c && c->statusPath && *c->statusPath) path = utf8_to_wide(c->statusPath);
    if (!path) path = default_status_path();

    if (path && !write_file_utf8(path, b.data ? b.data : "", b.len)) {
        wchar_t *fb = fallback_status_path();
        if (fb) {
            write_file_utf8(fb, b.data ? b.data : "", b.len);
            free(fb);
        }
    }
    free(path);
    buf_free(&b);
}

/* ------------------------------------------------------------------ */
/* Hosting the runtime.                                                */
/* ------------------------------------------------------------------ */
static int start_runtime(BootConfig *c) {
    write_status(c, "starting", "The runtime is being loaded.");

    wchar_t *hostFxrPath = utf8_to_wide(c->hostFxrPath);
    HMODULE hostFxr = NULL;
    if (hostFxrPath) hostFxr = LoadLibraryW(hostFxrPath);
    free(hostFxrPath);
    if (!hostFxr) {
        char msg[4096];
        snprintf(msg, sizeof(msg), "The library '%s' could not be loaded.",
                 c->hostFxrPath ? c->hostFxrPath : "");
        write_status(c, "failed", msg);
        return 1;
    }

    hostfxr_initialize_for_runtime_config_fn initialize =
        (hostfxr_initialize_for_runtime_config_fn)GetProcAddress(
            hostFxr, "hostfxr_initialize_for_runtime_config");
    hostfxr_get_runtime_delegate_fn getDelegate =
        (hostfxr_get_runtime_delegate_fn)GetProcAddress(
            hostFxr, "hostfxr_get_runtime_delegate");
    if (!initialize || !getDelegate) {
        write_status(c, "failed",
                     "The library that hosts the runtime has no entry points.");
        return 1;
    }

    wchar_t *runtimeConfig = utf8_to_wide(c->runtimeConfigPath);
    wchar_t *loaderPath = utf8_to_wide(c->loaderPath);
    wchar_t *runtimePath = utf8_to_wide(c->runtimePath);

    /* The type and the entry point belong to the loader and never change. */
    static const wchar_t *loaderTypeName =
        L"SlothCombo.Loader.Loader, SlothCombo.Loader";
    static const wchar_t *loaderEntryName = L"Boot";

    int result = 1;
    void *context = NULL;
    int32_t r = initialize(runtimeConfig, NULL, &context);
    if (r != 0 && r != 1) {
        char msg[256];
        snprintf(msg, sizeof(msg), "The runtime could not be prepared (0x%08X).",
                 (unsigned)r);
        write_status(c, "failed", msg);
        goto cleanup;
    }

    void *loadAssembly = NULL;
    r = getDelegate(context, HDT_LOAD_ASSEMBLY_AND_GET_FUNCTION_POINTER,
                    &loadAssembly);
    if (r < 0 || !loadAssembly) {
        char msg[256];
        snprintf(msg, sizeof(msg),
                 "The loader of the runtime could not be read (0x%08X).",
                 (unsigned)r);
        write_status(c, "failed", msg);
        goto cleanup;
    }

    /*
     * The loader is the only assembly that is handed to the default context
     * of the runtime, where it stays for the rest of the session. It takes
     * the path of the runtime and keeps that one in a context of its own,
     * which can be unloaded when a newer build arrives.
     */
    void *entry = NULL;
    r = ((load_assembly_and_get_function_pointer_fn)loadAssembly)(
        loaderPath, loaderTypeName, loaderEntryName, UNMANAGEDCALLERSONLY_METHOD,
        NULL, &entry);
    if (r < 0 || !entry) {
        char msg[256];
        snprintf(msg, sizeof(msg),
                 "The loader of the runtime could not be loaded (0x%08X).",
                 (unsigned)r);
        write_status(c, "failed", msg);
        goto cleanup;
    }

    wchar_t *dataFolder = utf8_to_wide(c->dataFolder);
    wchar_t *gameInstallPath = utf8_to_wide(c->gameInstallPath);
    int exitCode = ((component_entry_fn)entry)(runtimePath, dataFolder,
                                               gameInstallPath);
    free(dataFolder);
    free(gameInstallPath);

    if (exitCode != 0) {
        char msg[256];
        snprintf(msg, sizeof(msg),
                 "The managed runtime refused to start (%d).", exitCode);
        write_status(c, "failed", msg);
        result = 1;
        goto cleanup;
    }

    write_status(c, "running", "The runtime is running.");
    result = 0;

cleanup:
    free(runtimeConfig);
    free(loaderPath);
    free(runtimePath);
    return result;
}

static int run(void) {
    wchar_t *folder = config_folder();
    wchar_t *path = path_join(folder, L"boot.json");
    free(folder);

    char *json = path ? read_file_utf8(path) : NULL;
    BootConfig config;
    memset(&config, 0, sizeof(config));
    int parsed = json ? config_parse(json, &config) : 0;

    if (!parsed) {
        char *utf8Path = wide_to_utf8(path);
        char msg[4096];
        snprintf(msg, sizeof(msg), "The settings file '%s' is missing.",
                 utf8Path ? utf8Path : "boot.json");
        free(utf8Path);
        write_status(NULL, "failed", msg);
        free(json);
        free(path);
        config_free(&config);
        return 1;
    }

    int result = start_runtime(&config);
    free(json);
    free(path);
    config_free(&config);
    return result;
}

/* ------------------------------------------------------------------ */
/* The exports the injector and the loader of the system call.         */
/* ------------------------------------------------------------------ */
BOOT_EXPORT DWORD WINAPI SlothComboBoot(LPVOID reserved) {
    (void)reserved;
    return (DWORD)run();
}

BOOT_EXPORT DWORD WINAPI SlothComboBootRundll32(HWND window, HINSTANCE instance,
                                                LPSTR commandLine, int showCommand) {
    (void)window;
    (void)instance;
    (void)commandLine;
    (void)showCommand;
    return (DWORD)run();
}