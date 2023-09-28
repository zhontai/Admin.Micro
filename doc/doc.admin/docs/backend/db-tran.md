# 数据库事务

## 使用事务

在服务方法上增加 **[AppTransaction]** 特性，使用当前项目主库事务

::: tip 提示

1. 事务可跨方法，支持同步和异步方法。
2. 如果服务是动态 Api，开启事务的方法需定义为`virtual`虚方法，才能正常使用事务。
   :::

例如下面的用户更新，存在多个 cud 操作，操作要么全部成功，要么全部失败。

```cs
[AppTransaction]
public virtual async Task UpdateAsync(UserUpdateInput input)
{
	if (!(input?.Id > 0))
	{
		//事务回滚
		throw ResultOutput.Exception("请选择用户");
	}

	//查询用户
	var user = await _userRepository.GetAsync(input.Id);
	if (!(user?.Id > 0))
	{
        //事务回滚
		throw ResultOutput.Exception("用户不存在");
		// throw new AppException("用户不存在");
	}

	//数据映射
	_mapper.Map(input, user);

	//更新用户
	await _userRepository.UpdateAsync(user);

	//删除用户角色
	await _userRoleRepository.DeleteAsync(a => a.UserId == user.Id);
	if (input.RoleIds != null && input.RoleIds.Any())
	{
		var roles = input.RoleIds.Select(a => new UserRoleEntity { UserId = user.Id, RoleId = a });
		//批量插入用户角色
		await _userRoleRepository.InsertAsync(roles);
	}
}
```

## 事务回滚

抛出友好异常（推荐）

```cs
throw ResultOutput.Exception(msg);
```

```cs
throw new AppException(msg);
```

## 提交事务

无异常抛出，将自动提交事务

## 事务属性

| 参数           | 说明         | 类型 | 默认值   |
| -------------- | ------------ | ---- | -------- |
| Propagation    | 事务传播方式 | enum | Required |
| IsolationLevel | 事务隔离级别 | enum | -        |

Propagation 事务传播方式可选值：

- Requierd：如果当前没有事务，就新建一个事务，如果已存在一个事务中，加入到这个事务中，默认的选择。
- Supports：支持当前事务，如果没有当前事务，就以非事务方法执行。
- Mandatory：使用当前事务，如果没有当前事务，就抛出异常。
- NotSupported：以非事务方式执行操作，如果当前存在事务，就把当前事务挂起。
- Never：以非事务方式执行操作，如果当前事务存在则抛出异常。
- Nested：以嵌套事务方式执行。

IsolationLevel 事务隔离级别可选值：

- Chaos：无法覆盖隔离级别更高的事务中的挂起的更改。
- ReadCommitted：在正在读取数据时保持共享锁，以避免脏读，但是在事务结束之前可以更改数据，从而导致不可重复的读取或幻像数据。
- ReadUncommitted：可以进行脏读，意思是说，不发布共享锁，也不接受独占锁。
- RepeatableRead：在查询中使用的所有数据上放置锁，以防止其他用户更新这些数据。 防止不可重复的读取，但是仍可以有幻像行。
- Serializable：在 DataSet 上放置范围锁，以防止在事务完成之前由其他用户更新行或向数据集中插入行。
- Snapshot：通过在一个应用程序正在修改数据时存储另一个应用程序可以读取的相同数据版本来减少阻止。 表示您无法从一个事务中看到在其他事务中进行的更改，即便重新查询也是如此。
- Unspecified：正在使用与指定隔离级别不同的隔离级别，但是无法确定该级别。
