using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.Infrastucture
{
    /// <summary>
    /// Inferfejs dla widoków.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Każdy widok zawiera referencje do viewmodelu z nim związanego
        /// </summary>
        IViewModel ViewModel { get; set; }
    }
}
