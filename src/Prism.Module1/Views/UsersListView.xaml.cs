using Prism.Infrastucture;
using Prism.Module1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Prism.Module1.Views
{
    /// <summary>
    /// Interaction logic for UsersList.xaml
    /// </summary>
    public partial class UsersListView : UserControl, IUsersListView
    {
        public UsersListView()
        {
            InitializeComponent();
        }

        public Infrastucture.IViewModel ViewModel
        {
            get
            {
                return (IUsersListViewModel)DataContext;
            }
            set
            {
                DataContext = value;
            }
        }
    }
}
