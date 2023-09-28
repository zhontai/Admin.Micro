﻿using ZhonTai.Api.Rpc.Enums;

namespace ZhonTai.Api.Rpc.Dtos;

public class DataPermissionResponse
{
    /// <summary>
    /// 部门Id
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    public List<long> OrgIds { get; set; }

    /// <summary>
    /// 数据范围
    /// </summary>
    public DataScope DataScope { get; set; } = DataScope.Self;
}