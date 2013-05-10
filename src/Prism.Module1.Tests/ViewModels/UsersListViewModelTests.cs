using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
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
    [TestFixture]
    public class UsersListViewModelTests
    {
        IUserService _userService;
        IEventAggregator _eventAggregator;
        IRegionManager _regionManager;
        IUsersListView _view;
        UsersListViewModel _viewModel;

        [SetUp]
        public void InitTest()
        {
            _userService = A.Fake<IUserService>();
            _eventAggregator = A.Fake<IEventAggregator>();
            _regionManager = A.Fake<IRegionManager>();
            _view = A.Fake<IUsersListView>();
            _viewModel = new UsersListViewModel(_userService, _eventAggregator, _regionManager, _view);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckUserServiceNull()
        {
            UsersListViewModel viewModel = new UsersListViewModel(null, _eventAggregator, _regionManager, _view);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckEventAggregatorNull()
        {
            UsersListViewModel viewModel = new UsersListViewModel(_userService, null, _regionManager, _view);
        }

        [Test]
        public void LoadCorrectData()
        {
            A.CallTo(() => _userService.GetUsers())
                .Returns((new List<User>() { new User { Name = "Daniel" } }));
                        
            _viewModel.Load();

            A.CallTo(() => _userService.GetUsers()).MustHaveHappened();
            Assert.AreEqual(1, _viewModel.Users.Count);
            Assert.AreEqual("Daniel", _viewModel.Users[0].Name);
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
