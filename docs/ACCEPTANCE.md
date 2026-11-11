# 验收清单

独立程序形态（脱离 Dalamud）的验收分为两部分：

- **A. 本机已验证**：在开发机上跑通、不需要真实游戏的部分。
- **B. 需真实游戏验证**：必须在 FFXIV 进程内才能确认的部分。

每一项都写明了怎么验、看哪里。

---

## A. 本机已验证

| # | 验收项 | 方式 | 结果 |
| --- | --- | --- | --- |
| A1 | 运行时工程可编译 | `dotnet build src\SlothCombo.Runtime -c Release` | 0 错误 0 警告（全量重建） |
| A2 | 主程序工程可编译 | `dotnet build src\SlothCombo.Host -c Release` | 0 错误 0 警告 |
| A3 | 引导库（原生 C，clang 编译）可编译 | `dotnet build src\SlothCombo.Boot -c Release` | 0 错误 0 警告 |
| A4 | 安装包可装配 | `.\build\publish.ps1` | 产出 `dist/`，含 hostfxr、`shared/Microsoft.NETCore.App`、引导库、Loader、托管运行时、静态表、wwwroot；Host 的 `--help` 正常 |
| A5 | 注入引导链可用 | `rundll32 dist\SlothCombo.Boot.dll,SlothComboBootRundll32` | hostfxr → Loader → 可卸载上下文里的托管运行时全部执行到（非游戏进程因无游戏状态按预期报「拒绝启动」） |
| A6 | PE 导出表解析正确 | 用 `llvm-readobj --coff-exports` 或 PE 查看器读 `dist\SlothCombo.Boot.dll` | 导出名 `SlothComboBoot` 与 `SlothComboBootRundll32` 均可见 |
| A7 | 命名管道 IPC 往返 | 运行 `SlothCombo.Host.exe`，在设置页触发任一需要游戏状态的接口 | 请求有正常响应，不报连接错误 |
| A8 | 无三方游戏库依赖 | `git grep -i -e Lumina -e FFXIVClientStructs -- src tools build` | 只剩文档与注释里的提及，工程引用里没有 |
| A9 | 热重载加载器 | 关掉 Host 窗口（游戏保持运行）→ 重新 `.\build\publish.ps1` → 再运行 Host | 打印 `The runtime is running.`；`%TEMP%\XIVSlothCombo\loader.log` 出现 `A new build was found` 与 `unloaded`；旧 DLL 可删除（文件锁释放） |
| A10 | 游戏占用时仍可构建 | 占用 dist 中任意文件后运行 `.\build\publish.ps1` | 未变化的占用文件被跳过；变化了的占用文件会列名并中止（不留半残安装） |
| A11 | 自研 interop 层可编译 | `dotnet build src\SlothCombo.Interop -c Release` | 0 错误 0 警告 |
| A12 | 静态表工程可编译 | `dotnet build src\SlothCombo.GameData -c Release` | 0 错误 0 警告 |
| A13 | 静态表齐全 | 读 `dist\game\GameData\` | 六种语言（chs/en/ja/de/fr/ko）都在，每种 17 张表 |
| A14 | 静态表能被运行时读出来 | `dotnet run --project tools\GameDataBuilder -c Release -- "src\SlothCombo.GameData\Tables" chs` | 每张表写完后立刻读回校验，全部通过（读不出来的表会直接报错中止） |
| A15 | 静态表体积 | 统计 `src\SlothCombo.GameData\Tables` | 六种语言合计约 10MB（压缩存放） |
| A16 | 图标地址规则 | 抽查静态表里的图标编号：`Invoke-WebRequest -Method Head "https://xivapi.com/i/<编号去掉末三位>/<编号>.png"` | 绝大多数返回 200；取不到的都是图床上确实没有的图标（多为刚更新的新技能） |
| A17 | 加载器与解决方案可编译 | 分别执行 `dotnet build src\SlothCombo.Loader -c Release`、`dotnet build XIVSlothCombo.sln -c Release` | 均为 0 错误 0 警告 |
| A18 | 前端类型检查与打包 | 在 `XIVSlothComboX.UI` 中执行 `npm run typecheck`、`npm run build:only` | 均通过，产物位于 `wwwroot/` |
| A19 | 缓存迁移及同版本复用（非游戏进程） | 将原 `"1"` 版本缓存复制到独立临时数据目录，通过 Loader 启动运行时；再次从同一 EXE 使用新版缓存启动 | 观察到 `fingerprint changed`、新 `2:<SHA-256>:<MVID>` 版本及 `fingerprint matches`；无缓存启动也观察到重新扫描；均按 A5 拒绝非游戏进程，不等同于游戏内启动验收 |
| A20 | 原用户数据保持不变 | 比对修复前后原数据目录的 `sigcache.json`、`config.json` 的 SHA-256 | 两者均未变化；诊断仅使用独立临时目录 |
| A21 | XIVAPI 图标资源 | 请求职业与技能的普通、高清 PNG，检查响应状态、类型与 PNG 文件头 | 骑士普通/高清为 32×32 / 64×64，技能 158 普通/高清为 40×40 / 80×80；均返回 200、`image/png` 和正确 PNG 文件头 |
| A22 | 引导库是原生库 | 看 `src\SlothCombo.Boot\bin\Release\net10.0\SlothCombo.Boot.dll` 与 `dist\SlothCombo.Boot.dll` | 两份都是几十 KB 的原生 DLL，导出表含 `SlothComboBoot` 与 `SlothComboBootRundll32`；`src\SlothCombo.Boot\` 目录下不留 DLL（clang 的产物必须落在输出目录，落在源码目录就等于没构建） |
| A23 | 引导库工具链 | `clang --version` | 目标为 `x86_64-w64-windows-gnu`（llvm-mingw）；官方 LLVM 的 clang 默认目标是 msvc，要靠 MSVC 与 Windows SDK 才能链接，与本仓库「不需要 MSVC」的前提不符 |
| A24 | 轮转功能不依赖三方游戏库 | `git grep -i -e Lumina -e FFXIVClientStructs -- src tools build` | 远端的 8.0 轮转功能已改用自研 `SlothCombo.Interop` 与自研静态表，源码里没有 FCS/Lumina 的引用 |
| A25 | 轮转用到的游戏原语可解析 | 看运行日志的 `Startup address` | `CanUseActionOnTarget`、`GetAdjustedCastTime`、`GetRecastTime`、`RaycastMaterialFilter`、`ResolvePlaceholder` 五个地址非零；缺任一个时运行时拒绝启动并写 `boot-error.txt` |
| A26 | 静态表带轮转需要的列 | 读 `src\SlothCombo.GameData\Tables\chs\Action.json.gz` 与 `ClassJobCategory.json.gz` | Action 行带 `canTargetSelf`、`canTargetHostile`、`canTargetAlly`、`targetArea`；ClassJobCategory 行带 `jobs` 成员职业缩写列表（分类 30「战斗精英」含 WAR 等 21 个职业） |
| A27 | 状态表带层数上限 | 读 `src\SlothCombo.GameData\Tables\chs\Status.json.gz` | Status 行带 `maxStacks`，轮转的层数条件据此判断 |

---

## B. 需真实游戏验证

> 前置：解压 `dist/SlothCombo.zip` 到一个路径不含特殊字符的目录；启动游戏到主菜单；
> 运行 `SlothCombo.Host.exe`。
>
> **热重载更新的第一次部署需要先关一次游戏**：旧的 `dist\SlothCombo.Boot.dll` 正被当前
> 游戏会话锁住，属于加载机制本身的一次性迁移。之后改代码只需要关掉 Host 窗口、重新构建、
> 再运行 Host，游戏不必再关（见 B11）。
>
> **2026-11-10 启动闪退修复的游戏内复验尚未完成。** B2–B6、B11 及新增 B13–B17
> 均需在真实游戏中确认，不能以构建成功或非游戏进程拒绝启动代替。

| # | 验收项 | 怎么验 | 预期 |
| --- | --- | --- | --- |
| B1 | 找到游戏进程 | 直接运行 `SlothCombo.Host.exe` | 控制台打印 `Injecting the runtime into the game (process <pid>)...` |
| B2 | 注入成功 | 同上 | 打印 `The runtime is running.`，数据目录下出现 `boot-status.json`（state=running） |
| B3 | 引导库在游戏进程内拉起托管运行时 | 查看 `%APPDATA%\XIVSlothComboX\logs\` | 有运行日志，无异常 |
| B4 | 技能改写 Hook 生效 | 游戏中启用一个连招，按对应键位 | 释放的是连招里的技能，不是原技能 |
| B5 | Hook 稳定性 | 连续读条、切图、进战斗 10 分钟 | 不崩、不掉线、Hook 不失效 |
| B6 | 游戏内 API 返回正确 | 浏览器打开设置页，逐个点开功能与选项 | `/api/meta`、`/api/presets/{id}`、`/api/options`、`/api/value`、`/api/config`、`/api/timeline` 均有正常响应 |
| B7 | 配置写回生效 | 在设置页改一个选项 → 保存 → 观察游戏内行为 | 改动立即生效；重启游戏后仍在（配置落在数据目录） |
| B8 | 主线程调度正确 | 连续快速切换多个功能开关 | 无跨线程异常，日志无 `InvalidOperationException` |
| B9 | 进程生命周期 | 关闭游戏 | `SlothCombo.Host.exe` 随之退出，端口释放 |
| B10 | 图标显示 | 设置页看职业图标、功能卡里的替换技能图标 | 图标正常显示；断网时显示占位块；版本刚更新的新技能图标在图床同步之前也是占位块 |
| B11 | 热重载（不关游戏） | 关掉 Host 窗口（游戏保持运行）→ 重新 `.\build\publish.ps1` → 再运行 `SlothCombo.Host.exe` | 打印 `The runtime is running.`；`%TEMP%\XIVSlothCombo\loader.log` 出现 `A new build was found` 与 `unloaded`；日志目录出现新会话头；设置页正常 |
| B12 | 构建不关游戏 | 游戏运行中执行 `.\build\publish.ps1` | 未变化的被占用文件被跳过；改动了的被占用文件会列名并要求关一次游戏 |
| B13 | 函数地址解析成功 | 看运行日志 | 没有 `Unresolved address:` 的记录；`%APPDATA%\XIVSlothComboX\` 下出现 `addresses.json` 且条数与签名表一致 |
| B14 | 静态表按语言加载 | 把客户端语言设成中文 / 英文各进一次游戏 | 技能名、状态名跟随客户端语言变化 |
| B15 | 读游戏状态 | 设置页看诊断信息 | 角色名、职业、等级、区域、队伍成员、状态列表都与游戏里一致 |
| B16 | 游戏更新后的旧缓存失效 | 保留现有 `sigcache.json`（包括 `Version` 为 `"1"` 的旧缓存），启动更新后的游戏，再运行 Host | 日志出现 `Signature cache fingerprint` 和 `fingerprint changed`；重新扫描后缓存版本为 `2:<游戏 EXE SHA-256>:<MVID>`，不复用旧偏移；游戏不闪退。**待验证** |
| B17 | 无缓存启动 | 使用 `SlothCombo.Host.exe --data <新的空目录>` 启动，不删除原数据目录或配置 | 日志出现 `No signature cache was found`，扫描并创建新缓存，启动成功。**待验证** |
| B18 | 相同版本复用缓存及重复启动 | 首次启动成功后关闭 Host，再运行同一构建；随后重启游戏并再次运行 Host | 同一会话中 Loader 提示运行时已在运行；新游戏会话日志出现 `fingerprint matches`，缓存可复用且连招仍生效。**待验证** |
| B19 | 主菜单与游戏内启动 | 分别在主菜单及角色进入游戏后启动 Host，随后打开配置页 | 两处均不闪退；消耗品目录加载不访问 UI Agent；游戏内 `/api/config` 食物列表正常，启用连招后技能改写正常。**待验证** |
| B20 | 地址更新后的热重载与稳定性 | 完成 B16 后重新构建运行时并重跑 Host，连续切图、读条及战斗 10 分钟 | 热重载正常；日志记录 `Startup address`，所有必需地址非零；技能改写、动作记录及主线程调度正常，无新增访问异常或崩溃。**待验证** |

### B10 说明（图标从哪来）

程序不读游戏文件，`.tex` 的像素在本地拿不到 —— Dalamud 的
`ITextureProvider.GetRgbaImageData` 随框架一起移除了，静态表里也只有图标的编号。

现在的做法是**按编号去公开图床取**，和 [souma 的 overlay](https://souma.diemoe.net/ff14-overlay-vue/)
一个路子：

- 编号来自静态表（`Action.Icon`、`Status.Icon`、`Item.Icon`）；
- 前端拼出 `https://<图床>/i/<编号去掉末三位>/<编号>.png`，高清版在文件名后加 `_hr1`；
- 先取 `xivapi.com`（国际站，png，直连），失败退回 `cafemaker.wakingsands.com`
  （国服镜像，webp）；
