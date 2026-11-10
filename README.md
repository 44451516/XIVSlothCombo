# XIVSlothComboX

把连击与互斥技能压缩到一个键位上的 FF14 连招增强工具。

> **当前形态：独立程序。**
> 不再依赖 Dalamud，也不需要在游戏里安装任何插件。发版节奏由本仓库自己决定，
> 不受上游 Dalamud / FFXIVClientStructs 的版本发布约束。
>
> **三方依赖已全部移除。** FFXIVClientStructs、Lumina、Lumina.Excel 都不再引用：
> 游戏结构与函数地址由本仓库自己的 `src/SlothCombo.Interop/` 承担，游戏数据表由
> `src/SlothCombo.GameData/` 读随程序发布的静态表。仓库里不再有 FCS 子模块。

---

## 架构

程序分成三块，两块在游戏进程外、一块在游戏进程内：

```
┌──────────────────────────── 游戏进程之外 ────────────────────────────┐
│                                                                    │
│  SlothCombo.Host.exe                                              │
│    ├─ 找到 ffxiv_dx11.exe 进程                                    │
│    ├─ LoadLibrary 注入 SlothCombo.Boot.dll（原生 C）                │
│    ├─ HttpListener 托管设置页（仅 127.0.0.1）                      │
│    └─ 管道客户端 SlothCombo.Ipc.{用户名}                           │
│                            │ 4 字节小端长度 + UTF-8 JSON 帧         │
│                            ▼                                       │
│  wwwroot/  Vue 3 单页应用（浏览器打开）                             │
└──────────────────────────── 游戏进程之内 ────────────────────────────┘
                             │ 注入 + hostfxr
┌──────────────────────────── 游戏进程之内 ────────────────────────────┐
│  SlothCombo.Boot.dll（原生 C 引导库）                               │
│    └─ hostfxr → SlothCombo.Loader.dll                               │
│         └─ 把 SlothCombo.Runtime.dll 装进可卸载的 AssemblyLoadContext│
│              ├─ Hook ActionManager.GetAdjustedActionId 改写技能选择  │
│              ├─ SlothCombo.Interop 读游戏状态、解析函数地址          │
│              ├─ SlothCombo.GameData 读随程序发布的静态表            │
│              ├─ 管道服务端：把请求派发到游戏主线程再执行             │
│              └─ API 语义：presets / value / config / timeline / icon│
└────────────────────────────────────────────────────────────────────┘
```

关键点：

- **注入是必须的**。连招的核心是改写「按了哪个键最终释放哪个技能」，这只能在游戏进程内
  Hook `ActionManager.GetAdjustedActionId` 做到，纯外部进程做不到。
- **不依赖任何游戏相关的三方库**。游戏结构与函数地址由 `src/SlothCombo.Interop/`
  自己承担，游戏数据表由 `src/SlothCombo.GameData/` 读随程序发布的静态表。
  游戏更新导致地址失效时，改的是本仓库里的签名表，不等上游发版。
- **设置界面跑在浏览器里**。Host 进程负责 HTTP 服务、静态资源与鉴权；所有需要游戏状态的
  接口由 Host 通过命名管道转给游戏内运行时，运行时再派发到游戏主线程执行。
- **一套 .NET 运行时**。Host 与游戏内运行时共用安装包里的 `shared/Microsoft.NETCore.App`，
  不要求玩家机器上装 .NET。
- **改代码不用关游戏**。CoreCLR 一旦进进程就卸不掉，所以常驻的 `SlothCombo.Loader.dll`
  只占住默认上下文的一小块，把运行时装进可回收的 `AssemblyLoadContext`。关掉 Host 窗口、
  重新构建、再运行 Host，就会先停旧运行时、卸旧上下文，再载入新构建。

### 目录

