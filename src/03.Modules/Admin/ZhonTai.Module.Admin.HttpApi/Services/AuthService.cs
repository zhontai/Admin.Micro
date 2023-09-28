using System.Diagnostics;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using ZhonTai.Api.Core.Auth;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Api.Core.Configs;
using ZhonTai.Api.Core.Consts;
using ZhonTai.Module.Admin.HttpApi.Domain.Permission;
using ZhonTai.Module.Admin.HttpApi.Domain.User;
using ZhonTai.Module.Admin.HttpApi.Domain.Tenant;
using ZhonTai.Module.Admin.HttpApi.Domain.RolePermission;
using ZhonTai.Module.Admin.HttpApi.Domain.UserRole;
using ZhonTai.Utils.Extensions;
using ZhonTai.Utils.Helpers;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using FreeSql;
using ZhonTai.Module.Admin.HttpApi.Domain.TenantPermission;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Lazy.SlideCaptcha.Core.Validator;
using static Lazy.SlideCaptcha.Core.ValidateResult;
using ZhonTai.Module.Admin.HttpApi.Domain.PkgPermission;
using ZhonTai.Module.Admin.HttpApi.Domain.TenantPkg;
using Microsoft.Extensions.Options;

namespace ZhonTai.Module.Admin.HttpApi.Services;

