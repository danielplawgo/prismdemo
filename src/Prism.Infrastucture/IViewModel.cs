using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.Prism.Regions;

namespace Prism.Infrastucture
{
    /// <summary>
    /// Interfejs dla viewmodeli
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Każdy viewmodel zawiera referencje do widoku z nim związanego.
        /// </summary>
        IView View { get; set; }

        /// <summary>
        /// Zdarzenie informujące o zamknięciu danego widoku
        /// </summary>
        event EventHandler<EventArgs> Closed;
    }
}
