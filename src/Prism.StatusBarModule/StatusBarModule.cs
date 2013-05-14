using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Prism.Infrastucture;
using Prism.StatusBarModule.Messages;
using Prism.StatusBarModule.ViewModels;
using Prism.StatusBarModule.Views;

namespace Prism.StatusBarModule
{
    public class StatusBarModule : BaseModule
    {
        protected IRegion StatusBarRegion { get; set; }

        public StatusBarModule(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggreagator)
            :base(regionManager, container, eventAggreagator)
        {}

        public override void Initialize()
        {
            base.Initialize();

            StatusBarRegion = RegionManager.Regions[RegionNames.StatusBar];

            EventAggregator.GetEvent<ShowStatusBarEvent>().Publish(new ShowStatusBarMessage());
        }

        protected override void ConfigureContainer()
        {
            Container.RegisterType<IStatusBarView, StatusBarView>();
            Container.RegisterType<IStatusBarViewModel, StatusBarViewModel>();
        }

        protected override void ConfigureEventAggregator()
        {
            EventAggregator.GetEvent<ShowStatusBarEvent>().Subscribe(ProcessShowStatusBarMessage, ThreadOption.UIThread, true);
        }

        private IStatusBarViewModel _statusBarViewModel;
        private void ProcessShowStatusBarMessage(ShowStatusBarMessage message)
        {
            if (_statusBarViewModel == null)
            {
                _statusBarViewModel = Container.Resolve<IStatusBarViewModel>();
                StatusBarRegion.Add(_statusBarViewModel.View);
            }

            StatusBarRegion.Activate(_statusBarViewModel.View);
        }
    }
}