/// <summary>
/// 认证授权服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class AuthService : BaseService, IAuthService, IDynamicApi
{
    private readonly AppConfig _appConfig;
    private readonly JwtConfig _jwtConfig;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITenantRepository _tenantRepository;
    private IPasswordHasher<UserEntity> _passwordHasher => LazyGetRequiredService<IPasswordHasher<UserEntity>>();
    private ISlideCaptcha _captcha => LazyGetRequiredService<ISlideCaptcha>();

    public AuthService(
        AppConfig appConfig,
        IOptions<JwtConfig> jwtConfig,
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        ITenantRepository tenantRepository
    )
    {
        _appConfig = appConfig;
        _jwtConfig = jwtConfig.Value;
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
        _tenantRepository = tenantRepository;
    }

    /// <summary>
    /// 获得token
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    [NonAction]
    public string GetToken(AuthGetTokenRequest user)
    {
        if (user == null)
        {
            return string.Empty;
        }

        var claims = new List<Claim>()
       {
            new Claim(ClaimAttributes.UserId, user.Id.ToString(), ClaimValueTypes.Integer64),
            new Claim(ClaimAttributes.UserName, user.UserName),
            new Claim(ClaimAttributes.Name, user.Name),
            new Claim(ClaimAttributes.UserType, user.Type.ToInt().ToString(), ClaimValueTypes.Integer32),
        };

        if (_appConfig.Tenant)
        {
            claims.AddRange(new []
            {
                new Claim(ClaimAttributes.TenantId, user.TenantId.ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimAttributes.TenantType, user.Tenant?.TenantType.ToInt().ToString(), ClaimValueTypes.Integer32),
                new Claim(ClaimAttributes.DbKey, user.Tenant?.DbKey ?? "")
            });
        }

        var token = LazyGetRequiredService<IUserToken>().Create(claims.ToArray());

        return token;
    }

    /// <summary>
    /// 查询密钥
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<AuthGetPasswordEncryptKeyResponse> GetPasswordEncryptKeyAsync()
    {
        //写入Redis
        var guid = Guid.NewGuid().ToString("N");
        var key = CacheKeys.PassWordEncrypt + guid;
        var encyptKey = StringHelper.GenerateRandom(8);
        await Cache.SetAsync(key, encyptKey, TimeSpan.FromMinutes(5));
        return new AuthGetPasswordEncryptKeyResponse { Key = guid, EncyptKey = encyptKey };
    }

    /// <summary>
    /// 查询用户个人信息
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<AuthUserProfileDto> GetUserProfileAsync()
    {
        if (!(User?.Id > 0))
        {
            throw Response.Exception("未登录");
        }

        using (_userRepository.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        {
            var profile = await _userRepository.GetAsync<AuthUserProfileDto>(User.Id);

            return profile;
        }
    }
   
    /// <summary>
    /// 查询用户菜单列表
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<List<AuthUserMenuDto>> GetUserMenusAsync()
    {
        if (!(User?.Id > 0))
        {
            throw Response.Exception("未登录");
        }

        using (_userRepository.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        {
            var menuSelect = _permissionRepository.Select;

            if (!User.PlatformAdmin)
            {
                var db = _permissionRepository.Orm;
                if (User.TenantAdmin)
                {
                    menuSelect = menuSelect.Where(a =>
                       db.Select<TenantPermissionEntity>()
                       .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                       .Any()
                       ||
                       db.Select<TenantPkgEntity, PkgPermissionEntity>()
                       .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                       .Any()
                   );
                }
                else
                {
                    menuSelect = menuSelect.Where(a =>
                       db.Select<RolePermissionEntity>()
                       .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                       .Where(b => b.PermissionId == a.Id)
                       .Any()
                   );
                }

                menuSelect = menuSelect.AsTreeCte(up: true);
            }

            var menuList = await menuSelect
                .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
                .ToListAsync(a => new AuthUserMenuDto { ViewPath = a.View.Path });

            return menuList.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList();

        }
    }

    /// <summary>
    /// 查询用户权限列表
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<AuthGetUserPermissionsResponse> GetUserPermissionsAsync()
    {
        if (!(User?.Id > 0))
        {
            throw Response.Exception("未登录");
        }

        using (_userRepository.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        {
            var authGetUserPermissionsResponse = new AuthGetUserPermissionsResponse
            {
                //用户信息
                User = await _userRepository.GetAsync<AuthUserProfileDto>(User.Id)
            };

            var dotSelect = _permissionRepository.Select.Where(a => a.Type == PermissionType.Dot);

            if (!User.PlatformAdmin)
            {
                var db = _permissionRepository.Orm;
                if (User.TenantAdmin)
                {
                    dotSelect = dotSelect.Where(a =>
                       db.Select<TenantPermissionEntity>()
                       .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                       .Any()
                       ||
                       db.Select<TenantPkgEntity, PkgPermissionEntity>()
                       .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                       .Any()
                    );
                }
                else
                {
                    dotSelect = dotSelect.Where(a =>
                        db.Select<RolePermissionEntity>()
                        .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                        .Where(b => b.PermissionId == a.Id)
                        .Any()
                    );
                }
            }

            //用户权限点
            authGetUserPermissionsResponse.Permissions = await dotSelect.ToListAsync(a => a.Code);

            return authGetUserPermissionsResponse;
        }
    }

    /// <summary>
    /// 查询用户信息
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<AuthGetUserInfoResponse> GetUserInfoAsync()
    {
        if (!(User?.Id > 0))
        {
            throw Response.Exception("未登录");
        }

        using (_userRepository.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
        {
            var authGetUserInfoResponse = new AuthGetUserInfoResponse
            {
                //用户信息
                User = await _userRepository.GetAsync<AuthUserProfileDto>(User.Id)
            };

            var menuSelect = _permissionRepository.Select;
            var dotSelect = _permissionRepository.Select.Where(a => a.Type == PermissionType.Dot);

            if (!User.PlatformAdmin)
            {
                var db = _permissionRepository.Orm;
                if (User.TenantAdmin)
                {
                    menuSelect = menuSelect.Where(a =>
                       db.Select<TenantPermissionEntity>()
                       .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                       .Any()
                       ||
                       db.Select<TenantPkgEntity, PkgPermissionEntity>()
                       .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                       .Any()
                   );

                    dotSelect = dotSelect.Where(a =>
                       db.Select<TenantPermissionEntity>()
                       .Where(b => b.PermissionId == a.Id && b.TenantId == User.TenantId)
                       .Any()
                       ||
                       db.Select<TenantPkgEntity, PkgPermissionEntity>()
                       .Where((b, c) => b.PkgId == c.PkgId && b.TenantId == User.TenantId && c.PermissionId == a.Id)
                       .Any()
                    );
                }
                else
                {
                    menuSelect = menuSelect.Where(a =>
                       db.Select<RolePermissionEntity>()
                       .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                       .Where(b => b.PermissionId == a.Id)
                       .Any()
                   );

                    dotSelect = dotSelect.Where(a =>
                        db.Select<RolePermissionEntity>()
                        .InnerJoin<UserRoleEntity>((b, c) => b.RoleId == c.RoleId && c.UserId == User.Id)
                        .Where(b => b.PermissionId == a.Id)
                        .Any()
                    );
                }

                menuSelect = menuSelect.AsTreeCte(up: true);
            }

            var menuList = await menuSelect
                .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
                .ToListAsync(a => new AuthUserMenuDto { ViewPath = a.View.Path });

            //用户菜单
            authGetUserInfoResponse.Menus = menuList.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList();

            //用户权限点
            authGetUserInfoResponse.Permissions = await dotSelect.ToListAsync(a => a.Code);

            return authGetUserInfoResponse;
        }
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<dynamic> LoginAsync(AuthLoginRequest request)
    {
        using (_userRepository.DataFilter.DisableAll())
        {
            var sw = new Stopwatch();
            sw.Start();

            #region 验证码校验

            if (_appConfig.VarifyCode.Enable)
            {
                if(request.CaptchaId.IsNull() || request.CaptchaData.IsNull())
                {
                    throw Response.Exception("请完成安全验证");
                }
                var validateResult = _captcha.Validate(request.CaptchaId, JsonConvert.DeserializeObject<SmsSendCodeRequest.Models.SlideTrack>(request.CaptchaData));
                if (validateResult.Result != ValidateResultType.Success)
                {
                    throw Response.Exception($"安全{validateResult.Message}，请重新登录");
                }
            }

            #endregion

            #region 密码解密

            if (request.PasswordKey.NotNull())
            {
                var passwordEncryptKey = CacheKeys.PassWordEncrypt + request.PasswordKey;
                var existsPasswordKey = await Cache.ExistsAsync(passwordEncryptKey);
                if (existsPasswordKey)
                {
                    var secretKey = await Cache.GetAsync(passwordEncryptKey);
                    if (secretKey.IsNull())
                    {
                        throw Response.Exception("解密失败");
                    }
                    request.Password = DesEncrypt.Decrypt(request.Password, secretKey);
                    await Cache.DelAsync(passwordEncryptKey);
                }
                else
                {
                    throw Response.Exception("解密失败");
                }
            }

            #endregion

            #region 登录
            var user = await _userRepository.Select.Where(a => a.UserName == request.UserName).ToOneAsync();
            var valid = user?.Id > 0;
            if(valid)
            {
                if (user.PasswordEncryptType == PasswordEncryptType.PasswordHasher)
                {
                    var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
                    valid = passwordVerificationResult == PasswordVerificationResult.Success || passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded;
                }
                else
                {
                    var password = MD5Encrypt.Encrypt32(request.Password);
                    valid = user.Password == password;
                }
            }
            
            if(!valid)
            {
                throw Response.Exception("用户名或密码错误");
            }

            if (!user.Enabled)
            {
                throw Response.Exception("账号已停用，禁止登录");
            }
            #endregion

            #region 获得token
            var authGetTokenRequest = Mapper.Map<AuthGetTokenRequest>(user);
            if (_appConfig.Tenant)
            {
                var tenant = await _tenantRepository.Select.WhereDynamic(user.TenantId).ToOneAsync<AuthGetTokenRequest.Models.TenantModel>();
                if (!(tenant != null && tenant.Enabled))
                {
                    throw Response.Exception("企业已停用，禁止登录");
                }
                authGetTokenRequest.Tenant = tenant;
            }
            string token = GetToken(authGetTokenRequest);
            #endregion

            sw.Stop();

            #region 添加登录日志

            var loginLogAddRequest = new LoginLogAddRequest
            {
                TenantId = authGetTokenRequest.TenantId,
                Name = authGetTokenRequest.Name,
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Status = true,
                CreatedUserId = authGetTokenRequest.Id,
                CreatedUserName = user.UserName,
            };

            await LazyGetRequiredService<ILoginLogService>().AddAsync(loginLogAddRequest);

            #endregion 添加登录日志

            return new { token };
        }
    }

    /// <summary>
    /// 手机号登录
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [NoOprationLog]
    public async Task<dynamic> MobileLoginAsync(AuthMobileLoginRequest request)
    {
        using (_userRepository.DataFilter.DisableAll())
        {
            var sw = new Stopwatch();
            sw.Start();

            #region 短信验证码验证
            if(request.CodeId.IsNull() || request.Code.IsNull())
            {
                throw Response.Exception("验证码错误");
            }
            var codeKey = CacheKeys.GetSmsCodeKey(request.Mobile, request.CodeId);
            var code = await Cache.GetAsync(codeKey);
            if (code.IsNull())
            {
                throw Response.Exception("验证码错误");
            }
            await Cache.DelAsync(codeKey);
            if (code != request.Code)
            {
                throw Response.Exception("验证码错误");
            }

            #endregion

            #region 登录
            var user = await _userRepository.Select.Where(a => a.Mobile == request.Mobile).ToOneAsync();
            if (!(user?.Id > 0))
            {
                throw Response.Exception("账号不存在");
            }

            if (!user.Enabled)
            {
                throw Response.Exception("账号已停用，禁止登录");
            }
            #endregion

            #region 获得token
            var authGetTokenRequest = Mapper.Map<AuthGetTokenRequest>(user);
            if (_appConfig.Tenant)
            {
                var tenant = await _tenantRepository.Select.WhereDynamic(user.TenantId).ToOneAsync<AuthGetTokenRequest.Models.TenantModel>();
                if (!(tenant != null && tenant.Enabled))
                {
                    throw Response.Exception("企业已停用，禁止登录");
                }
                authGetTokenRequest.Tenant = tenant;
            }
            string token = GetToken(authGetTokenRequest);
            #endregion

            sw.Stop();

            #region 添加登录日志

            var loginLogAddRequest = new LoginLogAddRequest
            {
                TenantId = authGetTokenRequest.TenantId,
                Name = authGetTokenRequest.Name,
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Status = true,
                CreatedUserId = authGetTokenRequest.Id,
                CreatedUserName = user.UserName,
            };

            await LazyGetRequiredService<ILoginLogService>().AddAsync(loginLogAddRequest);

            #endregion 添加登录日志

            return new { token };
        }
    }

    /// <summary>
    /// 刷新Token
    /// 以旧换新
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<dynamic> Refresh([BindRequired] string token)
    {
        var jwtSecurityToken = LazyGetRequiredService<IUserToken>().Decode(token);
        var userClaims = jwtSecurityToken?.Claims?.ToArray();
        if (userClaims == null || userClaims.Length == 0)
        {
            throw Response.Exception("无法解析token");
        }

        var refreshExpires = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.RefreshExpires)?.Value;
        if (refreshExpires.IsNull() || refreshExpires.ToLong() <= DateTime.Now.ToTimestamp())
        {
            throw Response.Exception("登录信息已过期");
        }

        var userId = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.UserId)?.Value;
        if (userId.IsNull())
        {
            throw Response.Exception("登录信息已失效");
        }

        //验签
        var securityKey = _jwtConfig.IssuerSigningKey;
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)), SecurityAlgorithms.HmacSha256);
        var input = jwtSecurityToken.RawHeader + "." + jwtSecurityToken.RawPayload;
        if (jwtSecurityToken.RawSignature != JwtTokenUtilities.CreateEncodedSignature(input, signingCredentials))
        {
            throw Response.Exception("验签失败");
        }

        var authGetTokenRequest = await _userRepository.Select.DisableGlobalFilter(FilterNames.Tenant)
            .WhereDynamic(userId.ToLong()).ToOneAsync<AuthGetTokenRequest>();

        if (_appConfig.Tenant && authGetTokenRequest?.TenantId.Value > 0)
        {
            var tenant = await _tenantRepository.Select.DisableGlobalFilter(FilterNames.Tenant)
                .WhereDynamic(authGetTokenRequest.TenantId).ToOneAsync<AuthGetTokenRequest.Models.TenantModel>();
            authGetTokenRequest.Tenant = tenant;
        }
        if (!(authGetTokenRequest?.Id > 0))
        {
            throw Response.Exception("账号不存在");
        }
        if (!authGetTokenRequest.Enabled)
        {
            throw Response.Exception("账号已停用，禁止登录");
        }

        if (_appConfig.Tenant)
        {
            if (!(authGetTokenRequest.Tenant != null && authGetTokenRequest.Tenant.Enabled))
            {
                throw Response.Exception("企业已停用，禁止登录");
            }
        }

        string newToken = GetToken(authGetTokenRequest);
        return new { token = newToken };
    }

    /// <summary>
    /// 是否开启验证码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [NoOprationLog]
    public bool IsCaptcha()
    {
        return _appConfig.VarifyCode.Enable;
    }
}