- 两处都取不到就显示占位块。

所以设置页的图标需要连得上外网。版本刚更新时图床还没跟上新技能图标，
这些图标会暂时显示占位块，等图床同步后自动恢复。

若要彻底离线，只能自写 BC1/BC2/BC3/BC7 解码器读 `sqpack` 里的 `.tex`，
或者在游戏进程内调用游戏自己的纹理读取接口；两者都要有游戏的安装文件。

### 启动闪退诊断（2026-11-10）

- 崩溃事件与转储定位到 `ConsumableChecker.Init()` 的 `0xc0000005` 访问异常；固定版本缓存使新游戏继续使用旧 Framework 偏移。
- 缓存现按格式版本、当前游戏 EXE 的 SHA-256 和程序集 MVID 隔离。游戏或结构层更新后自动重新扫描，不要求用户删除缓存；无法读取指纹时拒绝启动，不回退到旧地址。
- 自定义扫描器使用 `.text` 段基址计算匹配位置，再解析相对调用，避免 Hook 地址少算段偏移。
- 第一次读取游戏状态前检查 Framework 地址和实例；安装任何 Hook 前预检 Framework.Tick、技能改写、动作接收、动作发送及地面技能的必需地址。
- 缺少必需地址时，运行时写入 `boot-error.txt` 并让原生引导上报失败；不继续解引用，也不在失败清理时首次初始化 ActionWatching。
- 修复前的事件日志、转储和现有用户配置保留；尚未完成的游戏内验收继续按上表标为待验证。

