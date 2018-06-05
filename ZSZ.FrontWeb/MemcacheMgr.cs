using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.FrontWeb
{
    public class MemcacheMgr
    {
        private MemcachedClient client;

        public static MemcacheMgr Instance { get; private set; } 

        static MemcacheMgr()
        {
            Instance = new MemcacheMgr();
        }

        private MemcacheMgr()
        {

            var settingService =
                DependencyResolver.Current.GetService<ISettingService>();
            string[] servers 
                =settingService.GetValue("MemCachedServers").Split(';');

            MemcachedClientConfiguration config = 
                new MemcachedClientConfiguration();
            foreach(var server in servers)
            {
                config.Servers.Add(new IPEndPoint(IPAddress.Parse(server), 11211));
            }
            
            config.Protocol = MemcachedProtocol.Binary;
            client = new MemcachedClient(config);
        }

        public void SetValue(string key,object value,TimeSpan expires)
        {
            if(!value.GetType().IsSerializable)
            {
                throw new ArgumentException("value必须是可序列化的对象");
            }
            client.Store(StoreMode.Set, key, value, expires);
        }

        public T GetValue<T>(string key)
        {
            return client.Get<T>(key);
        }
    }
}