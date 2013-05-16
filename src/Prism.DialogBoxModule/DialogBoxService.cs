using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.DialogBoxModule.ViewModels;
using Prism.Infrastucture;

namespace Prism.DialogBoxModule
{
    /// <summary>
    /// usługa odpowiedzialna za wyświetlanie widoków w oknie dialogowym
    /// </summary>
    public class DialogBoxService : IDialogBoxService
    {
        private IUnityContainer _container;

        public DialogBoxService(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Metoda wyświetla widok z viewmodelu w oknie dialogowym
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public bool? ShowDialog(IViewModel viewModel)
        {
            var dialogBoxViewModel = _container.Resolve<IDialogBoxViewModel>();
            dialogBoxViewModel.ViewToDisplay = viewModel.View;

            Window dialogBoxView = dialogBoxViewModel.View as Window;
            dialogBoxView.Owner = Application.Current.MainWindow;
            
            if (dialogBoxView == null)
            {
                return null;
            }

            //podpinamy się pod zdarzenie Closed, aby zamknąć okno dialogowe,
            //w ten sposób viewmodel informuje nas, że okno trzeba zamknąć
            viewModel.Closed += (sender, args) => dialogBoxView.Close();

            return dialogBoxView.ShowDialog();
        }
    }
}
