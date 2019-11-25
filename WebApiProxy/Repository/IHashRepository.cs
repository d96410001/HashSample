using System;

namespace WebApiProxy.Repository
{
    public interface IHashRepository
    {
        string GetOrCreateValue(string key);
        string GetValue(string key);
        bool SetValue(string key, string value, TimeSpan? expiry = null);
    }
}