using IATMS.Configurations;
using IATMS.Models.Authentications;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IATMS.Components
{
    public class JwtToken
    {
        public static string GenerateToken(string username, string secret, string guid, DateTime expire)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("param1", Cryptography.encryptStrAndToBase64(username, secret, guid)),
                    new Claim(JwtRegisteredClaimNames.Jti, guid),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUniversalTime().ToString())
                }),
                Expires = expire,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
        public static AccessTokenProps AccessTokenValidation(HttpRequest request, TokenValidationParameters _tokenValidationParameters)
        {
            AccessTokenProps result = new();
            try
            {
                var token = request.Headers["Authorization"].ToString().Substring("Bearer".Length).Trim();
                string secret = AppSettings.AccessSecretKey;

                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var tokenInVerification = jwtTokenHandler.ValidateToken(token, _tokenValidationParameters, out var validedToken);

                result.guid = tokenInVerification.Claims.FirstOrDefault(x => x.Type.Equals("jti")).Value;

                result.username = Cryptography.decryptStrAndFromBase64(tokenInVerification.Claims.FirstOrDefault(x => x.Type.Equals("param1")).Value, secret, result.guid);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
        public static AccessTokenProps RefreshTokenValidation(string token, string secret)
        {
            AccessTokenProps result = new();
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var tokenInVerification = jwtTokenHandler.ValidateToken(token, new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    RequireExpirationTime = true
                }, out var validedToken);

                result.guid = tokenInVerification.Claims.FirstOrDefault(x => x.Type.Equals("jti")).Value;

                result.username = Cryptography.decryptStrAndFromBase64(tokenInVerification.Claims.FirstOrDefault(x => x.Type.Equals("param1")).Value, secret, result.guid);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}
