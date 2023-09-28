# 表实体

## 继承实体

### 主键实体

需要主键 Id 时继承`Entity`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : Entity
{
}
```

属性说明
| 属性 | 说明 | 类型 |
| -------------- | ------------ | ---- |
| Id | 主键 Id，默认雪花 Id | long |

### 创建实体

需要创建信息时继承`EntityAdd`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : EntityAdd
{
}
```

属性说明
| 属性 | 说明 | 类型 |
| -------------- | ------------ | ---- |
| Id | 主键 Id，默认雪花 Id | long |
| CreatedUserId | 创建者 Id，不可修改 | long? |
| CreatedUserName | 创建者, 长度 50，不可修改 | string |
| CreatedTime | 创建时间，不可修改 | DateTime? |

### 修改实体

需要修改信息时继承`EntityUpdate`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : EntityUpdate
{
}
```

属性说明
| 属性 | 说明 | 类型 |
| -------------- | ------------ | ---- |
| Id | 主键 Id，默认雪花 Id | long |
| CreatedUserId | 创建者 Id，不可修改 | long? |
| CreatedUserName | 创建者, 长度 50，不可修改 | string |
| CreatedTime | 创建时间，不可修改 | DateTime? |
| ModifiedUserId | 修改者 Id，不可创建 | long? |
| ModifiedUserName | 修改者, 长度 50，不可创建 | string |
| ModifiedTime | 修改时间，不可创建 | DateTime? |

### 删除实体

需要假删除时继承`EntityDelete`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : EntityDelete
{
}
```

属性说明
| 属性 | 说明 | 类型 |
| -------------- | ------------ | ---- |
| Id | 主键 Id，默认雪花 Id | long |
| CreatedUserId | 创建者 Id，不可修改 | long? |
| CreatedUserName | 创建者，长度 50，不可修改 | string |
| CreatedTime | 创建时间，不可修改 | DateTime? |
| ModifiedUserId | 修改者 Id，不可创建 | long? |
| ModifiedUserName | 修改者, 长度 50，不可创建 | string |
| ModifiedTime | 修改时间，不可创建 | DateTime? |
| IsDeleted | 是否删除，默认 false | bool |

### 版本实体

需要设置行锁（乐观锁）版本号时继承`EntityVersion`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : EntityVersion
{
}
```

属性说明
| 属性 | 说明 | 类型 |
| -------------- | ------------ | ---- |
| Id | 主键 Id，默认雪花 Id | long |
| Version | 版本，默认 0 | long |
| CreatedUserId | 创建者 Id，不可修改 | long? |
| CreatedUserName | 创建者，长度 50，不可修改 | string |
| CreatedTime | 创建时间，不可修改 | DateTime? |
| ModifiedUserId | 修改者 Id，不可创建 | long? |
| ModifiedUserName | 修改者, 长度 50，不可创建 | string |
| ModifiedTime | 修改时间，不可创建 | DateTime? |
| IsDeleted | 是否删除，默认 false | bool |

### 数据权限实体

需要数据权限时继承`EntityData`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : EntityData
{
}
```

属性说明
| 属性 | 说明 | 类型 |
| -------------- | ------------ | ---- |
| Id | 主键 Id，默认雪花 Id | long |
| OwnerId | 拥有者 Id | long? |
| OwnerOrgId | 拥有者部门 Id | long? |
| CreatedUserId | 创建者 Id，不可修改 | long? |
| CreatedUserName | 创建者，长度 50，不可修改 | string |
| CreatedTime | 创建时间，不可修改 | DateTime? |
| ModifiedUserId | 修改者 Id，不可创建 | long? |
| ModifiedUserName | 修改者, 长度 50，不可创建 | string |
| ModifiedTime | 修改时间，不可创建 | DateTime? |
| IsDeleted | 是否删除，默认 false | bool |

### 租户实体

需要隔离不同租户数据时继承`EntityTenant`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : EntityTenant
{
}
```

属性说明
| 属性 | 说明 | 类型 |
| -------------- | ------------ | ---- |
| Id | 主键 Id，默认雪花 Id | long |
| TenantId | 租户 Id，不可修改 | long? |
| CreatedUserId | 创建者 Id，不可修改 | long? |
| CreatedUserName | 创建者，长度 50，不可修改 | string |
| CreatedTime | 创建时间，不可修改 | DateTime? |
| ModifiedUserId | 修改者 Id，不可创建 | long? |
| ModifiedUserName | 修改者, 长度 50，不可创建 | string |
| ModifiedTime | 修改时间，不可创建 | DateTime? |
| IsDeleted | 是否删除，默认 false | bool |

### 租户数据权限实体

同时需要租户和数据权限时继承`EntityTenantWithData`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : EntityTenantWithData
{
}
```

属性说明
| 属性 | 说明 | 类型 |
| -------------- | ------------ | ---- |
| Id | 主键 Id，默认雪花 Id | long |
| TenantId | 租户 Id，不可修改 | long? |
| OwnerId | 拥有者 Id | long? |
| OwnerOrgId | 拥有者部门 Id | long? |
| CreatedUserId | 创建者 Id，不可修改 | long? |
| CreatedUserName | 创建者，长度 50，不可修改 | string |
| CreatedTime | 创建时间，不可修改 | DateTime? |
| ModifiedUserId | 修改者 Id，不可创建 | long? |
| ModifiedUserName | 修改者, 长度 50，不可创建 | string |
| ModifiedTime | 修改时间，不可创建 | DateTime? |
| IsDeleted | 是否删除，默认 false | bool |

