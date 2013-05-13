using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Windows.Media.Converters;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using Prism.Entities;
using Prism.Entities.Interfaces;
using Prism.Module1.Messages;
using Prism.Module1.Views;
using Prism.Module1.Service;
using Prism.Module1.ViewModels;
using Prism.Infrastucture;
using Prism.ServiceDataAccess;

namespace Prism.Module1
{
    /// <summary>
    /// Klasa modułu Prism.Module1. Klasa ta jest wykorzystywana przez Bootstrappera do inicjacji całego modułu.
    ///  </summary>
    public class Module1 : BaseModule
    {
        /// <summary>
        /// Konstruktor klasy modułu. Plusem wykorzystywania Unity w aplikacji jest to, że tworząc obiekty
        /// z wykorzystaniem kontenera automatycznie zostaną do konstruktora przekazane konkrente instancje
        /// obiektów implementujących interfejscy, które zostały wcześniej zarejestrowane. Dlatego bardzo
        /// istotne jest, aby właśnie tworzyć obiekty z wykorzystaniem Unity.
        /// </summary>
        /// <param name="regionManager"></param>
        /// <param name="container"></param>
        /// <param name="eventAggreagator"></param>
        public Module1(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggreagator)
            :base(regionManager, container, eventAggreagator)
        {}

        /// <summary>
        /// Metoda inicjuje moduł (główne jest odpowiedzialna za załodowanie pierwszego widoku.
        /// </summary>
        public override void Initialize()
        {
            //bardzo istotne jest uruchomienie metody Initialize klasy bazowej, która jest odpowiedzialna
            //za wywołwanie metod abstracyjnych klasy bazowej
            base.Initialize();

            //wyświetlenie startowego widoku modułu.
            //Aplikacja wykorzystuje event aggregatora do nawigacji między modułami
            //Poniższa wiadomość mówi, że należy wyświetlić widok z listą użytkowników
            EventAggregator.GetEvent<ShowUserListViewEvent>().Publish(new ShowUserListViewMessage());
        }

        /// <summary>
        /// Metoda jest odpowpiedzialna za inicjację kontenera Unity dla modułu.
        /// W niej rejestrujemy konkretne typy dla poszczególnych interfejsów.
        /// Zależności w aplikacji są budowane z wykorzystaniem interfejsów oraz Unity
        /// rozwiązuje już konkretne zależności.
        /// </summary>
        protected override void ConfigureContainer()
        {
            Container.RegisterType<IUserService, UserService>();
            Container.RegisterType<IUsersListView, UsersListView>();
            Container.RegisterType<IUsersListViewModel, UsersListViewModel>();
            Container.RegisterType<IManageUserView, ManageUserView>();
            Container.RegisterType<IManageUserViewModel, ManageUserViewModel>();
            Container.RegisterType<IUsersRepository, UsersRepository>();
        }

        /// <summary>
        /// Metoda inicjuje event aggregatora główne potrzez subsrypcję na zdarzenia globalne dla modułu.
        /// W tym momencie event aggregator wykorzystywany jest główne do wykoanania nawigacji
        /// między poszczególnymi widokami. Dzięki czasu widoku nie zależą od siebie i jedynie ważne
        /// aby wysywałny odpowiednie wiadomości, których logikę właśnie obsługujemy tutaj.
        /// Przyjełem założenie, że dla podstawowych widoków wyświetlanych w shellu jest wykorzystywana
        /// strategia, w której istnieje jedna instacja danego widoku, przez co nawigując wracać będziemy do tego
        /// samego widoku.
        /// Kolejnym wykorzystanym założeniem jest to, że pracujemy z viewmodelami (gdzie viewmodel posiada
        /// referencję do swojego widoku).
        /// Subskrybując się na zdarzenia istotne jest, aby robić to jak jest to poniżej. Istotny jest drugi i 
        /// trzeci parametr metody Subscribe. ThreadOption.UIThread spowoduje, że metoda obsługi zdarzenie
        /// zostanie wykonana na wątku interfejsu użytkownika, przez co w metodzie nie będziemy musieli korzystać z 
        /// Dispatchera.
        /// Trzeci parametr jest bardzo istotny. Określa on, czy event aggregator ma utworzyć silną referencję 
        /// między zdarzeniem, a metodą obsługi zdarzenia. True oznacza, że zostanie utworzona silna referencja.
        /// Brak silnej referencji może spowodować, że garbage collector w pewnym momencie może usunać instacje
        /// klasy Module1, a przez co może się okazać, że zdarzenia nie została poprawnie obsługiwane w aplikacji
        /// i nawigacje między widokami przestanie działać.
        /// W normalnym wykorzystywaniu event aggregatora zależy nam na tworznieu słabych relacji. Przez co w 
        /// momencie, gdy nie będzie nam potrzebny obiekt, event aggregator nie będzie trzebam do niego referencji
        /// i zostanie ten obiekt poprawnie usunięty z pamięci.
        /// </summary>
        protected override void ConfigureEventAggregator()
        {
            EventAggregator.GetEvent<ShowUserListViewEvent>().Subscribe(ProcessShowUserListViewMessage, ThreadOption.UIThread, true);
            EventAggregator.GetEvent<ShowManageUserEvent>().Subscribe(ProcessShowManageUserMessage, ThreadOption.UIThread, true);
            EventAggregator.GetEvent<SavedUserEvent>().Subscribe(ProcessSavedUserEvent, ThreadOption.UIThread, true);
        }

        /// <summary>
        /// Metoda obsługa zdarzenia zapisania danych użytkownika. Po zapisanu ma zostać wyświetlony
        /// widok listy użytkowników, co komunikujemy poprzez wysłanie odpowiedniej wiadomości.
        /// </summary>
        /// <param name="obj"></param>
        private void ProcessSavedUserEvent(SavedUserMessage obj)
        {
            EventAggregator.GetEvent<ShowUserListViewEvent>().Publish(new ShowUserListViewMessage());
        }

        /// <summary>
        /// Metoda obsługuje zdarzenie wyświetlenia widoku edycji danych użytkownika.
        /// Wiadomość ShowManageUserViewMessage przekazuje instacje użytkownika, którego dane będą edytowane.
        /// </summary>
        private IManageUserViewModel _manageUserViewModel;
        private void ProcessShowManageUserMessage(ShowManageUserViewMessage message)
        {
            if (_manageUserViewModel == null)
            {
                _manageUserViewModel = Container.Resolve<IManageUserViewModel>();
                MainRegion.Add(_manageUserViewModel.View);
            }

            _manageUserViewModel.User = message.User;
            MainRegion.Activate(_manageUserViewModel.View);
        }

        /// <summary>
        /// Metoda obsługi zdarzenia wyświetlenia widoku listy użytkowników.
        /// Widomość ShowUserListViewMessage przekazuje informację czy podczas wyświetlania widoku
        /// dane mają zostać przeładowane.
        /// </summary>
        private IUsersListViewModel _usersListViewModel;
        private void ProcessShowUserListViewMessage(ShowUserListViewMessage message)
        {
            if (_usersListViewModel == null)
            {
                _usersListViewModel = Container.Resolve<IUsersListViewModel>();
                MainRegion.Add(_usersListViewModel.View);
            }

            if (message.Reload)
            {
                _usersListViewModel.LoadAsync();
            }
            MainRegion.Activate(_usersListViewModel.View);
        }
    }
}
