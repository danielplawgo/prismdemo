using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;
using Prism.Infrastucture.Messages;

namespace Prism.Infrastucture
{
    /// <summary>
    /// Klasa bazowa dla usług (nie mylić z usługami np. wcf, klasy usług do 
    /// klasy wykorzystywane przez viewmodel do wykonywania jakiś czynności
    /// np. pobierania danych, gdzie w tych klasach jest zawarta logika
    /// jak to robić np. pobierać dane w partiach, a nie za jednym zamachem).
    /// </summary>
    public class BaseService : IService
    {
        protected IEventAggregator _eventAggregator;

        public BaseService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Pomocnicza metoda do aktualizacji statusu
        /// </summary>
        /// <param name="status"></param>
        protected void UpdateStatus(string status)
        {
            _eventAggregator.GetEvent<UpdateStatusEvent>().Publish(new UpdateStatusMessage()
            {
                Value = status
            });
        }
    }
}