### B10 步骤（XIVAPI 图标）

设置页也可直接使用 XIVAPI 的 PNG 资源，不再等待游戏内 `.tex` 解码，也不通过主线程 IPC
获取图标。旧本地图标接口保留，返回 404 不代表新版设置页的图标加载失败。

1. 重新装配安装包，关闭旧 Host 后再启动新版 Host，使用它新打开的地址。仅刷新旧 Host
   页面不够：旧进程的 CSP 只允许本地图像，会拦截 XIVAPI。
2. 打开职业列表、功能详情中的替换技能、时间轴职业图标，滚动列表确认延迟加载正常。
   网络面板中的图标请求应指向 `https://v2.xivapi.com/api/asset`，格式为 PNG，
   路径按千位分组，例如骑士为 `ui/icon/062000/062119.tex`。
3. 通过高清图标调用确认路径使用 `062119_hr1.tex`；普通/高清均应返回可显示的 PNG。
4. 检查 Host 的 CSP：只有 `img-src` 增加 `https://v2.xivapi.com`，脚本和 API 连接仍
   限定本地。图标 URL 不包含本地 `t` 令牌，图片请求不发送 Referer。
5. 在浏览器网络面板阻断 `v2.xivapi.com` 并禁用缓存后刷新：应显示等尺寸占位，
   不出现浏览器破图标记、不影响功能操作。解除阻断后刷新应恢复；切换到其他图标时应重新加载。
