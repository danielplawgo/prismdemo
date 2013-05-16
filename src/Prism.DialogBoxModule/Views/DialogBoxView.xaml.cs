using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Prism.Infrastucture;
using Prism.DialogBoxModule.ViewModels;

namespace Prism.DialogBoxModule.Views
{
    /// <summary>
    /// Interaction logic for DialogBoxView.xaml
    /// </summary>
    public partial class DialogBoxView : Window, IDialogBoxView
    {
        public DialogBoxView()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get { return (IDialogBoxViewModel) DataContext; }
            set { DataContext = value; }
        }
    }
}
