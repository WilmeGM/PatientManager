using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PatientManager.Core.Application.Helpers
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key) => session.GetString(key) == null ? default : JsonConvert.DeserializeObject<T>(session.GetString(key));
    }
}
