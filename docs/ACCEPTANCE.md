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
| A4 | 安装包可装配 | `.\build\publish.ps1` | 产出 `dist/`，含 hostfxr、`shared/Microsoft.NETCore.App`、引导库、Loader、托管运行时、静态表、wwwroot |
| A5 | 注入引导链可用 | `rundll32 dist\SlothCombo.Boot.dll,SlothComboBootRundll32` | hostfxr → Loader → 可卸载上下文里的托管运行时全部执行到（非游戏进程因无游戏状态按预期报「拒绝启动」） |
| A6 | PE 导出表解析正确 | 用 `dumpbin /exports` 或 PE 查看器读 `dist\SlothCombo.Boot.dll` | 导出名 `SlothComboBoot` 可见 |
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

---

## B. 需真实游戏验证

> 前置：解压 `dist/SlothCombo.zip` 到一个路径不含特殊字符的目录；启动游戏到主菜单；
> 运行 `SlothCombo.Host.exe`。
>
> **热重载更新的第一次部署需要先关一次游戏**：旧的 `dist\SlothCombo.Boot.dll` 正被当前
> 游戏会话锁住，属于加载机制本身的一次性迁移。之后改代码只需要关掉 Host 窗口、重新构建、
> 再运行 Host，游戏不必再关（见 B11）。

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

---

## C. 发版前检查

```powershell
dotnet build src\SlothCombo.Interop -c Release      # 0 错误 0 警告
dotnet build src\SlothCombo.GameData -c Release      # 0 错误 0 警告
dotnet build src\SlothCombo.Runtime -c Release        # 0 错误 0 警告
dotnet build src\SlothCombo.Host -c Release          # 0 错误 0 警告
dotnet build src\SlothCombo.Loader -c Release        # 0 错误 0 警告
dotnet build src\SlothCombo.Boot -c Release          # 0 错误 0 警告
.\build\publish.ps1                                  # 产出 dist/
```

游戏数据改了才需要重跑生成器：

```powershell
dotnet run --project tools\GameDataBuilder -c Release -- `
    "src\SlothCombo.GameData\Tables" chs en ja de fr ko
```

CI（`.github/workflows/build.yml`）在 `push` 到 `API11` / `CN` / `main` / `备战8.0` 时触发，
自动完成装配 + 打包；提交信息里带 `[PUSH]` 时再额外发布一个 GitHub Release，
发版时机由提交者自己决定。

> 本仓库不写测试用例。上面 A 段每一项都要手工确认，CI 也不再跑测试步骤。
