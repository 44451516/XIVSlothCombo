# 轮转协议与 Runtime 存储

本目录保存独立编辑器协议的固定副本。`rotation.schema.json` 同时描述计划、
编辑器项目和参考目录；Runtime 导入接口**只接受单个计划**。
结构校验实际使用 JsonSchema.Net 9.4.1，再进行下述语义校验。

## 文件与资源

| 文件 / 资源名 | 用途 |
| --- | --- |
| `rotation.schema.json` / `SlothCombo.Rotations.Schema` | 随 Runtime DLL 内嵌的固定 Schema；所有 `$ref` 均为本文件内引用，不访问远程 registry |
| `catalog.rotation-catalog.json` / `SlothCombo.Rotations.Catalog` | 国服优先的参考目录；未认证为国服当前版本 |
| `Initialize(folder)` 指定目录下的 `rotation-store.json` | 用户计划、逐职业模式、活动计划及参数覆盖值 |

Schema 必须与独立编辑器 `schema/rotation.schema.json` 保持逐字节一致。
缺少 Schema 时不跳过校验；缺少或损坏 catalog 时返回明确标记的空数据 fallback，
其中列出首版支持的 22 个职业，不伪造技能、特性或状态记录。

## 计划结构

根对象固定为：

```json
{
  "format": "xiv-rotation",
  "version": 1,
  "id": "plan-id",
  "name": "计划名称",
  "jobId": 21,
  "entryPhaseId": "phase-id",
  "parameters": [],
  "parameterValues": {},
  "variables": [],
  "phases": []
}
```

上例仅展示根字段，实际计划必须至少包含一个阶段，且入口必须存在。
首版职业白名单为 `19,20,21,22,23,24,25,27,28,30,31,32,33,34,35,36,37,38,39,40,41,42`；
不接受生产、采集、基础职业或尚未确认的新职业。

- `parameters`：`{id,name,type,default,min?,max?}`；`type` 仅为 `number` 或 `boolean`。
- `parameterValues`：已声明参数的覆盖值；缺省项使用默认值，未知参数和越界值拒绝。
- `variables`：`{id,name,type,default}`；与参数的区别是可由阶段和动作修改。
- `phases`：`{id,name,onEnter,transitions,rules}`。
- `onEnter` 与 `onSuccess`：`{variableId,operation,value}` 数组，操作仅为 `set`、`add`；`add` 只用于数值变量。
- `transitions`：`{id,toPhaseId,condition}`，目标阶段必须存在。
- `rules`：`{id,name,enabled,condition,action? ,sequence?}`，`action` 与 `sequence` 必须恰有一个。
- `sequence`：`{id,loop,timeoutMs,timeoutPolicy,steps}`；策略仅为 `abort`、`skip`。
- `steps`：`{id,condition,action}` 数组。

稳定实体 ID 在单个计划内全局唯一，包含计划、参数、变量、阶段、跳转、规则、
序列、步骤及每个条件。`Value.id`、`variableId`、`toPhaseId` 等引用不计为新实体。
ID 匹配 `^[A-Za-z0-9][A-Za-z0-9_-]{0,79}$`。

## 条件与值

`Value` 必须恰为以下一种：

```json
{"kind":"literal","value":70}
{"kind":"field","path":"party.lowestHp"}
{"kind":"parameter","id":"hpThreshold"}
{"kind":"variable","id":"counter"}
```

字面量只允许有限数值或布尔值；参数和变量引用必须存在。
条件有稳定 `id`，并且只允许：

- `constant`：`{id,kind,value:boolean}`。
- `all`、`any`：`{id,kind,children:Condition[]}`，子条件数量 1–64。
- `not`：同样使用 `children`，但必须恰有一个子条件。
- `compare`：`{id,kind,left,op,right}`；运算符为 `eq/ne/gt/gte/lt/lte`。

比较两侧类型必须一致，布尔值只支持 `eq/ne`。未知种类、未知属性、未知字段、
类型不符、重复 JSON 属性和不安全的属性名不会静默忽略。

### 运行中的未知条件

条件按从左到右短路求值；已经短路跳过的子条件不会读取字段。
实际求值时，缺失目标、当前职业不提供的量谱、不可解码的状态或其他不可用值
属于“未知”，不是 `false`，也不会在 `not` 中变成 `true`。
Runtime 将异常交给主线程 Update 安全停止整个引擎；不继续低优先级规则、
后续阶段转换或序列步骤，避免高优先级治疗条件缺失后落入下方攻击规则。

