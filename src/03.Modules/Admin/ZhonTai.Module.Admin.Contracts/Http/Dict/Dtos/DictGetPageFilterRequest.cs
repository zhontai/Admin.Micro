﻿namespace ZhonTai.Module.Admin.Contracts.Http;

public partial class DictGetPageFilterRequest
{
    /// <summary>
    /// 字典类型Id
    /// </summary>
    public long DictTypeId { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    public string Name { get; set; }
}