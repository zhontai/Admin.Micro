namespace ZhonTai.Api.Rpc.Configs;

/// <summary>
/// 远程过程调用配置
/// </summary>
public class RpcConfig
{
    /// <summary>
    /// 地址列表
    /// </summary>
    public List<Address> AddressList { get; set; }
}

/// <summary>
/// 地址
/// </summary>
public class Address
{
    /// <summary>
    /// 模块名
    /// </summary>
    public string ModuleName { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    public string ApiUrl { get; set; }

    /// <summary>
    /// Grpc地址
    /// </summary>
    public string GrpcUrl { get; set; }
}