安全停止会清空阶段、变量、活动/挂起序列及 pending，保留 `reason` 和
`unavailableFields` 诊断；修复数据来源或计划后必须显式重新开始。
`unknownConditionPolicy` 状态字段标记这一策略。它不取消已经发送或开始施放的原生动作。
离线调试中的未知条件同样应中止当前步且不产生低优先级候选；人工快照不能替代原生合法性检查。

## 字段类型

| 字段 | 类型 |
| --- | --- |
| `player.level/hpPercent/mp` | number |
| `player.moving/inCombat` | boolean |
| `target.hpPercent/distance/enemyCount` | number |
| `combo.action/remaining`、`lastAction` | number |
| `countdown.remaining` | number |
| `party.count/lowestHp` | number |
| `action.<ID>.ready` | boolean |
| `action.<ID>.cooldown/charges` | number |
| `status.self.<ID>.remaining/stacks`、`status.target.<ID>.remaining/stacks` | number |
| `status.self.<ID>.mine`、`status.target.<ID>.mine` | boolean |
| `gauge.<Name>` | 按 Runtime Gauge 标量属性的真实类型确定，区分 boolean/number，不接受任意名称 |

大小写敏感。动态动作、状态 ID 必须是非零 uint。
`RotationValidator.GetFieldType` 提供与校验一致的字段类型；catalog 响应中的
`runtimeFields` 和 `runtimeFieldPatterns` 导出该类型表。
例如 `gauge.HasDarkArts` 为 boolean，`gauge.Blood`、`gauge.Ammo` 为 number。
量谱数组、指针及非标量属性不在白名单内；当前职业无法提供的字段由引擎拒绝，
不能以零值冒充。目录不是客户端版本认证依据。

## 动作与限制

动作结构为 `{actionId,target,ground,allowPrepull?,onSuccess}`。
`target.kind` 仅为 `current/self/partySlot/lowestHp/role`；
`partySlot` 必须提供 1–8 的 `slot`，`role` 必须提供 `tank/healer/dps`。
其他种类不得夹带 `slot` 或 `role`，可选 `dead` 必须是 boolean。
`ground` 仅为 `none/self/target`；实际目标、技能与地面落点适用性仍由引擎检查。

当前能力限制：

- 地面技能要求存在当前敌方目标；锚点仅限自身或该敌方目标，友方地面锚点尚未实现。
- 无法可靠分类的自身技能在没有当前敌方目标时拒绝执行，不能用网页说明推断其安全性。
- 所有脱战动作都要求有效倒计时及显式 `allowPrepull:true`，不仅攻击动作。
- 停止不取消已经提交的原生动作或读条；不可将该行为等同于原生施法取消。
- 离线调试不实现原生目标、地形、动作队列或动作确认包；候选不等于游戏内动作可执行。

- 不再设置单方案专属的 1 MiB 文件大小上限；方案与其他文档共用 32 MiB 底层处理容量保护。
  IPC 完整帧及完整存储文件仍各受 32 MiB 约束，信封、已有方案等会占用容量，
  因此不能承诺接近 32 MiB 的单方案必定能传输和保存。格式、引用及运行安全校验不放宽。
  Host 对请求体流式计数，并检查实际序列化的完整请求；Store 在停止和保存前检查完整快照响应。
  IPC 容量超限返回 `413 / IPC_MESSAGE_TOO_LARGE`，不伪装成游戏未就绪；写帧前也执行同样检查。
- JSON 深度上限 128，总 JSON 节点上限 200000；根节点深度从 0 计。
- 条件根深度从 0 计，上限 32；整个计划条件数量上限 8192。
- 序列步骤 1–256；`timeoutMs` 为 50–300000 的整数，常用示例值为 10000。
- `actionId` 和 JSON 的职业数字类型上限为 4294967295；职业另外受白名单约束。
- 阶段最多 128，每阶段规则最多 1000；参数、变量各最多 128。
- Store 最多 256 个计划，完整存储文件最多 32 MiB。

## 持久化与安全关闭

