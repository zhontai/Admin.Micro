using Xunit;
using ZhonTai.Api.Rpc.Dtos;
using ZhonTai.Module.Admin.Contracts.Http;

namespace ZhonTai.Module.Admin.Tests.Controllers;

public class ApiControllerTest : BaseControllerTest
{
    public ApiControllerTest() : base()
    {
    }

    [Fact]
    public async Task Get()
    {
        var res = await GetResult<Response<ApiGetResponse>>("/api/admin/api/get?id=161227167658053");
        Assert.True(res.Success);
    }

    [Fact]
    public async Task GetList()
    {
        var res = await GetResult<Response<List<ApiGetListResponse>>>("/api/admin/api/get-list?key=接口管理");
        Assert.True(res.Success);
    }

    [Fact]
    public async Task Add()
    {
        var input = new ApiAddRequest
        {
           Label = "新接口",
           Path = "/api/admin/api/newapi",
           HttpMethods = "post"
        };

        var res = await PostResult($"/api/admin/api/add", input);
        Assert.True(res.Success);
    }

    [Fact]
    public async Task Update()
    {
        var output = await GetResult<Response<ApiGetResponse>>("/api/admin/api/get?id=161227167658053");
        var res = await PutResult($"/api/admin/api/update", output.Data);
        Assert.True(res.Success);
    }

    [Fact]
    public async Task Delete()
    {
        var res = await DeleteResult($"/api/admin/api/soft-delete?{ToParams(new { id = 191182807191621 })}");
        Assert.True(res.Success);
    }
}