6. 在真实游戏中保持设置页打开并切换职业，确认连招与配置读写正常。此项不以图片 HTTP 200
   或前端构建通过代替真实游戏验收。

---

## C. 8.0 专区联合验收

本节为新增能力的验收凭据。未完成的步骤保持“待验证”，不以编译通过、
资料接口可读或人工快照调试代替真实游戏结论。国服版本未认证的目录只作为参考发布。

### 本机与文件交换

| # | 验收项 | 怎么验 | 结果 / 预期 |
| --- | --- | --- | --- |
| C1 | 全部构建 | 执行下方五个 Release 构建及两个前端的类型检查、生产打包 | 2026-11-10 最新容量与 IPC 修订后通过；Runtime、Host、Loader、Boot 及解决方案均 0 错误 0 警告，两端前端类型检查和生产打包通过 |
| C2 | 独立部署边界 | 独立构建 `C:\UGit\FF14\XIVRotationEditor`，检查网络请求及主解决方案、安装包列表 | 编辑器不连接 Host、不发现令牌；不加入主解决方案或安装包。待验证 |
| C3 | 三种文件 | 分别导出运行方案、工程、职业目录并重新导入 | 已核对三类 JSON 文件后缀；工程下载、文件重导入及替换确认成功，布局和快照存在；约 23 MB 目录完成下载、文件重导入及刷新恢复。主项目联动往返见 C4，尚待验证 |
| C4 | 往返语义 | 编辑器导出 → 专区导入 → 调整声明参数 → 下载 → 编辑器导入 | 稳定 ID、规则顺序、条件、阶段、序列、变量与参数语义保持；无第三方格式隐式转换。待验证 |
| C5 | 无效导入保护 | 导入损坏 JSON、未知版本、重复 ID、悬空引用、错误类型、未知字段及超深条件 | 定位阻断错误；已有方案和参数不变；重复方案默认另存，覆盖需确认。待验证 |
| C6 | 双视图与优先级 | 在规则表增删排序，切换画布、移动节点，再切回规则表 | 同一模型同步；拖动位置不改变执行顺序；自动布局只改变布局。待验证 |
| C7 | 编辑效率 | 搜索、复制、多选、撤销重做，刷新并恢复草稿 | 已确认重复方案另存、撤销导入恢复原方案和快照、刷新后 IndexedDB 恢复；复制全类型 ID、搜索和多选的完整操作仍待验证 |
| C8 | 人工快照调试 | 同一快照检查嵌套 AND/OR/NOT、阶段、变量、参数、单次/循环序列 | 已以人工快照选出动作 75，确认失败后不执行成功操作；嵌套条件、阶段和序列完整语义仍待验证 |
| C9 | 桌面与窄屏 | 检查宽桌面及 390px 窄屏，打开属性、条件与调试面板 | 已观察桌面三栏与 390px 布局、树折叠及画布自适应；未发现页面水平溢出，2000ms 输入完整显示。复杂属性和图标阻断回归仍待验证 |
| C10 | 不可信描述 | 打开含标签或脚本样式文本的资料，检查页面与导出文件 | 按文本显示、不执行脚本、不使用原始 HTML 注入。待验证 |
| C11 | 分组全量覆盖 | 执行采集、校验、打包并查看 coverage、gaps、五个分组中的 22 职业记录 | 已运行 catalog:validate/pack，22 职业、1455 条职业动作关联、1317 个去重动作、374 特性、4793 具名状态，1317/1317 中文动作详情请求成功；68 项资料缺口，不代表机制全量认证 |
| C12 | 职业与基础职业 | 检查骑士/剑术师、战士/斧术师、学者/秘术师及共享职能技 | 多对多归属保留，不因直属职业或 IsPlayerAction 过滤漏掉变体。待验证 |
| C13 | 数据特殊关系 | 检查特性充能、多前置、忍术、身位、宠物动作与青魔法书 | 特性等级与变体不硬猜；宠物不直接开放执行；青魔法书编号、行 ID、Action ID 分开。待验证 |
| C14 | 来源与版本 | 检查中文名称、机制、字段和关系的来源、哈希、schema 与响应版本 | 国际服仅参考；缺失不填零；冲突及 alternative 多值不擅自裁决；灰机 403/429 不绕过。待验证 |

