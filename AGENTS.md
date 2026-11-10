# AGENTS.md

给在本仓库工作的 AI Agent 的约定。**动手前先读完这份文件。**

---

## 不要写测试用例

**本仓库不写测试用例。不要新增任何测试。**

具体地，以下都是禁止的：

- 不要新建 `tests/` 目录，不要新建任何 `*.Tests.csproj` / `*.Test.csproj` 测试工程。
- 不要写 xUnit / NUnit / MSTest / 自研测试运行器 / 任何断言用的测试类。
- 不要写前端测试：不要建 `*.spec.ts` / `*.test.ts`，不要装 vitest / jest / @vue/test-utils / jsdom。
- 不要给工程加 `InternalsVisibleTo` 指向测试程序集。
- 不要在 CI（`.github/workflows/build.yml`）里加测试步骤。
- 不要为了「让测试通过」而改动产品代码。

原因是产品决策，不是技术偏好：这个程序注入进游戏进程运行，测试要么得伪造游戏环境（价值极低、维护成本极高），要么只能测到 PE 头解析这类与产品行为无关的部分。

### 验证改用什么

改动的正确性靠这两件事保证，两者都要做：

1. **构建必须干净。** 每个工程 0 错误 0 警告：

   ```powershell
   dotnet build src\SlothCombo.Runtime -c Release
   dotnet build src\SlothCombo.Host -c Release
   dotnet build src\SlothCombo.Loader -c Release
   dotnet build src\SlothCombo.Boot -c Release
   dotnet build XIVSlothCombo.sln -c Release
   ```

   前端：

   ```powershell
   cd XIVSlothComboX.UI
   npm run typecheck      # vue-tsc --noEmit
   npm run build:only
   ```

2. **走验收清单。** [`docs/ACCEPTANCE.md`](docs/ACCEPTANCE.md) 逐项手工确认，
   把改动影响到的条目更新到该文件里（新增能力就加一项，行为变了就改预期）。
   验收清单是这个仓库唯一的验收凭据，它比测试更能反映用户实际会遇到的问题。

### 不要「顺手」把测试加回来

即使看起来某个改动「应该配个测试」，也不要加。如果确实觉得某处需要回归保护，
正确的做法是把对应的手工验收步骤写进 `docs/ACCEPTANCE.md`，而不是写自动化测试。

---

## 仓库是什么

XIVSlothComboX —— FF14 连招增强工具。**独立程序形态，不依赖 Dalamud**，
不需要在游戏里安装任何插件。

程序分两块：游戏进程外是 `SlothCombo.Host.exe`（找游戏进程、注入、跑设置页的 HTTP 服务），
游戏进程内是 `SlothCombo.Runtime`（Hook 技能选择、读游戏数据、提供 API）。

```
src/SlothCombo.Abstractions/   跨进程契约：IPC 帧协议、抽象接口
src/SlothCombo.Boot/           注入进游戏的原生 C 引导库（clang 编译）
src/SlothCombo.Loader/         常驻加载器：把运行时装进可卸载上下文，实现热重载
src/SlothCombo.Runtime/        游戏内托管运行时：连招逻辑、Hook、WebUI API
src/SlothCombo.Host/           独立主程序：注入器、HTTP 服务、管道客户端
XIVSlothComboX.UI/             Vue 3 + TypeScript + Vite + Naive UI 前端
build/publish.ps1              装配 dist/ 安装包
docs/ACCEPTANCE.md             验收清单
lib/FFXIVClientStructs/        子模块（FCS），只读，不要改动
```

完整说明见 [`README.md`](README.md)。

## 动手前的几条硬约束

- **子模块只读。** `lib/FFXIVClientStructs` 是上游的子模块。改它之前先想清楚，
  它的改动不会随本仓库的提交走。
- **`src/Directory.Build.props` 不许挪到仓库根目录。** 根目录的 props 会同时作用到
  `lib/` 下的子模块工程，改变它们的编译行为。子模块的警告压制在根目录的
  `Directory.Build.targets` 里按工程名定向处理，别改成全局。
- **前端产物输出到 `XIVSlothComboX.UI/wwwroot`**，由 `build/publish.ps1` 收进安装包。
  改前端输出路径时，这两处要一起改。
- **注入链路的每一步都在游戏进程外做。** `SlothCombo.Host` 只负责注入和转发，
  真正需要游戏状态的逻辑在 `SlothCombo.Runtime` 里，经由命名管道派发到游戏主线程执行。
  不要把游戏状态的读取搬到 Host 侧。
- **钩子里抛出的异常不能带崩游戏。** 走 `DetourGuard.Report`，不要在 detour 里裸抛。
- **代码注释用英文。** 仓库里的注释是英文的，跟着写。
- **面向用户的文档用中文。** `README.md`、`docs/ACCEPTANCE.md` 是中文的。

## 提交与推送

- 分支：`备战8.0`（当前开发分支）。
- 提交信息用中文，格式是「做了什么」加一段说明为什么这么改。
- 推送走 `ssh.github.com:443`，密钥 `~/.ssh/Acer_8745h`，对应账号 44451516。
  仓库的 `core.sshCommand` 已经配好了。
- CI 在 push 到 `API11` / `CN` / `main` / `备战8.0` 时触发。
  提交信息里带 `[PUSH]` 时会额外发布一个 GitHub Release。