| 路径 | 说明 |
| --- | --- |
| `src/SlothCombo.Abstractions/` | 跨进程契约：IPC 请求/响应/帧协议、抽象接口、游戏表的行类型 |
| `src/SlothCombo.Interop/` | 与游戏对话的那一层：结构体布局、虚表、签名与地址解析 |
| `src/SlothCombo.GameData/` | 游戏静态表的读取：按客户端语言读 JSON，只查表 |
| `src/SlothCombo.Boot/` | 注入进游戏的原生 C 引导库（clang 编译），负责拉起托管运行时 |
| `src/SlothCombo.Loader/` | 常驻加载器：把运行时装进可卸载上下文，实现热重载 |
| `src/SlothCombo.Runtime/` | 游戏内托管运行时：连招逻辑、Hook、游戏数据、WebUI API |
| `src/SlothCombo.Host/` | 独立主程序：注入器、HTTP 服务、管道客户端 |
| `src/Directory.Build.props` | 独立程序的共享编译设置 |
| `tools/GameDataBuilder/` | 静态表生成器：从公开数据源生成 JSON 表（不进安装包） |
| `docs/ACCEPTANCE.md` | 验收清单：本机已验证项 / 需真实游戏验证项 |
| `build/publish.ps1` | 一键装配 `dist/` 安装包 |
| `XIVSlothComboX.UI/` | Vue 3 + TypeScript + Vite + Naive UI 前端工程 |

> 旧 Dalamud 插件工程 `XIVSlothComboX/` 已从仓库移除。它的代码早已整体并入
> `src/SlothCombo.Runtime/`，CI 也只构建 `src/` 下的独立程序。

---

## 快速开始

### 使用现成的安装包

1. 下载 `SlothCombo.zip`，解压到任意目录（路径不要含特殊字符）。
2. 启动游戏，等主菜单出来。
3. 运行 `SlothCombo.Host.exe`。
4. 浏览器会自动打开设置页，在页面里调整连招配置。

程序会一直跟着游戏进程存活，关掉游戏后一起退出。

### 热重载（改代码不关游戏）

1. 关掉 Host 窗口（游戏保持运行）。
2. 重新运行 `.\build\publish.ps1`。
3. 再运行 `SlothCombo.Host.exe`：加载器发现新构建，先停旧运行时、卸载旧上下文，再载入新的。

`%TEMP%\XIVSlothCombo\loader.log` 会记录每次替换，以及旧上下文是否真的被回收（旧构建的
临时文件要等回收后才释放）。只有 `SlothCombo.Loader.dll`、`hostfxr` 或 `shared` 运行时
本身变化时才需要重启游戏；第一次升级到带加载器的版本也需要先关一次游戏。

钩子里抛出的异常不会带崩游戏：`%TEMP%\XIVSlothCombo\detour-errors.log` 里有按秒节流的
记录，同样的错误也进运行日志。

### 命令行参数

```
SlothCombo.Host [选项]

  --pid <id>    指定要附加的游戏进程（默认自动查找）
  --data <dir>  指定数据目录（默认 %APPDATA%\XIVSlothComboX）
  --game <dir>  指定游戏安装目录（默认从游戏进程路径推导）
  --no-browser  不自动打开浏览器
  --help        显示帮助
```

### 数据目录

| 文件 | 说明 |
| --- | --- |
| `sloth-webui.json` | 设置页的端口、令牌与地址，供外部工具发现 |
| `boot-status.json` / `boot-error.txt` | 最近一次注入的结果与失败原因 |
| `logs/` | 运行日志 |

---

## 安装包布局

```
dist/
├── SlothCombo.Host.exe          独立主程序
├── SlothCombo.Boot.dll          注入进游戏的原生 C 引导库
├── SlothCombo.Loader.dll        游戏内的常驻加载器（热重载）
├── hostfxr.dll                  供主程序启动时寻找运行时
├── host/fxr/<版本>/hostfxr.dll  供引导库寻找运行时
├── shared/Microsoft.NETCore.App/<版本>/   .NET 运行时本体
├── game/                        游戏内托管运行时（SlothCombo.Runtime.*）
│   └── GameData/<语言>/*.json.gz 游戏静态表（压缩存放）
└── wwwroot/                     设置页前端构建产物
```

