using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using System.Windows;

namespace Prism.Shell
{
    /// <summary>
    /// Bootstrapper to klasa odpowiedzialna za uruchomienie oraz załadowanie całej aplikacji.
    /// Aby korzystać z Bootstrapera trzeba zrobić dwie rzeczy:
    ///     *usunąć z App.xmal atrybut StatupUri
    ///     *nadpisać metodą w App.xmal.cs OnStartup - szczegóły w tej metodzie
    /// </summary>
    public class Bootstrapper : UnityBootstrapper
    {
        /// <summary>
        /// Metoda służy do utworzenia okna głównego aplikacji, do którego będą ładowane widoki
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        /// <summary>
        /// Ten kod zawsze wygląda sam samo, czy ustawienie shella jako głównego okna
        /// </summary>
        protected override void InitializeShell()
        {
            base.InitializeShell();
            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
        }

        /// <summary>
        /// W metodzie tej określamy jakie moduły będą w danej chwili dostępne. Możemy na przykład ładować moduły na podstawie jakieś logiki
        /// np. odpytujemy PharmacyService i dostajemy listę modułów do załadowania.
        /// 
        /// Prism umożliwia jeszcze poza ładowaniem modułów z kodu również ładowanie na podstawie pliku konfiguracyjnego
        /// lub dynamicznie na podstawie skanowania assemlby
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(Module1.Module1));//Aby skorzystać z modułu musimy go dodać do katalogu. Możemy też określić, kiedy moduł ma być ładowany
            //np. w momencie, kiedy będzie potrzebny lub przy starcie aplikacji
            moduleCatalog.AddModule(typeof (StatusBarModule.StatusBarModule));
            moduleCatalog.AddModule(typeof (DialogBoxModule.DialogBoxModule));
        }

        /// <summary>
        /// Globalna konfiguracja kontenera Unity. Rejestrujemy tutaj typy globalne dla całej aplikacji (wykorzystywane między modułami).
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            //Rejestracja typów w Unity wygląda w ten sposób, że określamy interfejs jaki nas interensuje oraz 
            //jaka konkretnie klasa będzie w danym momencie wykorzystywana dla danego interfejsu.
            //Następnie w całej aplikacji korzystamy z interfejsu, a Unity będzie zwracał instancje zarejestrowanej klasy.
            //Rejestracja typów nie musi odbywać się w kodzie. Można też zrobić to z poziomu pliku konfiguracyjnego
            //Dzięki czemu zmiana informacji w nim będzie powodować zmiane zależności między typamie bez potrzeby rekompiliacji kodu
            //
            //Rejestrujemy EventAggregatora. Istotny jest tutaj parametr metody. Oznacza on, że w całej aplikacji
            //będzie tylko jedna instancje EventAggregatora (na czym nam zależy). Gdyby nie było tego parametru
            //wtedy za każdym razem byłaby zwracana nowa instacja EventAggregatora.
            //W większości sytuacji nie będziemy podawać parametru metody.
            Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
        }
    }
}
