using System;
using System.Reflection;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace FizzBuzzApplication.Attributes
{
    public class CacheAttribute : Attribute
    {
        public int TTL { get; set; }
        public string Key { get; set; }
        
        public string OnBefore(MethodInfo targetMethod, object[] args, IDistributedCache distributedCache)
        {
            return  distributedCache.GetString(Key);
        }

        public void OnAfter(MethodInfo targetMethod, object[] args, object value, IDistributedCache distributedCache)
        {
            distributedCache.SetString(Key, JsonConvert.SerializeObject(value));
        }    
    }
    
    public interface ICacheAttribute
    {
        string OnBefore(MethodInfo targetMethod, object[] args, IDistributedCache distributedCache);
        void  OnAfter(MethodInfo targetMethod,  object[] args, object value,  IDistributedCache distributedCache);
    }
}