`GetActive`、`GetSnapshot` 返回内存状态的深拷贝，不逐帧读盘。
模式和选择分别持久化；选择计划不自动改为 v8，导入也不自动选择或开始。
`SaveParameters` 替换所选计划的整个参数覆盖对象，省略的项回到默认值。

快照、引擎状态读取及所有变更经游戏主线程 dispatcher；变更先准备、验证和序列化候选状态，
再停止引擎，以 UTF-8 无 BOM 写同目录临时文件、Flush 到磁盘、原子 Replace/Move，
最后发布新内存状态。无效导入和未授权覆盖在停止引擎前失败。
文件写入失败时原存储和原内存副本保持不变，但引擎保持停止。

首次无存储文件时生成 WAR 动作 31 / current、WHM 动作 120 / lowestHp 两个结构示例。
后者有血量阈值参数；两个示例的规则均 `enabled:false`，模式均为 legacy，
活动计划为空，不自动开始。示例不宣称为经国服版本认证的生产循环。

删除活动计划只清空 `activePlanId`，**保留 v8 模式**；只有用户显式切换 legacy 才恢复旧循环。
覆盖同 ID 的计划不能改变其职业。

已有存储损坏、不支持的版本或初始化失败时，不覆盖原文件：
`initializationError` 保留原因，`failClosed:true`，22 个支持职业的 `IsV8` 返回 true，
阻止旧 PvE 无声恢复；生产、采集和基础职业不受该关闭状态影响。
需要用户修复原文件并重新初始化；普通 API 变更不能抹掉故障。

参考目录只包含内嵌静态资料和字段类型元数据；量谱只反射属性类型，不实例化或读取游戏量谱。
`ExportCatalog` 使用独立 Lazy 缓存，不持有方案状态锁，返回深拷贝。
`GET /api/rotations/catalog` 不经 dispatcher：首次在线程池读取资源、解析目录、
转换 JSON 并序列化完整响应，后续复用不可变响应字符串；并发首次请求共用同一任务。
目录缓存与 Runtime DLL 生命周期一致，重新初始化用户存储不会重复解析目录。
动态状态仍在 Runtime 游戏主线程读取，不移至 Host。

## API

沿用现有 `{ok,data,error}` 响应信封，System.Text.Json 节点会转换为现有响应序列化格式。

| 接口 | 内容 |
| --- | --- |
| `GET /api/rotations` | `{plans,jobs,format,version,failClosed,initializationError}` 快照；存储故障仍为 200、`ok:true`；`jobs` 以职业 ID 字符串为键，值为 `{mode,activePlanId}` |
| `POST /api/rotations` | `operation: import` + `plan` + 可选 boolean `overwrite` |
| 同上 | `operation: mode` + `jobId` + `mode: legacy/v8` |
| 同上 | `operation: select` + `jobId` + `planId` |
| 同上 | `operation: parameters` + `jobId` + `values` |
| 同上 | `operation: delete` + `planId` |
| `GET /api/rotations/catalog` | 内嵌参考目录及 Runtime 类型表；未认证版本固定 `certification:reference`、`certified:false`、`referenceOnly:true` |
| `GET /api/rotations/state` | 引擎状态 |
| `POST /api/rotations/control` | `command:start/stop/reset`，可选字符串 `emergencyHotkey`；start 必须带 `expectedJobId`，Runtime 主线程核对真实职业并绑定启动职业；stop/reset 不接受该字段。reset 停止并解除急停锁存，不自动开始 |

请求信封同样拒绝未知字段。格式、引用、类型错误及无效引擎控制返回 400，
重复计划且未允许覆盖、覆盖时更换职业返回 409，缺失计划或路径返回 404，
不支持的方法返回 405，真实存储 I/O 错误返回 500。start 缺失或非法职业返回 400，
与游戏实际职业不符返回 409；职业校验在停止当前引擎前完成。
完整请求或响应超过 IPC 底层容量返回 413；提交前预检失败不会停止引擎、写盘或发布候选状态。
已有存储不能生成容量内的完整快照时，初始化保留原文件并进入可读的 fail-closed。
存储故障属于可读状态：快照 GET 返回 200、`ok:true`，`data` 保留
`failClosed:true` 和 `initializationError`，主界面可正常读取并显示锁定原因。
启动和 POST 变更仍返回 400；停止操作仍然可用，不解除存储故障。

真实游戏验收项见 `docs/ACCEPTANCE.md`，不能用编译成功代替游戏内确认。
