using System;
using System.Text;
 
namespace FinmoreNetflity.Utils
{
    public static class AuthHelper
    {
        public static string GetBasicAuthHeader(string username, string password)
        {
            var credentials = $"{username}:{password}";
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
            return $"Basic {encoded}";
        }
    }
}