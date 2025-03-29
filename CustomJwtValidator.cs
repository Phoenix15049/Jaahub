using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class CustomJwtValidator : ISecurityTokenValidator
{
    private readonly TokenValidationParameters _validationParameters;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public CustomJwtValidator(TokenValidationParameters validationParameters)
    {
        _validationParameters = validationParameters;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public bool CanValidateToken => true;

    public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

    public bool CanReadToken(string token)
    {
        try
        {
            var jwtToken = _tokenHandler.ReadToken(token);
            return jwtToken != null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token Reading Error: {ex.Message}");
            return false;
        }
    }

    public ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        try
        {
            return _tokenHandler.ValidateToken(token, _validationParameters, out validatedToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token Validation Error: {ex.Message}");
            Console.WriteLine($"Full Exception: {ex}");
            throw;
        }
    }
}