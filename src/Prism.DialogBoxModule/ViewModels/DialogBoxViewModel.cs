using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.DialogBoxModule.Views;
using Prism.Infrastucture;

namespace Prism.DialogBoxModule.ViewModels
{
    public class DialogBoxViewModel : BaseViewModel, IDialogBoxViewModel
    {
        public DialogBoxViewModel(IDialogBoxView view)
            : base(view)
        {

        }

        private IView _viewToDisplay;
        public IView ViewToDisplay
        {
            get { return _viewToDisplay; }
            set
            {
                if (_viewToDisplay != value)
                {
                    _viewToDisplay = value;
                    OnPropertyChanged(() => this.ViewToDisplay);
                }
            }
        }
    }
}
