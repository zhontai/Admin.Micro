using System.Threading.Tasks;
using ZhonTai.Api.Core.Configs;

namespace ZhonTai.Api.Core.Db.Data;

/// <summary>
/// 生成数据接口
/// </summary>
public interface IGenerateData
{
    Task GenerateDataAsync(IFreeSql db, AppConfig appConfig);
}