2026-11-11 独立编辑器补充验收：规则表动作栏及右侧技能编辑栏显示真实技能图标；
目录缺失 ID 使用固定尺寸占位，动作序列使用独立标记。已确认桌面及 390px 窄屏布局，
临时验收修改撤销后用户两条规则保留；独立编辑器类型检查、生产打包通过。
实际断网后的占位及离线流程仍待验证，详见独立编辑器 `docs/ACCEPTANCE.md`。

2026-11-11 骑士高级方案本机验收：Abstractions、Interop、GameData、Runtime、Host、
Loader、Boot 和完整解决方案逐项 Release 构建，均 0 错误 0 警告；主界面与独立编辑器
分别通过 `typecheck`、`build:only`。两仓库 `rotation.schema.json` 的 SHA-256 均为
`f52e17265a92e8f093c34883d39249317b7620899860b84d690ef40659298928`。
独立编辑器已导入 24/14 条骑士单体/AoE 规则，编辑参数并重载、导出 JSON 后在隔离的
本地浏览器源重新导入；人工快照核对起手 2/3 GCD、悔罪链、远距离圣灵/投盾、
AoE 连击和缺失字段停止。上述仅为离线编排验收，不等于 Runtime 在游戏内导入或执行，
E14–E16 仍待真实游戏验证。

### 必须在真实游戏验证

以下各项本轮默认**待验证**，只有实际观察后才能更改结论。

