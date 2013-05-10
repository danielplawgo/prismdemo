using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.Infrastucture
{
    public class NavigationParameters
    {
        private static Dictionary<string, object> _values;

        static NavigationParameters()
        {
            _values = new Dictionary<string, object>();
        }

        public static string Add<T>(T value)
        {
            string key = Guid.NewGuid().ToString();
            _values.Add(key, value);
            return key;
        }

        public static T Get<T>(string key)
        {
            if (_values.ContainsKey(key))
            {
                if (_values[key] is T)
                {
                    T value = (T)_values[key];
                    _values.Remove(key);                       
                    return value;
                }
                return default(T);
            }
            return default(T);
        }
    }
}
