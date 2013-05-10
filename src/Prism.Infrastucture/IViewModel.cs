using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.Prism.Regions;

namespace Prism.Infrastucture
{
    public interface IViewModel : INotifyPropertyChanged
    {
        IView View { get; set; }
    }
}