| # | 验收项 | 怎么验 | 预期 |
| --- | --- | --- | --- |
| C15 | 职业模式互斥 | 同一职业保留旧 PvE 开关和旧时间轴，切到 8.0 后停止 | 旧配置保留；技能映射、旧时间轴、占星目标改写被隔离；暂停不回退 |
| C16 | 启停与后台 | 启动、手动开始后切出游戏，再停止与热重载 | 默认停止；开始后后台继续；热重载停止，不恢复运行状态 |
| C17 | 急停与人工接管 | 使用默认及自定义急停键，执行手动读条、队列技能 | 急停不依赖页面焦点；手动技能至少让位 2 秒，等待手动读条及队列结束 |
| C18 | 原生合法性及单 pending | 观察 GCD、动画锁、未解锁/缺资源/超射程/遮挡动作及慢响应 | 50ms 评估；每次最多提交一个，只保留一个待确认动作；不绕过原生合法性 |
| C19 | 动作关联与序列 | 观察同技能手动施放、重复效果、失败/超时、被抢占序列 | 校验自身施法者、实际技能、动作序号并去重；只有对应成功推进；默认等 10 秒，按终止/跳过策略处理 |
| C20 | 阶段与变量 | 对照同一人工快照，执行多条件阶段转换与动作后操作 | 每次最多转一个阶段；变量仅进入阶段或确认成功后改变；与编辑器语义一致 |
| C21 | 攻击与治疗目标 | 清空/切换敌方目标，使用自身、槽位、最低血量和职责治疗 | 不搜敌、不改变当前选中对象；治疗仅选规则合法候选；以原生目标检查为准 |
| C22 | 复活与地面技能 | 明确选择死亡队员，分别以自身/选定目标位置放置地面技能 | 存活/死亡选择明确；目标丢失不乱放；不调用消耗品或自动移动 |
| C23 | 脱战准备 | 有效倒计时、取消倒计时、转入战斗，检查预读授权 | 无倒计时不执行；攻击预读必须显式授权；取消重置、进战保留准备进度 |
| C24 | 生命周期安全 | 死亡、切图、切职业、进入 PvP、模拟可处理运行错误，再普通脱战 | 特殊状态停止重置；普通脱战重置本场后等待下一场；错误不能带崩游戏 |
| C25 | 保存与错误保护 | 运行中导入/切换/改参数；导入无效文件 | 成功变更前先停止且必须重新开始；无效导入不破坏已有数据 |
| C26 | 长时间稳定性 | 后台至少 10 分钟战斗、治疗、切图、读条并检查日志 | 无崩溃、掉线、重复提交、未处理异常或失效 Hook |
| C27 | 未知条件安全关闭 | 在高优先级治疗规则中引用当前职业不提供但协议允许的量谱字段，下方保留攻击规则；再分别检查 NOT、阶段转换和序列步骤中的未知条件 | 未知不当作 false，不选择低优先级规则或后续转换；Runtime 停止并清空进度，reason/unavailableFields 保留原因，必须显式重新开始；左到右短路跳过的字段不读取。待验证 |
| C28 | 启动职业绑定 | 两职业均配置 v8，在页面查看另一职业方案并开始；另在启动请求与首帧之间切职业 | 界面先刷新并核对职业；start 必须携带 expectedJobId，Runtime 主线程再次核对，错配 409 且不停止原运行；首帧前切职业也不能运行另一职业方案。待验证 |
| C29 | 控制超时与迟到响应 | 在独立验收环境中断控制响应，随后停止，再恢复响应 | 状态过期后显示未知并禁止启动；超时不宣称操作已取消；失败立即重读状态，迟到的 start 在用户停止意图之后须补偿停止。待验证 |
| C30 | 上一技能观测一致性 | 手动及自动施放原生变体，混合消耗品和其他 ActionType，再检查 lastAction 条件 | 上一技能持续以自身实际效果观测更新，不被旧循环全局记录在下一帧回写；职业/角色变更重新建立基线。待验证 |

### 本轮范围与限制（2026-11-10）

- 主项目及独立编辑器未连接真实游戏。本轮没有验证后台执行、急停、目标、复活、
  地面落点、倒计时、热重载和 10 分钟稳定性，也没有重新装配 `dist/` 安装包。
- 友方地面锚点尚不支持；地面技能仅支持自身或当前敌方目标锚点，并要求存在当前敌方目标。
  部分无法可靠分类的自身动作无敌方目标时会拒绝执行，C22/C23 不能据此标为完成。
