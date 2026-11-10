# XIVSlothComboX

把连击与互斥技能压缩到一个键位上的 FF14 连招增强工具。

> **当前形态：独立程序。**
> 不再依赖 Dalamud，也不需要在游戏里安装任何插件。发版节奏由本仓库自己决定，
> 不受上游 Dalamud / FFXIVClientStructs 的版本发布约束。

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
│              ├─ FCS 读游戏数据、Lumina 读 Sheet                     │
│              ├─ 管道服务端：把请求派发到游戏主线程再执行             │
│              └─ API 语义：presets / value / config / timeline / icon│
└────────────────────────────────────────────────────────────────────┘
```

关键点：

- **注入是必须的**。连招的核心是改写「按了哪个键最终释放哪个技能」，这只能在游戏进程内
  Hook `ActionManager.GetAdjustedActionId` 做到，纯外部进程做不到。
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
| `src/SlothCombo.Abstractions/` | 跨进程契约：IPC 请求/响应/帧协议、抽象接口 |
| `src/SlothCombo.Boot/` | 注入进游戏的原生 C 引导库（clang 编译），负责拉起托管运行时 |
| `src/SlothCombo.Loader/` | 常驻加载器：把运行时装进可卸载上下文，实现热重载 |
| `src/SlothCombo.Runtime/` | 游戏内托管运行时：连招逻辑、Hook、游戏数据、WebUI API |
| `src/SlothCombo.Host/` | 独立主程序：注入器、HTTP 服务、管道客户端 |
| `src/Directory.Build.props` | 独立程序的共享编译设置 |
| `Directory.Build.targets` | 定向压制子模块工程的警告 |
| `docs/ACCEPTANCE.md` | 验收清单：本机已验证项 / 需真实游戏验证项 |
| `build/publish.ps1` | 一键装配 `dist/` 安装包 |
| `docs/ACCEPTANCE.md` | 验收清单：本机已验证项 / 需真实游戏验证项 |
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

### 8.0 专区与独立编辑器

“8.0”是自定义循环专区的名称，不代表已经适配尚未核验的游戏版本。
设置页在 PvE 后提供独立的 **8.0 专区**，每个战斗职业分别选择旧 PvE 或 8.0 模式。
选择 8.0 模式后，即使停止执行，也不会回退到旧职业循环或旧时间轴；原有配置不会删除。

完整编排项目位于独立目录 `C:\UGit\FF14\XIVRotationEditor`，不加入本解决方案或安装包。
编辑器离线编辑文件，不连接 Host、不发现设置页令牌。两边交换以下版本化 JSON 文件：

| 文件后缀 | 内容 |
| --- | --- |
| `.rotation.json` | 单个运行方案、声明参数及参数值，不包含运行开关和实时进度 |
| `.rotation-project.json` | 多方案工程、节点布局与人工调试快照，仅供独立编辑器 |
| `.rotation-catalog.json` | 职业资料、字段说明、版本与来源，仅作为静态资料 |

在编辑器导出运行方案后，在专区导入并选择对应职业的方案、调整声明参数，再手动开始。
导入、切换、修改参数前会停止执行，保存后必须重新开始；启动及热重载不会恢复自动运行。
默认急停键为 `Ctrl+Alt+F12`，可在专区更改。请先检查目标、规则与同步等级，再开始。
战士、白魔示例不默认开启，也不是最佳循环承诺。

采集器优先保存 Garland 中文资料；XIVAPI 固定版本数据仅作国际服参考。
国服精确客户端版本尚未认证，目录必须保留参考标记；灰机访问受限和复杂机制审核缺口
不会用其他版本的数值静默补齐。采集、打包和来源记录见独立项目的 `docs/CATALOG.md`。

自动执行的真实游戏验收，包括手动接管、目标与复活、地面技能、倒计时、热重载和
至少 10 分钟稳定性，必须按 `docs/ACCEPTANCE.md` 完成。构建成功不等于这些行为已验证。

当前运行能力仍有保守限制：地面技能只允许自身或当前敌方目标锚点且要求存在当前敌方目标，
尚不支持友方地面锚点；部分无法分类的自身动作在没有敌方目标时拒绝执行。
所有战前动作都要求有效倒计时和显式 `allowPrepull` 授权。停止阻止后续提交，
不会取消已发出的动作或原生读条。这些限制不是已经完成完整游戏适配的承诺。

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

dotnet build src\SlothCombo.Runtime\SlothCombo.Runtime.csproj -c Release
dotnet build src\SlothCombo.Host\SlothCombo.Host.csproj -c Release
dotnet build src\SlothCombo.Loader\SlothCombo.Loader.csproj -c Release
dotnet build src\SlothCombo.Boot\SlothCombo.Boot.csproj -c Release
```

