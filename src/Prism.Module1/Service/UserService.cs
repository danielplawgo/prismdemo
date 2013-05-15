using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;
using Prism.Entities.Interfaces;
using Prism.Entities.Users;
using Prism.Infrastucture;
using Prism.Infrastucture.Messages;

namespace Prism.Module1.Service
{
    /// <summary>
    /// Przykładowan implementacja usługi wyciągającej dane z jakieś źródła danych (np. bazy danych
    /// usługi wcfowej takiej jak PharmacyService).
    /// Klasa opakowuje logikę efektywnej pracy z danymi.
    /// Czyli z źródła danych jakim jest IUserRepository wyciąga dane nie w ramach jednego
    /// requesty, tylko w pętli do określoną liczbę rekordów na raz. Dzięki czemu w sytuacji, gdy
    /// nastąpi chwilowy problem np. z połączniem z internetem, nie będzie trzeba od nowa ściągać
    /// wszystkich danych, tylko brakujące dane. Podobnie można zrobić z wysyłaniem dużej ilości danych 
    /// do usługi.
    /// Poniższa usługa jest w stanie wyciągać dane w sposób synchroniczny i asynchroniczny.
    /// Sposób asynchroniczny jest zalecany ponieważ nie blokuje interfejsu użytkownika oraz
    /// umożliwia przerwanie ściągania danych bez konieczności siłowego zapisania wątku pobierającego dane
    /// (co po stronie usługi będzie powodowało wyjątek).
    /// Asynchroniczne wyciąganie danych jest przystosowane do nowej funkcjonalności słów kluczowych
    /// async oraz await z .net 4.5. 
    /// </summary>
    public class UserService : BaseService, IUserService
    {
        /// <summary>
        /// źródło danych.
        /// </summary>
        private IUsersRepository _usersRepository;

        

        /// <summary>
        /// Określa jak dużo obiektów będzie pobierane w jednej iteracji ściągania danych z usługi.
        /// Dla testów ściągany będzie jeden obiekt.
        /// </summary>
        private int _numberOfPieces = 1;

        public UserService(EventAggregator eventAggregator, IUsersRepository usersRepository)
            :base(eventAggregator)
        {
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Wartość określa ile razy próbujemy ponowić pobranie danych.
        /// </summary>
        private int _retryCount = 3;

        /// <summary>
        /// Synchroniczna metoda pobierająca dane.
        /// Przy asynchronicznym pobieraniu danych jest również wykorzystywana ta metoda
        /// (dlatego niektóre jest elementy mają sens tylko przy asynchroniczny uruchomieniu),
        /// tylko, że jest ona uruchomiona w innym wątku w wykorzystaniu klasy Task, 
        /// która jest wykorzystywana przez słowa kluczowe async oraz await.
        /// Logika działania metody jest następująca:
        /// W pierwszej kolejności wyciągamy listę użytkowników.
        /// Następnie iteracyjnie ściągamy w partiach listę użytkowników i zapisujemy do wynikowej kolekcji.
        /// Zwracamy wynikową kolekcję.
        /// Między poszczególnymi iteracjami sprawdzamy, czy czas nie trzeba przerwać ściągania danych,
        /// poprzez sprawdzanie flagi _cancelGetUsers, którą ustawia metoda CancelGetUsersData.
        /// W metodzie GetUsers UserRepository jest uśpienie wątku na sekundę, aby zasymulować operacje długotrwałą.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUsers()
        {
            List<User> users = new List<User>();
            int usersCount = _usersRepository.GetUsersCount();
            int currentRetryCount = 0;
            for (int i = 0; i < usersCount; i += _numberOfPieces)
            {
                if (_cancelGetUsers)
                {
                    _cancelGetUsers = false;
                    break;
                }
                //Obsługujemy możliwość problemu z połączeniem z usługą.
                //Gdy wystąpi wyjątek próbujemy ponowić operację.
                try
                {
                    users.AddRange(_usersRepository.GetUsers(i, _numberOfPieces));
                    currentRetryCount = 0;
                    UpdateStatus("Połączony z usługą...");
                }
                catch (Exception ex)
                {
                    UpdateStatus("Rozłączony z usługą...");
                    if (currentRetryCount < _retryCount)
                    {
                        i -= _numberOfPieces;
                        currentRetryCount++;
                        continue;
                    }
                    throw new InfrastuctureException("Nastąpił problem z pobraniem danych", ex);
                }
            }
            return users;
        }

        public void Save(User user)
        {

        }

        /// <summary>
        /// Fajność nowości .net 4.5 w formie słów kluczowych async oraz await jest to,
        /// że możemy wykorzystam synchroniczną metodę do wyciągania danych.
        /// Jedynie co musimy zrobić to synchronicznej wersji metody musimy dodać obsługę anulowania wyciągania danych,
        /// oraz samą metodę uruchomić w wątku w tle z wykorzystaniem klasy Task.
        /// Parametr generyczny klasy task to typ zwracany przez metodę synchroniczną.
        /// Istotne jest uruchomienie metody Start na obiekcie klasy Task, bo dopiero on
        /// powoduje wywołanie metody synchronicznej w wątku w tle.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<User>> GetUsersAsync()
        {
            var task = new Task<IEnumerable<User>>(GetUsers);
            task.Start();
            return task;
        }

        /// <summary>
        /// Metoda wykorzystywana przez viewmodel do anulowanie wyciągania danych.
        /// Sens jest wywołania jest jedynie wtedy, gdy dane są wyciągane w sposób asynchroniczny.
        /// </summary>
        private bool _cancelGetUsers = false;
        public void CancelGetUsersData()
        {
            _cancelGetUsers = true;
        }
    }
}
