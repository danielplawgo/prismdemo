using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Prism.Entities.Users;
using Prism.Module1.Service;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Prism.Infrastucture;
using Prism.Module1.Views;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism;
using Prism.Module1.Messages;


namespace Prism.Module1.ViewModels
{
    /// <summary>
    /// Klasa viewModelu odpowiedzialnego za widok listy u€żytkowników.
    /// </summary>
    public class UsersListViewModel : BaseViewModel, IUsersListViewModel
    {
        private IUserService _userService;
        private IEventAggregator _eventAggregator;
        private IUnityContainer _container;

        public UsersListViewModel(IUserService userService, IEventAggregator eventAggregator, IUsersListView view,
            IUnityContainer container)
            : base(view)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            _userService = userService;

            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;

            Title = "Users List";

            //rejestrujemy się na zdarzenie zapisania danych użytkownika, aby dodać nowego użytkownika
            //do listy uzytkowników
            _eventAggregator.GetEvent<SavedUserEvent>().Subscribe(ProcessSavedUserMessage);
        }

        /// <summary>
        /// Metoda obsługi zdarzenia SaveUserEvent. W sytuacji, gdy został dodane nowy użytkownik, zosatnie on 
        /// dodany do listy użytkowników.
        /// </summary>
        /// <param name="message"></param>
        public void ProcessSavedUserMessage(SavedUserMessage message)
        {
            if (message.IsNewUser)
            {
                Users.Add(message.User);
            }
        }

        /// <summary>
        /// Metoda jest odpowiedzialna za wczytanie użytkowników z wykorzystaniem UserService
        /// w sposób synchroniczny. Nie jest zalecana. Lepiej korzystać z metody LoadAsync.
        /// </summary>
        public void Load()
        {
            var data = _userService.GetUsers();
            Users = data == null ? new ObservableCollection<User>() : new ObservableCollection<User>(data);
        }

        /// <summary>
        /// Metoda jest odpowiedzialna za wczytanie użytkowników z wykorzystaniem UserService
        /// w sposób asynchroniczny. Jest zalecanym sposobem ładowania danych w przeciwieństwie do 
        /// metody Load. Nie blokuje interfejsu użytkownika oraz umożliwia w ładny sposób
        /// anulowanie operacji wczytywania danych.
        /// Ustawienie właściwości IsBusy na true powoduje, że w interfejsie użytkownika zostanie 
        /// wyświetlony loader. Ustawienie na flase chowa loadera.
        /// </summary>
        public async void LoadAsync()
        {
            IsBusy = true;
            var data = await _userService.GetUsersAsync();
            IsBusy = false;
            Users = data == null ? new ObservableCollection<User>() : new ObservableCollection<User>(data);
        }

