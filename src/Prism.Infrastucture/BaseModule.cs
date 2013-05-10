using Microsoft.Practices.Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.Infrastucture
{
    public abstract class BaseModule : IModule
    {
        public abstract void Initialize();
    }
}
