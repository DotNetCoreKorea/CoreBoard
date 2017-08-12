using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CoreBoard.Extensions
{
    public static class SessionExtension
    {
        public static void SetInt64(this ISession _this, string key, long data) => _this.Set(key, BitConverter.GetBytes(data));
        public static long? GetInt64(this ISession _this, string key)
        {
            var val = _this.Get(key);
            if (val == null)
                return null;

            return BitConverter.ToInt64(_this.Get(key), 0);
        } 
    }
}