- 所有战前动作都要求有效倒计时与 `allowPrepull:true`，比仅对攻击动作授权更严格。
  停止不取消已经提交的原生动作或读条。
- 已在独立编辑器粘贴导入 1,424,704 字节的合法运行方案（8 阶段、2400 条禁用规则），
  结构及引用校验无错误，导出 `.rotation.json` 为 2,003,604 字节；随后撤销导入恢复原草案。
  主专区及 Runtime 的大方案往返尚未连接游戏验证。
- 完整目录导出文本框及文件重导入默认显示最多 64 KiB 只读预览，内部原文和字节计数保持完整。
  下载文件为 22,986,583 字节（不含来源包末尾换行），已实际重导入并刷新恢复；390px 导入框无页面水平溢出。
- 目录为 22,986,584 字节；含 JSON 字符串信封估算为 25,424,636 字节，小于 32 MiB。
  主项目内嵌副本与独立编辑器公开包 SHA-256 均为
  `342b34c42267591ba2216b9a1f5314f2e84b89184f498593be6e3fa4d024c9de`。
- 青魔 124 条法书三 ID 关联已采集；骑士 255 条记录中有 216 条候选、215 条归属解释冲突。
  这些记录不能视为已确认可执行技能。中文 Trait 名称/说明、中文 Status 说明及 Wiki 正文均为 0，
  复杂关系保持待人工审核，国服精确版本未认证。

---

## D. 发版前检查

```powershell
dotnet build src\SlothCombo.Abstractions -c Release     # 0 错误 0 警告
dotnet build src\SlothCombo.Interop -c Release        # 0 错误 0 警告
dotnet build src\SlothCombo.GameData -c Release       # 0 错误 0 警告
dotnet build src\SlothCombo.Runtime -c Release        # 0 错误 0 警告
dotnet build src\SlothCombo.Host -c Release           # 0 错误 0 警告
dotnet build src\SlothCombo.Loader -c Release         # 0 错误 0 警告
dotnet build src\SlothCombo.Boot -c Release           # 0 错误 0 警告
dotnet build XIVSlothCombo.sln -c Release             # 0 错误 0 警告
Push-Location XIVSlothComboX.UI
npm run typecheck
npm run build:only
Pop-Location
.\build\publish.ps1                                   # 产出 dist/
```

CI（`.github/workflows/build.yml`）在 `push` 到 `API11` / `CN` / `main` / `备战8.0` 时触发，
自动完成装配 + 打包；提交信息里带 `[PUSH]` 时再额外发布一个 GitHub Release，
发版时机由提交者自己决定。

> 本仓库不写测试用例。上面 A 段每一项都要手工确认，CI 也不再跑测试步骤。

---

## E. 轮转协议、API 与持久化

以下为新增能力的手工验收清单，当前均**待真实游戏验证**。
协议及存储位置见 `docs/rotations/README.md`；验收使用独立数据目录，不破坏原用户文件。

