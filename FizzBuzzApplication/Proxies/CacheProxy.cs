using System;
using System.Linq;
using System.Reflection;
using FizzBuzzApplication.Attributes;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FizzBuzzApplication.Proxies
{
    public class CacheProxy<TDecorated> : DispatchProxy
    {
        private TDecorated _decorated;
        private IDistributedCache _distributedCache;
        
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var aspect = targetMethod.GetCustomAttributes(typeof(ICacheAttribute), true).FirstOrDefault();
           
            if (aspect == null)
            {
                return targetMethod.Invoke(_decorated, args);
            }
            
            var beforeResponse =  ((ICacheAttribute) aspect)?.OnBefore(targetMethod, args, _distributedCache);

            object result;
            if (string.IsNullOrEmpty(beforeResponse))
            {
                result = targetMethod.Invoke(_decorated, args);
                (aspect as ICacheAttribute)?.OnAfter(targetMethod, args, result, _distributedCache);
            }
            else
            {
                result = JsonConvert.DeserializeObject<object>(beforeResponse);
            }
            
            return result;  
        }
        
        public static TDecorated Create(TDecorated decorated, IDistributedCache distributedCache)
        {
            object proxy = Create<TDecorated, CacheProxy<TDecorated>>();
            ((CacheProxy<TDecorated>)proxy).SetParameters(decorated, distributedCache);
            return (TDecorated)proxy;
        }

        private void SetParameters(TDecorated decorated,IDistributedCache distributedCache)
        {
            _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }
    }
}