using System.Collections;
using System.Threading.Tasks;
using Prism.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Infrastucture;

namespace Prism.Module1.Service
{
    /// <summary>
    /// interfejsc dla usług obsługi danych użytkowników.
    /// </summary>
    public interface IUserService : IService
    {
        /// <summary>
        /// Metoda zwracająca dane w sposób synchroniczny.
        /// Niezalecana ponieważ powoduje blokowanie interfejsu użytkownika.
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetUsers();

        /// <summary>
        /// Metoda zwracająca dane w sposób asynchroniczny.
        /// Zalecana, nie powoduje blokwania interfejsu użytkownika.
        /// Metodę używać z słowem kluczowym await z .net 4.5
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetUsersAsync();

        /// <summary>
        /// Metoda zapisująca dane użytkownika
        /// </summary>
        /// <param name="user"></param>
        void Save(User user);

        /// <summary>
        /// Metoda anulująca pobieranie danych z źródła danych.
        /// Sens jest wywołania jest w przypadku, gdy wyciągamy dane w sposób asynchroniczny.
        /// </summary>
        void CancelGetUsersData();
    }
}
