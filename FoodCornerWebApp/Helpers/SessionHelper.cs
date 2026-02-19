using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FoodCornerWebApp.Helpers
{
    public static class SessionHelper
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (string.IsNullOrEmpty(value))
                return default;

            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