共享的编译设置放在 `src/Directory.Build.props`。它刻意**不放在仓库根目录**：根目录的文件会同时作用到 `lib/` 下的子模块工程上，
会改变它们的编译行为。子模块的警告在根目录的 `Directory.Build.targets` 里按工程名定向压制，
子模块工作区保持 pristine。

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
| GET/POST | `/api/rotations` | 8.0 方案、逐职业模式、选择与声明参数 |
| GET | `/api/rotations/catalog` | 内嵌参考职业目录及 Runtime 字段类型 |
| GET | `/api/rotations/state` | 8.0 引擎状态与能力限制 |
| POST | `/api/rotations/control` | 开始、停止、复位及急停键配置；开始必须携带并匹配 `expectedJobId` |
| GET | `/api/icon/{id}` | 旧本地图标接口（当前仍返回 404；设置页改为直连 XIVAPI） |
| GET | `/api/debug/*` | 诊断信息，仅 Debug 构建 |

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

从 Dalamud 插件迁过来的时候，依赖被切成三层：

| 层 | 内容 | 态度 |
| --- | --- | --- |
| 可替换库层 | FFXIVClientStructs、Lumina | 直接引用，上游跟不上时用「本地缓存 + 人工补丁」兜底 |
| 服务层 | ECommons | **不整体引入**，只把实际用到的约 60 处 `Svc.*` 换成 `src/SlothCombo.Runtime/Services/Svc.cs` 的本地实现 |
| 框架层 | Dalamud | **完全移除**，不保留 `Dalamud.dll` / `Dalamud.Bindings.*` / `ImGuiScene.dll` 任何运行时依赖 |

### FCS 签名滞后的兜底

`SignatureResolver` 负责在运行时按签名解析函数地址。缓存只用于同一游戏 EXE 和同一
FCS 程序集：游戏 EXE 的 SHA-256、FCS 程序集 MVID 或缓存格式版本变化时自动重新扫描，
不会把旧游戏的偏移用于新版本。无法读取指纹或缺少启动必需地址时，运行时拒绝启动并
记录原因，不继续解引用或安装 Hook。其余未解析地址记录到日志，供本仓库跟进签名适配，
不修改只读的 FCS 子模块。

---

## 已知限制

- **设置页图标需要访问 XIVAPI**。职业与技能图标由浏览器直接从
  `https://v2.xivapi.com/api/asset` 获取 PNG，不经过游戏进程，也不发送本地会话令牌或
  Referer。普通与高清图标分别使用 `.tex` 和 `_hr1.tex` 资源；无法联网或图片不存在时
  显示等尺寸占位，不影响连招与配置。旧 `/api/icon/{id}` 接口保留，但设置页不再调用。
- **注入链路只做过非游戏进程的验证**。整条 `rundll32 → hostfxr → 托管运行时` 链路已实测跑通，
  但 Hook 稳定性、游戏内接口的真实返回、WebUI 的实际交互，仍需在真实游戏进程中验证。
  逐项验收步骤见 [`docs/ACCEPTANCE.md`](docs/ACCEPTANCE.md)。
- **插件工程 `XIVSlothComboX/` 已从仓库移除**。它的代码早已整体并入
  `src/SlothCombo.Runtime/`，CI 也只构建 `src/` 下的独立程序，保留一份副本只会让两处
  代码继续漂移。原先只服务旧插件的零调用代码（`RepoCheck`、`LuminaSheets`、
  `ActionEffectHeader`、`DutySchedule`、`MCHOpenerLogic94` 等）已一并清除。
- **`ECommons` 子模块已从解决方案中移除**。它引用的 Dalamud 侧 FCS / Lumina 版本
  与当前 `lib/FFXIVClientStructs` 不匹配，独立编译会报 11 个错（与本仓库改动无关，
  属于子模块版本错配）。独立程序不依赖 ECommons——原先那约 60 处 `Svc.*` 已由
  `src/SlothCombo.Runtime/Services/Svc.cs` 就地替代。需要时可重新加回解决方案。
- **「自动消耗」类功能（自动精炼、自动吃食物）目前只有实现，没有驱动**。它们的代码
  （`Generated/Spiritbond.cs`、`Generated/AutoItem.cs`）与设置项都还在，但旧插件里
  驱动它们的每帧逻辑随插件一起被移除，独立程序尚未补上对应的 tick 处理。
  设置页上的这些开关暂时不会有实际效果。

---

## 致谢

- [FFXIVClientStructs](https://github.com/aers/FFXIVClientStructs) —— 游戏内部结构定义
- [Lumina](https://github.com/xivdev/lumina) —— 游戏数据表读取
- [Dalamud](https://github.com/goatcorp/Dalamud) 与 [ECommons](https://github.com/FFXIVClientStructs/ECommons) —— 本项目前身的框架与工具
