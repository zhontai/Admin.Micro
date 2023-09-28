using System.Threading.Tasks;
using ZhonTai.Api.Core.Configs;

namespace ZhonTai.Api.Core.Db.Data;

/// <summary>
/// 同步数据接口
/// </summary>
public interface ISyncData
{
    Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null);
}
