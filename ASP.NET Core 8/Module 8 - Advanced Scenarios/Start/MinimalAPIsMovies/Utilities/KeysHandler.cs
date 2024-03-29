using Microsoft.IdentityModel.Tokens;

namespace MinimalAPIsMovies.Utilities
{
    public class KeysHandler
    {
        public const string OurIssuer = "our-app";
        private const string KeysSection = "Authentication:Schemes:Bearer:SigningKeys";
        private const string KeysSection_Issuer = "Issuer";
        private const string KeysSection_Value = "Value";

        public static IEnumerable<SecurityKey> GetKey(IConfiguration configuration)
            => GetKey(configuration, OurIssuer);

        public static IEnumerable<SecurityKey> GetKey(IConfiguration configuration,
            string issuer)
        {
            var signingKey = configuration.GetSection(KeysSection)
                .GetChildren()
                .SingleOrDefault(key => key[KeysSection_Issuer] == issuer);

            if (signingKey is not null && signingKey[KeysSection_Value] is string secretKey)
            {
                yield return new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
            }
        }

        public static IEnumerable<SecurityKey> GetAllKeys(IConfiguration configuration)
        {
            var signingKeys = configuration.GetSection(KeysSection)
                .GetChildren();

            foreach (var signingKey in signingKeys)
            {
                if (signingKey[KeysSection_Value] is string secretKey)
                {
                    yield return new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
                }
            }
        }
    }
}
