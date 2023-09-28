using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using OnceMi.AspNetCore.OSS;
using ZhonTai.Api.Core;
using ZhonTai.Module.Admin.HttpApi.Core.Configs;

namespace ZhonTai.Module.Admin.HttpApi.Core.Extensions;

public static class OSSServiceCollectionExtensions
{
    private static void CreateBucketName(IOSSServiceFactory oSSServiceFactory, OSSOption oSSOptions)
    {
        var oSSService = oSSServiceFactory.Create(oSSOptions.Provider.ToString());
        if (!oSSService.BucketExistsAsync(oSSOptions.BucketName).Result)
        {
            oSSService.CreateBucketAsync(oSSOptions.BucketName).Wait();
        }

        //设置Minio存储桶权限
        if (oSSOptions.Provider == OSSProvider.Minio)
        {
            var bucketName = oSSOptions.BucketName;
            var minioClient = new MinioClient()
                .WithEndpoint(oSSOptions.Endpoint)
                .WithCredentials(oSSOptions.AccessKey, oSSOptions.SecretKey);

            if (oSSOptions.Region.NotNull())
            {
                minioClient.WithRegion(oSSOptions.Region);
            }

            minioClient = minioClient.Build();
            //查看存储桶权限
            //var policy = minioClient.GetPolicyAsync(new GetPolicyArgs().WithBucket(bucketName)).Result;
            //设置存储桶权限，存储桶内的所有文件可以通过链接永久访问
            var policy = $@"{{""Version"":""2012-10-17"",""Statement"":[{{""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Action"":[""s3:GetBucketLocation""],""Resource"":[""arn:aws:s3:::{bucketName}""]}},{{""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Action"":[""s3:GetObject""],""Resource"":[""arn:aws:s3:::{bucketName}/*.*""]}}]}}";
            var setPolicyArgs = new SetPolicyArgs().WithBucket(bucketName).WithPolicy(policy);
            minioClient.SetPolicyAsync(setPolicyArgs).Wait();
        }
    }

    public static IServiceCollection AddMyOSS(this IServiceCollection services, IConfiguration configuration = null)
    {
        services.Configure<OSSConfig>(configuration ?? AppInfo.Configuration.GetSection("ApiConfig:OssConfig"));

        var oSSConfig = services.BuildServiceProvider().GetRequiredService<IOptions<OSSConfig>>().Value;
        if (oSSConfig.OSSConfigs?.Count > 0)
        {
            foreach (var oSSOptions in oSSConfig.OSSConfigs)
            {
                if (oSSOptions.Enable)
                {
                    services.AddOSSService(oSSOptions.Provider.ToString(), "ApiConfig:OssConfig");

                    var oSSServiceFactory = services.BuildServiceProvider().GetRequiredService<IOSSServiceFactory>();
                    CreateBucketName(oSSServiceFactory, oSSOptions);
                }
            }
        }

        return services;
    }
}