        /// <summary>
        /// Właściwość określa, czy viewmodel jest zajęty ładowaniem danych czy nie.
        /// Potrzebne do BusyIndicator.
        /// </summary>
        private bool _isBusy = false;
        public  bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged(() => this.IsBusy);
                }
            }
        }

        /// <summary>
        /// Komenda jest wykorzystana do anulowania operacji ładowania danych.
        /// Użycie jej ma sens tylko w momencie wczytywania danych w sposób
        /// asynchroniczny.
        /// </summary>
        private DelegateCommand _cancelLoadDataCommand;
        public DelegateCommand CancelLoadDataCommand
        {
            get
            {
                if (_cancelLoadDataCommand == null)
                {
                    _cancelLoadDataCommand = new DelegateCommand(
                        () => _userService.CancelGetUsersData());
                }
                return _cancelLoadDataCommand;
            }
        }

        /// <summary>
        /// Kolecja użytkowników do wyświetlenia.
        /// Istotne jest aby skorzytać z klasy ObservableCollection w przeciwieństwie do klasycznej listy (List).
        /// Kolekcja ObservableCollection informuje za pomocą zdarzenia, że jej zawartość się zmieniła 
        /// (w sensie, że coś zostało dodane, usuniętę - co istotne nie informuje, czy obiekty, któe przechowuje
        /// się zmieniły). Przy wykorzystaniu bindingu wpf będzie automatycznie aktulizował widok, gdy dodany/usuniemy
        /// coś z kolekcji.
        /// </summary>
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get
            {
                if (_users == null)
                {
                    LoadAsync();
                }
                return _users;
            }
            set
            {
                if (_users != value)
                {
                    _users = value;
                    OnPropertyChanged(() => this.Users);
                }
            }
        }

        private User _selectedUser;
        public  User SelectedUser
        {
            get { return _selectedUser; } 
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged(() => this.SelectedUser);
                    EditSelectedUserInDialogBoxCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private DelegateCommand _editSelectedUserInDialogBoxCommand;
        public  DelegateCommand EditSelectedUserInDialogBoxCommand
        {
            get
            {
                if (_editSelectedUserInDialogBoxCommand == null)
                {
                   _editSelectedUserInDialogBoxCommand = new DelegateCommand(
                       () =>
                       {
                           IManageUserViewModel viewModel = _container.Resolve<IManageUserViewModel>();
                           viewModel.User = SelectedUser;
                           
                           IDialogBoxService dialogBoxService = _container.Resolve<IDialogBoxService>();
                           
                           dialogBoxService.ShowDialog(viewModel);
                       },
                       () => SelectedUser != null);
                }
                return _editSelectedUserInDialogBoxCommand;
            }
        }

        /// <summary>
        /// Komenda obsługująca przycisk odświeżania danych w liście użytkowników. Zostaje ponownie 
        /// wywołana metoda Load.
        /// </summary>
        private DelegateCommand _refreshUsersListCommand;
        public DelegateCommand RefreshUsersListCommand
        {
            get
            {
                if (_refreshUsersListCommand == null)
                {
                    _refreshUsersListCommand = new DelegateCommand(LoadAsync);
                }
                return _refreshUsersListCommand;
            }
        }

        /// <summary>
        /// Komenda obsugjąca przycisk dodanie nowego użytkownika. Zostaje wysłana wiadomość,
        /// aby wyświetlić widok edycji danych użytkownika. Nie zosatje ustawiony jaki użytkownika
        /// ma zostać edytowany, przez co zosatnie wyświetlony widok dodania nowego użytkownika.
        /// </summary>
        private DelegateCommand _addUserCommand;
        public DelegateCommand AddUserCommand
        {
            get
            {
                if (_addUserCommand == null)
                {
                    _addUserCommand = new DelegateCommand(
                    () => _eventAggregator
                        .GetEvent<ShowManageUserEvent>()
                        .Publish(new ShowManageUserViewMessage()));
                }
                return _addUserCommand;
            }
        }

        /// <summary>
        /// Komenda obsługi edycji przycisku. Tutaj została wykorzystana generuczna klasa DelegateCommand,
        /// gdzie jest wykorzystywany parametr do przekazania jakiego użytkownika należy edytować.
        /// W widoku do wyświetlenia listy użytkownikow jest wykorzystywany ListBox, dla któego został określony
        /// ItemTemplate slużony do wyświetlenia danych użytkownika. W template został osadzony przycisk,
        /// którey jest zbindowany z tą komendą. Bindując komendę można jeszcze określić parametr przekazywany do komendy
        /// W tym przypadku jest to obiekt użytkownika. Gdy korzystwamy z generycznej klasy DelegateCommand,
        /// zmienia się sygnatura metody w pierwszym mi drugim parametrze konstruktora. Obie metody muszę
        /// przyjmować w takiej sytuacji argument typu generyczne (tutaj obiekt User).
        /// Komenda wysyła wiadomość wyświetlenia widoku edycji użytkownika.
        /// </summary>
        private DelegateCommand<User> _editUserCommand;
        public DelegateCommand<User> EditUserCommand
        {
            get
            {
                if (_editUserCommand == null)
                {
                    _editUserCommand = new DelegateCommand<User>(
                        user => _eventAggregator
                            .GetEvent<ShowManageUserEvent>()
                            .Publish(new ShowManageUserViewMessage()
                            {
                                User = user
                            })
                    );
                }
                return _editUserCommand;
            }
        }
    }
}