这个布局不是随意定的：`hostfxr` 会**从自己所在的位置反推 .NET 根目录**（在
`host/fxr/<版本>/` 形态下上溯三级），`DOTNET_ROOT` 环境变量对它无效。
所以运行时必须放在 `<根>/shared/Microsoft.NETCore.App/<版本>/`，
而 `hostfxr.dll` 必须同时出现在 `<根>/host/fxr/<版本>/` 与 apphost 同级两处。
自包含（self-contained）配置会被 `hostfxr_initialize_for_runtime_config` 直接拒绝。

---

## 从源码构建

### 前端

```powershell
cd XIVSlothComboX.UI
npm ci
npm run build:only     # 产物输出到 ./wwwroot
```

`npm run build` 会额外跑 `vue-tsc --noEmit` 做类型检查；`npm run build:only` 只打包，
`build/publish.ps1` 用的是后者。

### 装配完整安装包

```powershell
.\build\publish.ps1
```

脚本会依次：发布 Host → 发布游戏内托管运行时 → 发布加载器 → 用 clang 构建原生 C 引导库 →
构建前端 → 从本机 `dotnet` 安装目录取 `hostfxr` 与 `shared/Microsoft.NETCore.App` → 汇总成 `dist/`。

游戏运行中也可以构建：被占用的未变化文件会跳过，变化了的被占用文件会列名并要求先关掉
Host 与游戏，不会留下半残的安装目录。

参数：`-Configuration`（默认 Release）、`-Runtime`（默认 win-x64）、`-Output`（默认 dist）。

> **构建依赖**：.NET 10 SDK、Node.js（前端），以及 PATH 上的 `clang`（引导库）。
> 引导库 `src/SlothCombo.Boot/boot.c` 是原生 C，用 clang（如 llvm-mingw，默认目标
> `x86_64-w64-windows-gnu`）编译成 DLL，**不再需要 MSVC / Windows SDK / NativeAOT**。
> 可用 `-p:ClangCompiler=<路径>` 指定其它 clang。

### 单独构建某个工程

```powershell
dotnet build XIVSlothCombo.sln -c Release          # 整仓

dotnet build src\SlothCombo.Interop\SlothCombo.Interop.csproj -c Release
dotnet build src\SlothCombo.GameData\SlothCombo.GameData.csproj -c Release
dotnet build src\SlothCombo.Runtime\SlothCombo.Runtime.csproj -c Release
dotnet build src\SlothCombo.Host\SlothCombo.Host.csproj -c Release
dotnet build src\SlothCombo.Loader\SlothCombo.Loader.csproj -c Release
dotnet build src\SlothCombo.Boot\SlothCombo.Boot.csproj -c Release
```

共享的编译设置放在 `src/Directory.Build.props`。它刻意**不放在仓库根目录**：根目录的文件会同时作用到
仓库根下的其它工程上，会改变它们的编译行为。

### 重新生成游戏静态表

游戏数据表是预先生成好的，随程序一起发布。游戏改版后要重新生成：

```powershell
dotnet run --project tools\GameDataBuilder -c Release -- `
    "src\SlothCombo.GameData\Tables" chs en ja de fr ko