### 会员实体

需要隔离不同会员数据时继承`EntityMember`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : EntityMember
{
}
```

属性说明
| 属性 | 说明 | 类型 |
| -------------- | ------------ | ---- |
| Id | 主键 Id，默认雪花 Id | long |
| MemberId | 会员 Id，不可修改 | long? |
| CreatedTime | 创建时间，不可修改 | DateTime? |
| ModifiedTime | 修改时间，不可创建 | DateTime? |
| IsDeleted | 是否删除，默认 false | bool |

### 会员租户实体

同时需要会员和租户时继承`EntityMemberWithTenant`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : EntityMemberWithTenant
{
}
```

属性说明
| 属性 | 说明 | 类型 |
| -------------- | ------------ | ---- |
| Id | 主键 Id，默认雪花 Id | long |
| TenantId | 租户 Id，不可修改 | long? |
| MemberId | 会员 Id，不可修改 | long? |
| CreatedTime | 创建时间，不可修改 | DateTime? |
| ModifiedTime | 修改时间，不可创建 | DateTime? |
| IsDeleted | 是否删除，默认 false | bool |

## 继承接口

### 添加接口

需要创建信息时继承`IEntityAdd<TKey>`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : IEntityAdd<long>
{
    /// <summary>
    /// 创建者Id
    /// </summary>
    [Description("创建者Id")]
    [Column(Position = -22, CanUpdate = false)]
    public virtual long? CreatedUserId { get; set; }

    /// <summary>
    /// 创建者
    /// </summary>
    [Description("创建者")]
    [Column(Position = -21, CanUpdate = false), MaxLength(50)]
    public virtual string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Description("创建时间")]
    [Column(Position = -20, CanUpdate = false, ServerTime = DateTimeKind.Local)]
    public virtual DateTime? CreatedTime { get; set; }
}
```

### 修改接口

需要修改信息时继承`IEntityUpdate<TKey>`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : IEntityUpdate<long>
{
    /// <summary>
    /// 修改者Id
    /// </summary>
    [Description("修改者Id")]
    [Column(Position = -12, CanInsert = false)]
    [JsonProperty(Order = 10000)]
    [JsonPropertyOrder(10000)]
    public virtual long? ModifiedUserId { get; set; }

    /// <summary>
    /// 修改者
    /// </summary>
    [Description("修改者")]
    [Column(Position = -11, CanInsert = false), MaxLength(50)]
    [JsonProperty(Order = 10001)]
    [JsonPropertyOrder(10001)]
    public virtual string ModifiedUserName { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    [Description("修改时间")]
    [JsonProperty(Order = 10002)]
    [JsonPropertyOrder(10002)]
    [Column(Position = -10, CanInsert = false, ServerTime = DateTimeKind.Local)]
    public virtual DateTime? ModifiedTime { get; set; }
}
```

### 删除接口

需要假删除时继承`IDelete`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : IDelete
{
    /// <summary>
    /// 是否删除
    /// </summary>
    [Description("是否删除")]
    [Column(Position = -9)]
    public virtual bool IsDeleted { get; set; } = false;
}
```

### 版本接口

需要设置行锁（乐观锁）版本号时继承 `IVersion`

> 每次修改整个实体时会附带当前的版本号判断（修改成功累加版本号，失败时抛出异常）

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : IVersion
{
    /// <summary>
    /// 版本
    /// </summary>
    [Description("版本")]
    [Column(Position = -30, IsVersion = true)]
    public virtual long Version { get; set; }
}
```

### 租户接口

需要隔离不同租户数据时继承`ITenant`

> 按租户字段区分

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : ITenant
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [Description("租户Id")]
    [Column(Position = 2, CanUpdate = false)]
    [JsonProperty(Order = -20)]
    [JsonPropertyOrder(-20)]
    public virtual long? TenantId { get; set; }
}
```

### 数据权限接口

需要数据权限时继承`IData`

> 可在角色管理中设置数据权限：本部门、本部门和下级部门、指定部门、本人数据

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : IData
{
    /// <summary>
    /// 拥有者Id
    /// </summary>
    [Description("拥有者Id")]
    [Column(Position = -41)]
    public virtual long? OwnerId { get; set; }

    /// <summary>
    /// 拥有者部门Id
    /// </summary>
    [Description("拥有者部门Id")]
    [Column(Position = -40)]
    public virtual long? OwnerOrgId { get; set; }
}
```

### 会员接口

需要隔离不同会员数据时继承`IMember`

```cs
/// <summary>
/// 模块
/// </summary>
[Table(Name = "app_module")]
public class ModuleEntity : IMember
{
    /// <summary>
    /// 会员Id
    /// </summary>
    [Description("会员Id")]
    [Column(Position = -23, CanUpdate = false)]
    public virtual long? MemberId { get; set; }
}
```
