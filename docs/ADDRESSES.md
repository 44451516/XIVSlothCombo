# 内存地址清单

这个程序要读游戏进程里的东西，就得知道那些东西在内存里的位置。
这份文档记着**所有需要内存地址的地方**，以及地址是怎么来的、坏了会怎样。

地址本身全部写在一张表里：[`src/SlothCombo.Interop/GameAddresses.cs`](../src/SlothCombo.Interop/GameAddresses.cs)，
另加 [`src/SlothCombo.Runtime/Core/HookAddress.cs`](../src/SlothCombo.Runtime/Core/HookAddress.cs)
里三个只给钩子用的签名。

---

## 地址是怎么来的

程序不写死绝对地址，而是给每一处地址配一段**特征码**（字节模式）。运行时启动时
在游戏的代码段里搜这段模式，搜到之后再按模式前面的指令算出真正的地址。

两种形式：

- **成员函数**（`GameAddress.Member`）：特征码以 `E8`（call）或 `E9`（jmp）开头，
  跟到那个 call 跳过去的目标就是函数的地址。游戏里大部分函数都是这么被调用的。
- **静态地址**（`GameAddress.Static`）：特征码指向一处存放地址的地方。
  `relativeFollowOffset` 是那个距离在模式里的位置，`isPointer` 表示那里存的
  是不是一个还需要再解一层引用的指针。

解析结果写在数据目录的 `addresses.json` 里，下一次启动直接复用，只重搜失效的那几条。
文件里带版本号（`GameAddressTable.CacheVersion`），版本一改整份作废重搜 ——
这是修好被改版弄坏的地址后，让它重新生效的正规做法。

扫不到的地址进 `GameAddressTable.Unresolved`，**只记日志，由用到它的功能自己降级**，
不会带崩游戏。

---

## 一、静态地址（12 个）

拿到的是管理器一类的实例指针。

| 名称 | 拿到什么 | 支撑什么功能 |
| --- | --- | --- |
| `Framework` | 框架实例 | 几乎所有读取的根；UI 各模块都挂在它下面 |
| `FrameworkVirtualTable` | 框架虚表 | `FrameworkTickHook` 的每帧 tick |
| `ActionManager` | 动作管理器 | 施放技能、查 CD 与层数 |
| `CharacterManager` | 角色表 | 玩家、怪物、NPC |
| `ClientObjectManager` | 客户端对象表 | 客户端自建的对象 |
| `InventoryManager` | 背包 | 物品、消耗品 |
| `TargetSystem` | 目标系统 | 选中目标、鼠标指向 |
| `GroupManager` | 队伍 | 队伍成员 |
| `GameMain` | 游戏主状态 | 区域、PvP 判断 |
| `Conditions` | 条件标志位（112 字节） | 各种战斗/职业条件 |
| `UIState` | 界面状态 | 任务链解锁 |
| `JobGaugeManager` | 职业量表 | 21 个职业的资源 |

## 二、成员函数（23 个）

| 分组 | 名称 | 干什么 |
| --- | --- | --- |
| 技能施放与状态 | `UseAction` | 施放技能 |
| | `UseActionLocation` | 在指定位置施放 |
| | `GetActionStatus` | 技能当前状态 |
| | `GetAdjustedActionId` | 调整后的技能编号 |
| | `GetRecastTimeElapsed` | 已经过的 CD 时间 |
| | `GetCurrentCharges` | 当前层数 |
| | `GetAdjustedRecastTime` | 调整后的 CD |
| | `GetMaxCharges` | 最大层数 |
| | `GetActionCost` | 技能消耗 |
| | `GetActionInRangeOrLoS` | 是否在射程或视线内 |
| | `CheckActionResources` | 资源是否够 |
| | `ActionEffectHandlerReceive` | 接收技能效果 |
| 背包与任务 | `GetInventoryContainer` | 取某种背包容器 |
| | `GetInventoryItemCount` | 某种物品的数量 |
| | `IsUnlockLinkUnlockedOrQuestCompleted` | 任务链是否解锁 |
| PvP | `IsInPvPArea` | 是否在 PvP 区域 |
| | `IsInPvPInstance` | 是否在 PvP 副本内 |
| 界面 | `RaptureLogModulePrintString` | 往聊天里写一行 |
| | `AgentModuleGetAgentByInternalId` | 按编号取界面部件 |
| 目标 | `TargetSystemGetTargetObject` | 当前目标 |
| | `TargetSystemGetSoftTarget` | 软目标 |
| | `TargetSystemGetHardTarget` | 硬目标 |
| | `TargetSystemSetHardTarget` | 指定硬目标 |

