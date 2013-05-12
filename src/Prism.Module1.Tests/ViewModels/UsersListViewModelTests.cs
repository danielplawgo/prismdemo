using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Prism.Module1.Messages;
using Prism.Module1.Service;
using FakeItEasy;
using Prism.Infrastucture;
using Prism.Module1.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Prism.Module1.Views;
using Prism.Entities.Users;


namespace Prism.Module1.Tests.ViewModels
{
    /// <summary>
    /// Klasa testów jednostkowych dla UsersistViewModel. Do testów jest wykorzystane bibliotek
    /// NUnit, natomiast do mocków FakeItEasy.
    /// 
    /// NUnit wykorzystuje atrybut TestFixture do oznazcenia klasy jako klasy zawierającej testy 
    /// jednostkowe NUnita.
    /// 
    /// Metody wykorzystywane przez NUnita są okreslane przez odpowiednie atrybuty (część znajduje się
    /// poniżej w teście). Inne popularne atrybuty to:
    /// *TearDown - metoda jest odpalana po każdym teście.
    /// *TestFixtureSetUp - metoda jest odpalana przed wyszstkimi testami (tylko raz dla danej klasy z testami).
    /// *TestFixtureTearDown - metoda jest odpalana po wszystkich testach (tylko raz dla danej klasy z testami).
    /// 
    /// Każda metoda wykorzystywana przez NUnita musi być publiczna, zwracać typ void oraz nie przyjmować
    /// rzadnego parametru.
    /// </summary>
    [TestFixture]
    public class UsersListViewModelTests
    {
        IUserService _userService;
        IEventAggregator _eventAggregator;
        IUsersListView _view;
        UsersListViewModel _viewModel;
        
        /// <summary>
        /// Metoda oznaczona atrybutej SetUp jest wykonywane przez NUnita przed każdym testem.
        /// Metoda taka przydaje się, aby wykonywać w niej kod, który powiela się w testach.
        /// Tak jak tutaj powiela się Tworzenie mocków oraz instacji viewmodelu do testów.
        /// </summary>
        [SetUp]
        public void InitTest()
        {
            _userService = A.Fake<IUserService>();
            _eventAggregator = A.Fake<IEventAggregator>();
            _view = A.Fake<IUsersListView>();
            _viewModel = new UsersListViewModel(_userService, _eventAggregator, _view);
        }

        /// <summary>
        /// Metoda oznaczona atrybutem Test jest testem dla NUnita.
        /// Atrybut ExpectedException określa, że w teście oczekujemy, że zostanie wyrzucony 
        /// wyjątek danego typu. Gdy nie będzie wyjątku to test nie przejdzie.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckUserServiceNull()
        {
            UsersListViewModel viewModel = new UsersListViewModel(null, _eventAggregator, _view);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckEventAggregatorNull()
        {
            UsersListViewModel viewModel = new UsersListViewModel(_userService, null, _view);
        }

        /// <summary>
        /// Najczęściej pisane testy są zbudowane z trzech sekcji.
        /// Pierwsza sekcja przygotowuje obiekty do testu (np. konfiguruje mocki).
        /// Druga sekcja wykonuje testową metodę.
        /// Trzecie sekcja sprawdza warunki testu. np. czy wartości są obliczone poprawnie
        /// lub czy na mockach zostały wykonane odpowiednie metody.
        /// </summary>
        [Test]
        public void LoadCorrectData()
        {
            //Pierwsza sekscja testu
            //uczymy mocka, że jak zostanie wywołana metoda GetUsers
            //aby aby zwrócić instacje Listy<User> zawierającą jednego użytkownika.
            A.CallTo(() => _userService.GetUsers())
                .Returns((new List<User>() { new User { Name = "Daniel" } }));

            //Druga sekcja testu
            //Wykonujemy testowaną metodę
            _viewModel.Load();

            //Trzecia sekcja testu - spradzamy, czy wszystko przebiegło poprawnie.
            //Sprawdzamy, czy została wywołąna metoda GetUsers na UserService
            //FakeItEasy umożliwia też sprawdzanie, czy np. metoda nie zostały wykonana
            //wykonana określoną liczę razy. FakeItEasy jest na tyle fajne,
            //że rozróżnia również przekazywane parametry do mokowanej metody.
            //Czyli możemy sprawdzić, czy metoda z parametrem "Daniel" zostałą wykoanana raz
            //oraz czy metoda z parametrem "Tomek" nie zostałą wykonana.
            A.CallTo(() => _userService.GetUsers()).MustHaveHappened();
            //sprawdzamy ile jest wyświetlanych użytkowników. Skoro zamokowaliśmy
            //metoda zwracającą listę użytkowników to wiemy, że powinneń być tylko jeden.
            Assert.AreEqual(1, _viewModel.Users.Count);
            //Znając listę użytkowników sprawdzamy, czy pierszy użytkownika ma nazwę "Daniel".
            Assert.AreEqual("Daniel", _viewModel.Users[0].Name);
        }
        
        [Test]
        public void CheckSubscribeForAddedUserMessage()
        {
            SavedUserEvent addedUserEvent = A.Fake<SavedUserEvent>();
            A.CallTo(() => _eventAggregator.GetEvent<SavedUserEvent>())
                .Returns(addedUserEvent);

            //FakeItEasy odróżnia wywołania metod z różnymi parametrami.
            //Gdy tego nie potrzebujemy to możemy to przekażać z konstrukcji:
            //A<typ parametru>.Ignored
            A.CallTo(() => addedUserEvent.Subscribe(A<Action<SavedUserMessage>>.Ignored, A<ThreadOption>.Ignored, A<bool>.Ignored, A<Predicate<SavedUserMessage>>.Ignored));

            UsersListViewModel viewModel = new UsersListViewModel(_userService, _eventAggregator, _view);

            A.CallTo(() => _eventAggregator.GetEvent<SavedUserEvent>())
                .MustHaveHappened();
            A.CallTo(() => addedUserEvent.Subscribe(A<Action<SavedUserMessage>>.Ignored, A<ThreadOption>.Ignored, A<bool>.Ignored, A<Predicate<SavedUserMessage>>.Ignored))
                .MustHaveHappened();
        }

        [Test]
        public void LoadNull()
        {
            A.CallTo(() => _userService.GetUsers())
                .Returns(null);

            _viewModel.Load();

            A.CallTo(() => _userService.GetUsers()).MustHaveHappened();
            Assert.AreEqual(0, _viewModel.Users.Count);
        }
    }
}
