using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.Prism.Regions
{
    public static class INavigateAsyncExtentions
    {
        public static void RequestNavigate<T>(this INavigateAsync navigation)
        {
            navigation.RequestNavigate(typeof(T).FullName);
        }

        public static void RequestNavigate<T>(this INavigateAsync navigation, UriQuery query)
        {
            navigation.RequestNavigate(string.Format("{0}{1}", typeof(T).FullName, query.ToString()));
        }

        public static void RequestNavigate<T>(this INavigateAsync navigation, Action<NavigationResult> navigationCallback)
        {
            navigation.RequestNavigate(typeof(T).FullName, navigationCallback);
        }

        public static void RequestNavigate<T>(this INavigateAsync navigation,UriQuery query, Action<NavigationResult> navigationCallback)
        {
            navigation.RequestNavigate(string.Format("{0}{1}", typeof(T).FullName, query.ToString()), navigationCallback);
        }
    }
}