## 三、只给钩子用的独立签名（3 个）

这些不在地址表里，因为只有钩子需要它们。

| 名称 | 用途 | 用在哪 |
| --- | --- | --- |
| `ActionIdReplaceable` | 技能编号能否被替换 | `IconReplacer` 的 `isIconReplaceableHook` |
| `SendAction` | 发送技能 | `ActionWatching` 的 `SendActionHook` |
| `GetGameObjectFromPronounID` | 代词编号 → 对象 | `PartyTargetingService` |

另外三个钩子直接复用地址表的条目：`ReceiveActionEffect`（→ `ActionEffectHandlerReceive`）、
`UseActionLocation`、`GetAdjustedActionId`。

## 四、硬编码的偏移（3 个）

这些不靠扫签名，是写死的常量。它们的值是相对某个基址的偏移。

| 位置 | 常量 | 值 | 是什么 |
| --- | --- | --- | --- |
| `src/SlothCombo.Interop/UI/Framework.cs` | `RaptureLogModuleOffset` | `0x1AC0` | 聊天模块在框架里的位置 |
| | `PronounModuleOffset` | `0xBADA0` | 代词模块在框架里的位置 |
| `src/SlothCombo.Interop/Gauges/JobGauges.cs` | `SharedGaugesOffset` | `0x08` | 各职业量表共享区的位置 |

---

## 加一个新地址

**一、在 `GameAddresses.cs` 里加名字常量与注册。**

```csharp
/// <summary>The function that tells whether the player is mounted.</summary>
public const string IsMounted = "IsMounted";

// 注册：在 Register 方法里加一行
table.Add(GameAddress.Member(IsMounted, "E8 ?? ?? ?? ?? 84 C0 74"));
```

静态地址这样写：

```csharp
// 特征码 + 距离在模式里的位置 + 那里是不是一个还要再解的指针
table.Add(GameAddress.Static(SomeManager, "48 8D 0D ?? ?? ?? ?? 81 C2", 3));
```

**二、在结构体上加入口。**

```csharp
/// <summary>Gets the manager of the something.</summary>
/// <returns>The manager, or <c>null</c> when the game is not ready.</returns>
public static SomethingManager* Instance()
    => (SomethingManager*)GameFunctions.Get(GameAddresses.SomethingManager);
```

**三、调用时一定要判空。**

`GameFunctions.Get(name)` 在没解析到时返回 `0`，直接解引用就是访问野指针。
判空之后走降级路径，别让一个功能拖垮整个运行时。

```csharp
var manager = SomethingManager.Instance();
if (manager is null)
{
    return false;   // 这个功能这次不工作，但别的照常
}
```

**四、如果是钩子**，在 `HookAddress.cs` 里加签名，用
`Service.GameInteropProvider.HookFromSignature<TDelegate>(签名, detour)` 或
`HookFromAddress<TDelegate>(地址, detour)` 安装。

---

## 风险：两种坏法不一样

**签名扫不到 = 降级，安全。**
游戏改版后特征码大概率变了，扫不到就走降级路径，功能失效但不会出错。

**结构体偏移错了 = 静默读错内存，危险。**
特征码照样能扫到，偏移却变了，于是读到的是别的数据 —— 程序不报错，数据却是错的。
这类地方没有兜底，只能靠实机验证。

同理，`GameAddress.Member` 依赖 call 指令后面的那几字节上下文
（比如 `"E8 ?? ?? ?? ?? 48 8B BC 24 ?? ?? ?? ?? 44 0F B6 F8 B0"` 里 `call` 之后的
`48 8B BC 24`），这部分是纯上下文字节，改版时最容易对不上。

---

## 出问题时先看哪里

1. 运行日志里搜 `Unresolved address:` —— 有记录说明那个签名这次没扫到。
2. 数据目录下的 `addresses.json` —— 看扫到了多少条，和上面的表对得上就说明扫描本身没问题。
3. 某个功能表现不对、但日志里没有 unresolved —— 多半是**偏移或结构体布局**变了，
   按上一节说的那种危险情况排查。