```

工具从公开数据源拉取每个表，转换成 JSON，再压成 `.json.gz`。每写完一张表，工具会立刻用
运行时的读法把它读回来，读不出来的表不会留在仓库里。生成器不在安装包里。

表的内容只保留程序用得到的列，值为默认的行会被整行略过，因此体积只有原始数据的一小部分。

### 验证

本仓库不写测试用例。改动的验证靠两种方式：

- **构建**：每个工程都必须 0 错误 0 警告。
- **人工验收**：清单见 [`docs/ACCEPTANCE.md`](docs/ACCEPTANCE.md)，逐项手工确认。

---

## 接口一览

所有响应统一封装为 `{ "ok": true, "data": ... }` 或
`{ "ok": false, "error": { "code", "message" } }`。

| 方法 | 路径 | 说明 |
| --- | --- | --- |
| GET | `/api/health` | 运行时版本与设置页端口 |
| GET | `/api/meta` | 功能树、职业列表、运行时版本 |
| GET | `/api/presets/{id}` | 单个功能的详情与选项 |
| GET | `/api/presets/{id}/options` | 单个功能的选项 schema |
| POST | `/api/presets/{id}/toggle` | 启用/禁用功能，自动处理父子与冲突 |
| GET/POST | `/api/value` | 按 `key` + `type` 读写配置项 |
| GET/PATCH | `/api/config` | 顶层配置 |
| GET/POST | `/api/timeline` | 时间轴列表与编辑动作 |
| GET | `/api/debug/*` | 诊断信息，仅 Debug 构建 |

接口只传图标的**编号**，图片本身由前端按编号去取（见「图标是从哪来的」）。

`type` 取值：`int` / `float` / `bool` / `boolArray` / `intArray`。

### 鉴权

- 只在回环地址监听，端口在 20000–60000 之间随机选择。
- 每次启动生成 32 字节随机令牌，请求必须带 `X-Sloth-Token` 头（或 `?t=` 查询参数）。
- 静态资源做了目录穿越防护，HTML 响应带 CSP 与 `X-Content-Type-Options`。

### 选项 schema 是怎么来的

选项没有重写一遍。`UserConfigItems.cs` 里原有的 `Draw*` 辅助函数保持不变，
只是增加了「双模式」：当 `SchemaCollector` 处于激活状态时，辅助函数不再画 ImGui 控件，
而是往 `SchemaBuilder` 里追加一条 `OptionSchema`。运行时请求某个功能的选项时，
用 `SchemaCollector` 重放一遍 `UserConfigItems.Draw`，就得到了该功能的完整控件描述。

前端按 `OptionSchema.kind` 渲染：`slider` / `drag` / `radio` / `switch` / `multi` / `single` / `priority`。

---

## 依赖分层

从 Dalamud 插件迁过来的时候，依赖被切成三层；现在前三层里只剩框架层是「移除」，
另外两层换成了本仓库自己的实现：

| 层 | 内容 | 现状 |
| --- | --- | --- |
| 结构体层 | FFXIVClientStructs | **自己实现**：`src/SlothCombo.Interop/` 只抄需要的结构体、虚表与签名 |
| 数据表层 | Lumina | **自己实现**：`src/SlothCombo.GameData/` 读预先生成好的静态表 |
| 服务层 | ECommons | **不整体引入**，只把实际用到的约 60 处 `Svc.*` 换成 `src/SlothCombo.Runtime/Services/Svc.cs` 的本地实现 |
| 框架层 | Dalamud | **完全移除**，不保留 `Dalamud.dll` / `Dalamud.Bindings.*` / `ImGuiScene.dll` 任何运行时依赖 |

### 结构体层怎么做的

`src/SlothCombo.Interop/` 里只有程序真正用到的那部分游戏结构：

- 结构体的字段偏移、虚表索引都写在代码里，来源是 FFXIVClientStructs 的公开实现，
  但代码是本仓库自己的，不引入那个库；
- 函数地址靠签名在游戏进程里现扫，结果写进 `addresses.json` 缓存，下次启动直接复用；
- 签名对不上的游戏版本，改的是本仓库里的签名表，不用等上游发版。

签名扫不到时不直接崩，而是记进运行日志，由对应功能自己降级。

### 数据表层怎么做的

游戏数据表由 `tools/GameDataBuilder/` 从公开数据源预先生成，压成 `.json.gz` 随程序发布：

- 运行时只按客户端语言读一份表，查表即可，不需要游戏安装目录里的文件；
- 表里只保留程序用得到的列，值为默认的行整行略过，六种语言加起来约 10MB；
- 生成器每写完一张表就立刻读回来校验，读不出来的表不会留在仓库里。

### 图标是从哪来的

程序不读游戏文件，也就拿不到图标里的像素。图标**编号**来自静态表，图片本身由
设置页按编号去公开图床取，做法和 [souma 的 overlay](https://souma.diemoe.net/ff14-overlay-vue/)
一致：

```
https://<图床>/i/<文件夹>/<编号>.png
```

编号就是图片在游戏文件里的编号，文件夹则是编号去掉末三位：编号 `215001` 的
图片在 `215000/215001.png`。高清版在文件名后加 `_hr1`。

前端先取国际站 `xivapi.com`（png，直连），失败再退回国服社区维护的镜像
`cafemaker.wakingsands.com`（webp），两处都取不到就显示占位块。后端只传编号，
不再有 `/api/icon/{id}` 这个接口。

---

## 已知限制

- **图标取自外部的公开图床**。程序不读游戏文件，本地也解不出 `.tex` 的像素，
  所以图标按编号去公开图床取（见「图标是从哪来的」）。断网时图标全缺，
  版本刚更新的新技能图标在图床同步之前也会缺，这两种情况都显示占位块，
  其余功能不受影响。
- **注入链路只做过非游戏进程的验证**。整条 `rundll32 → hostfxr → 托管运行时` 链路已实测跑通，
  但 Hook 稳定性、游戏内接口的真实返回、WebUI 的实际交互，仍需在真实游戏进程中验证。
  逐项验收步骤见 [`docs/ACCEPTANCE.md`](docs/ACCEPTANCE.md)。
- **插件工程 `XIVSlothComboX/` 已从仓库移除**。它的代码早已整体并入
  `src/SlothCombo.Runtime/`，CI 也只构建 `src/` 下的独立程序，保留一份副本只会让两处
  代码继续漂移。原先只服务旧插件的零调用代码（`RepoCheck`、`LuminaSheets`、
  `ActionEffectHeader`、`DutySchedule`、`MCHOpenerLogic94` 等）已一并清除。
- **`ECommons` 子模块已从解决方案中移除**。它引用的 Dalamud 侧 FCS / Lumina 版本
  与本仓库不再匹配，独立编译会报错。独立程序不依赖 ECommons——原先那约 60 处 `Svc.*`
  已由 `src/SlothCombo.Runtime/Services/Svc.cs` 就地替代。
- **「自动消耗」类功能（自动精炼、自动吃食物）目前只有实现，没有驱动**。它们的代码
  （`Generated/Spiritbond.cs`、`Generated/AutoItem.cs`）与设置项都还在，但旧插件里
  驱动它们的每帧逻辑随插件一起被移除，独立程序尚未补上对应的 tick 处理。
  设置页上的这些开关暂时不会有实际效果。
- **结构体与签名只覆盖程序用到的部分**。`src/SlothCombo.Interop/` 是按需抄出来的，
  不是完整的游戏结构库。要接新的游戏状态时，在那一层补结构与签名，
  不要把上游库加回来。

---

## 致谢

- [FFXIVClientStructs](https://github.com/aers/FFXIVClientStructs) —— 游戏内部结构定义，
  本仓库的 `src/SlothCombo.Interop/` 抄自它的公开实现
- [xivdev/ffxiv-datamining](https://github.com/Souma-Sumire/ffxiv-datamining-hexcode-mixed) ——
  游戏数据表的公开数据源，静态表由它生成
- [souma 的 ff14-overlay-vue](https://github.com/Souma-Sumire/ff14-overlay-vue) ——
  图标的取法参考自它，用的也是它那一套图床
- [Dalamud](https://github.com/goatcorp/Dalamud) 与 [ECommons](https://github.com/FFXIVClientStructs/ECommons) —— 本项目前身的框架与工具
