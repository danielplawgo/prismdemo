using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Prism.DialogBoxModule.ViewModels;
using Prism.DialogBoxModule.Views;
using Prism.Infrastucture;
using Prism.Infrastucture.Messages;

namespace Prism.DialogBoxModule
{
    public class DialogBoxModule : BaseModule
    {
        public DialogBoxModule(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggreagator)
            : base(regionManager, container, eventAggreagator)
        {
            
        }

        protected override void ConfigureContainer()
        {
            Container.RegisterType<IDialogBoxView, DialogBoxView>();
            Container.RegisterType<IDialogBoxViewModel, DialogBoxViewModel>();
            Container.RegisterType<IDialogBoxService, DialogBoxService>();
        }

        protected override void ConfigureEventAggregator()
        {
            
        }
    }
}
