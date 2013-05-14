using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;
using Prism.Infrastucture;
using Prism.Infrastucture.Messages;
using Prism.StatusBarModule.Views;

namespace Prism.StatusBarModule.ViewModels
{
    public class StatusBarViewModel : BaseViewModel, IStatusBarViewModel
    {
        private IEventAggregator _eventAggregator;

        public StatusBarViewModel(IEventAggregator eventAggregator, IStatusBarView view)
            : base(view)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<UpdateStatusBarEvent>().Subscribe(ProcessUpdateStatusBarMessage);
        }

        private void ProcessUpdateStatusBarMessage(UpdateStatusBarMessage message)
        {
            Value = message.Value;
        }

        private string _value;
        public  string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(() => this.Value);
                }
            }
        }
    }
}
