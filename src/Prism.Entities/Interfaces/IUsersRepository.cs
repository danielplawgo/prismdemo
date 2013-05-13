using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Entities.Users;

namespace Prism.Entities.Interfaces
{
    /// <summary>
    /// Interfejs dla klas zwracających dane - odpowiednik PharmacyCore
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Metoda zwraca dane. Co istotne metodanie nie zwraca wszystkich danych w
        /// ramach jednego requestu, tylko umożliwia wyciągane danych partiami.
        /// Przez co możemy anulować ściąganie danych między poszczególnymi requestami,
        /// bez konieczności siłowego ubijania wątku/operacji. Dodatkowo takie rozwiązanie
        /// daje nam możliwość ponowienia operacji (gdy na przykład nastąpiło rozłączenie)
        /// bez konieczności ściągania lub wysyłąnia danych od początku.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IEnumerable<User> GetUsers(int index, int count);

        /// <summary>
        /// Metoda zwracająca ilość danych do pobrania. Potrzebne do metody GetUsers.
        /// </summary>
        /// <returns></returns>
        int GetUsersCount();
    }
}
