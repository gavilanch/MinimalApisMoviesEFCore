using Microsoft.IdentityModel.Tokens;

namespace MinimalAPIsMovies.Utilities
{
    public static class HttpContextExtensionsUtilities
    {
        public static T ExtractValueOrDefault<T>(this HttpContext context, string field,
            T defaultValue) where T: IParsable<T>
        {
            var value = context.Request.Query[field];

            if (!value.Any())
            {
                return defaultValue;
            }

            return T.Parse(value!, null);
        }
    }
}
