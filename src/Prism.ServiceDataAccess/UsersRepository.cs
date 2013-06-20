using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Prism.Entities.Interfaces;
using Prism.Entities.Users;

namespace Prism.ServiceDataAccess
{
    /// <summary>
    /// Przykładowa implementacji klasy zwracającej dane.
    /// Potrzebna jest ona do testów.
    /// W normalnym Pharmacy będzie to usługa wcfowa.
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private List<User> _users;

        public UsersRepository()
        {
            _users = new List<User>();
            _users.Add(new User() { Name = "Daniel" });
            _users.Add(new User() { Name = "Tomek" });
            _users.Add(new User() { Name = "Ola" });
            _users.Add(new User() { Name = "Kubek" });

        }

        /// <summary>
        /// Wątek usypiamy ponieważ chcemy przetestować długotrwałość operacji.
        /// Losowe wyrzucenie wyjątku ma zasymulować problem z ściąganiem danych
        /// z usługi, np. poprzez chwilowy brak dostępu do internetu.
        /// Wyrzucenie wyjątku służy do przetestowania, ponawiania próby
        /// połączenia z usługą oraz ponownego pobrania danych.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<User> GetUsers(int index, int count)
        {
            Thread.Sleep(1000);

            Random rand = new Random();
            if (rand.Next(4)%4 != 0)
            {
                throw new Exception("Problem z pobraniem danych z bazy");
            }

            return _users.Skip(index).Take(count);
        }


        public int GetUsersCount()
        {
            return _users.Count;
        }
    }
}
