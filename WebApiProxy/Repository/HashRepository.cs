using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApiProxy.Models;

namespace WebApiProxy.Repository
{
    public class HashRepository
    {
        private static readonly Lazy<HashRepository> lazy = new Lazy<HashRepository>(() => new HashRepository());

        public static HashRepository Instance { get { return lazy.Value; } }

        private ConnectionMultiplexer Redis { get; set; }

        private IDatabase DB { get; set; }

        private HashRepository()
        {
            this.Redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        }
        private void GetDBInstance()
        {
            if (this.DB == null)
            {
                this.DB = Redis.GetDatabase();
            }
        }

        public bool SetValue(string key, string value, TimeSpan? expiry = null)
        {
            GetDBInstance();
            return this.DB.StringSet(key, value, expiry);
        }

        public string GetValue(string key)
        {
            GetDBInstance();
            return this.DB.StringGet(key);
        }
    }
}
