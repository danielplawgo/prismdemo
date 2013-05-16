using Microsoft.Practices.Prism.Events;
using Prism.Entities.Users;
using Prism.Module1.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using Prism.Infrastucture;
using Prism.Module1.Views;
using Microsoft.Practices.Prism.Regions;
using Prism.Module1.Messages;

namespace Prism.Module1.ViewModels
{
    /// <summary>
    /// Klasa viewmodelu odpowiedzalna za logikę widoku edycji danych użytkownika.
    /// </summary>
    public class ManageUserViewModel : BaseViewModel, IManageUserViewModel
    {
        private IUserService _userService;
        private IEventAggregator _eventAggregator;

        public ManageUserViewModel(IUserService userService, IEventAggregator eventAggregator, IManageUserView view)
            : base(view)
        {
            _userService = userService;
            _eventAggregator = eventAggregator;

            User = new User();
        }

        /// <summary>
        /// Właściwość reprezentując użytkownika do edycji.
        /// </summary>
        private User _user;
        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                if (_user != value)//logikę settera wykonujemy tylko w momencje, gdy zmienił się użytkownik
                {
                    if (_user != null)
                    {
                        //przy zmianie uzytkownika do edycji musimy na potrzednim użytkowniku
                        //odrejestrować się ze zdarzenia PropertyChanged
                        _user.PropertyChanged -= UserPropertyChanged;
                    }

                    _user = value;
                    if (_user == null)//Null określa, że będziemy dodawać nowego użytkownika.
                    {
                        _user = new User();
                        IsEditMode = false;
                    }
                    else
                    {
                        IsEditMode = true;
                    }
                  //Pod zdarzenie PropertyChanged użytkownika podpisanymy się dlatego, aby obsłużyć zmianę widoku
                    //w momencie walidacji. W metodzie UserPropertyChanged sprawdzamy, czy użytkownik się waliduje
                    //oraz na podstawie wyniku walidacji aktualizuje widok.
                    _user.PropertyChanged += UserPropertyChanged;
                    OnPropertyChanged(() => this.User);
                }
            }
        }

        void UserPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Komenda SaveUserCommand w metodzie CanExecute sprawdza, czy użytkownik się waliduje.
            //Metoda CanExecute domyślne uruchamiana jest tylko przy tworzeniu bindingu.
            //Dlatego przy właściwości użytkownika musimy za pomocą metody RaiseCanExecuteChanged
            //wymówić jej ponowne wykonanie. Brak tego spowoduje, że interfejs użytkownika nie
            //będzie się aktualizować przy zmiane danych użytkownika.
            SaveUserCommand.RaiseCanExecuteChanged();

        }

        /// <summary>
        /// Właściwość określa, czy widok jest do edycji użytkownia, czy do dodania nowego użytkownika.
        /// </summary>
        private bool _isEditMode;
        public bool IsEditMode
        {
            get
            {
                return _isEditMode;
            }
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged(() => this.IsEditMode);
                }
            }
        }

        /// <summary>
        /// Komenda służąca do zapisania danych użytkownika. Pierwszy parametr konstruktora obektu DelegateCommand
        /// określa metodę wykonaną w momencie nacisknięcia na przycisk. Osobiście lubie wykorystawać do
        /// tego lamba expersion, bo jest wtedy czytelniej i od razu widzę, co ma się wykonać (w wiekszości
        /// przypadku logikę będziemy oddelegowywać do jakieś usługi, któa wykona wszystko za nas).
        /// Komenda zapisuje dane użytkownika z wykorzystaniem usługi UserService oraz wysyła odpowiednią 
        /// wiadomość.
        /// Drugi parametr konstruktora określa,  czy dana komenda może być wykonana, czy nie. Przekazywana
        /// metoda musi zwrócić typ bool. True określa, że komenta może być wykoanana, false, że nie.
        /// Przy bindingu do Buttona w praktyce metoda okresla to, czy Button jest aktywny, czy nie.
        /// Metoda przekazywana jako drugi parametr jest wykonywana domyślne tylko raz przy tworzeniu bindingu.
        /// Można wymusić jej ponowne wykonanie. Służy do tego metoda RaiseCanExecuteChanged obiektu 
        /// DelegataCommand.
        /// </summary>
        private DelegateCommand _saveUserCommand;
        public DelegateCommand SaveUserCommand
        {
            get
            {
                if (_saveUserCommand == null)
                {
                    _saveUserCommand = new DelegateCommand(
                    () =>
                    {
                        _userService.Save(User);
                        _eventAggregator
                                .GetEvent<SavedUserEvent>()
                                .Publish(new SavedUserMessage()
                                {
                                    User = User,
                                    IsNewUser = !IsEditMode
                                });
                        Close();
                    },
                    () => User != null && User.IsValid);

                }
                return _saveUserCommand;
            }
        }

        /// <summary>
        /// Metoda powoduje zamknięcie danego widoku bez zapisuje danych z wykorzystaniem UserService.
        /// </summary>
        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new DelegateCommand(
                     () =>
                     {
                         _eventAggregator
                             .GetEvent<ShowUserListViewEvent>()
                             .Publish(new ShowUserListViewMessage());
                         Close();
                     }
                 );
                }
                return _cancelCommand;
            }
        }
    }
}
