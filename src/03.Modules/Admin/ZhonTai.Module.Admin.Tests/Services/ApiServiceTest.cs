using Xunit;
using ZhonTai.Module.Admin.Contracts.Http;

namespace ZhonTai.Module.Admin.Tests.Services;

public class ApiServiceTest : BaseTest
{
    private readonly IApiService _apiService;

    public ApiServiceTest()
    {
        _apiService = GetService<IApiService>();
    }

    [Fact]
    public async void GetAsync()
    {
        var res = await _apiService.GetAsync(161227168079941);
        Assert.True(res?.Id > 0);
    }
}