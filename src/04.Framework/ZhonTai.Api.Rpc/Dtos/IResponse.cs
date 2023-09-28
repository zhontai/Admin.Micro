namespace ZhonTai.Api.Rpc.Dtos;

/// <summary>
/// 响应接口
/// </summary>
public interface IResponse
{
    /// <summary>
    /// 是否成功
    /// </summary>
    bool Success { get; }

    /// <summary>
    /// 消息
    /// </summary>
    string Msg { get; }

    /// <summary>
    /// 编码
    /// </summary>
    string Code { get; set; }
}

/// <summary>
/// 泛型响应接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResponse<T> : IResponse
{
    /// <summary>
    /// 返回数据
    /// </summary>
    T Data { get; }
}