using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.Unity
{
    public static class IUnityContainerExtensions
    {
        public static void RegisterTypeForNavigation<NameType, ObjectType>(this IUnityContainer container)
        {
            container.RegisterType<object, ObjectType>(typeof(NameType).FullName);
        }
    }
}