| # | 验收项 | 怎么验 | 预期 |
| --- | --- | --- | --- |
| E1 | 默认结构示例安全 | 使用新的独立数据目录启动，读取 `/api/rotations` | WAR 动作 31/current、WHM 动作 120/lowestHp 示例存在；规则禁用、legacy、未选择，不自动发动 |
| E2 | Schema 与语义校验 | 分别导入有效计划和带未知属性、非法目标、错误值类型、悬空引用、重复条件 ID 的副本 | 有效计划保存；其他返回 400，现有计划、选择和引擎运行状态不被失败导入更改 |
| E3 | 导入边界 | 导入大于 1 MiB 且结构合法的方案，再尝试超底层 32 MiB 容量、JSON 深度超 128、条件深度超 32、条件总数超 8192、超时 49/300001、uint 溢出及未知量谱字段 | 不再按单方案 1 MiB 拒绝；仍受完整 IPC 帧、完整存储和文档处理的 32 MiB 底层容量及结构安全预算约束；无效输入不写入；序列 50–300000ms、最多 256 步受协议约束 |
| E4 | 显式覆盖 | 同 ID 重复导入一次不传 overwrite，再传 true；另尝试同 ID 更改职业 | 未授权覆盖及更换职业返回 409，旧数据不变；合法显式覆盖前停止引擎，成功后才发布新数据 |
| E5 | 模式、选择、参数与重启 | 选择计划、切换 v8、设置合法阈值，再重启；另提交错误类型、越界及未知参数 | 设置持久化且重启不自动开始；非法参数返回 400，旧值保持；省略覆盖项使用默认值 |
| E6 | 删除活动计划 | v8 模式下删除当前选中的计划 | 先停止引擎，清空选择但仍为 v8；不恢复旧 PvE，只有显式切 legacy 才恢复 |
| E7 | 原子保存失败 | 仅在独立验收目录阻止替换存储文件，再尝试修改 | 返回存储错误；原文件可读取，原内存快照、模式、参数不变，引擎停止；不存在半写入文件被当成有效状态 |
| E8 | 损坏与不支持版本 | 备份独立验收存储，分别破坏 JSON 或版本后重启，读取快照并尝试启动、导入 | 原文件不被默认值覆盖；快照 GET 200、ok:true 且携带 failClosed/initializationError，主界面显示存储锁定原因；启动和 POST 变更返回 400；22 个支持职业阻止旧 PvE 自动恢复，修复并重新初始化后恢复可操作 |
| E9 | 职业白名单 | 导入或设置生产、采集、基础职业及候选 43 的计划／模式 | 拒绝变更；fail-closed 不占用这些职业的旧逻辑；22 个已确认职业正常显示 |
| E10 | 参考目录与内嵌资源 | 不依赖相邻仓库运行发布包，读取 catalog；检查普通、布尔量谱和 countdown.remaining 字段类型；导入引用倒计时数值的有效计划 | Schema/catalog 从 DLL 资源读取，目录标记 reference、未认证；布尔量谱不被当作 number，countdown.remaining 为 number 且可导入；fallback 明确空数据而非仅支持少数职业 |
| E11 | 路由与主线程 | 读取快照、目录、状态，执行五种变更及 start/stop；尝试未知请求字段、路径和方法 | 快照、动态状态及变更经 dispatcher；静态目录在线程池处理，不读取游戏状态；返回既有响应信封和正确的 400/404/405/409 状态，无跨线程或 JSON 节点序列化异常 |
| E12 | 大目录读取性能 | 首次及并发读取完整内嵌 catalog，读取期间观察主线程帧耗时与引擎；再重复请求并重新初始化用户存储 | 资源解析、JSON 转换及响应序列化均不占用 dispatcher 或 Store 状态锁；首次请求共用一个任务，后续复用完整响应字符串；存储重新初始化不重建静态目录缓存，动态状态操作不被目录锁阻塞 |
| E13 | 完整传输容量与提交保护 | 在独立验收数据目录分别提交超量 Content-Length、chunked 请求及封装转义后超量的请求；再构造存储仍在 32 MiB 内但完整快照响应超量的变更 | 传输超限返回 413/IPC_MESSAGE_TOO_LARGE，不误报 GAME_NOT_READY；候选响应预检在停止和写盘前拒绝，原方案、选择、文件及运行状态不变；已有超量快照存储初始化保留原件、返回可读 fail-closed，不发送超量 IPC 帧。待验证 |
| E14 | 骑士方案导入与字段契约 | 分别导入独立编辑器的 `examples/pld-st.rotation.json`、`examples/pld-aoe.rotation.json`；读取 catalog 的字段类型；选择其中一个方案 | 两份方案均通过 Runtime v1 语义校验；新增布尔/数值字段类型与编辑器一致；仅选中的方案可运行，不因导入而启动。待游戏内验证 |
| E15 | 骑士原生技能及插窗 | 在独立验收环境切换单体/AoE，核对 2/3 GCD 起手、战逃/安魂、悔罪及赎罪升级链、盾阵忠义阈值、调停保留层数、远距离投盾/圣灵和 AoE 连击 | `adjustedId` 为原生实际 ID，执行请求仍为安全的基础动作；`resourceCost` 和 `unlocked` 与游戏状态一致；`weave.allowed` + `weaveWindow` 不在双插窗后继续提交，非公共 GCD 动作不能充当窗口基准，原生动作拒绝不越过保护。待游戏内验证 |
| E16 | 本场动作历史与故障关闭 | 先后手动和自动使用动作、脱战、更换角色/职业/区域；使动作地址、解锁或消耗读取不可用后再次评估依赖字段的方案 | `usedThisCombat` 包含已观察到的手动和自动动作，场景变化后清空；不可用字段安全停止并报告原因，不落入低优先级攻击规则。待游戏内验证 |
