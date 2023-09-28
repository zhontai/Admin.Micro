namespace ZhonTai.Api.Rpc.Dtos;

/// <summary>
/// 分页信息响应
/// </summary>
public class PageResponse<T>
{
    /// <summary>
    /// 数据总数
    /// </summary>
    public long Total { get; set; } = 0;

    /// <summary>
    /// 数据
    /// </summary>
    public IList<T> List { get; set; }
}