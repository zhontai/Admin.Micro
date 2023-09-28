using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Api.Core.Configs;
using ZhonTai.Utils.Extensions;

namespace ZhonTai.Api.Core.Auth;

[SingleInstance]
public class UserToken : IUserToken
{
    private readonly JwtConfig _jwtConfig;

    public UserToken(IOptions<JwtConfig> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
    }

    public string Create(Claim[] claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.IssuerSigningKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var timestamp = DateTime.Now.AddMinutes(_jwtConfig.Expires + _jwtConfig.RefreshExpires).ToTimestamp().ToString();
        claims = claims.Append(new Claim(ClaimAttributes.RefreshExpires, timestamp)).ToArray();

        var token = new JwtSecurityToken(
            issuer: _jwtConfig.ValidIssuer,
            audience: _jwtConfig.ValidAudience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(_jwtConfig.Expires),
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public JwtSecurityToken Decode(string jwtToken)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jwtToken);
        return jwtSecurityToken;
    }
}