using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Infrastucture;

namespace Prism.DialogBoxModule.ViewModels
{
    public interface IDialogBoxViewModel : IViewModel
    {
        IView ViewToDisplay { get; set; }
    }
}
