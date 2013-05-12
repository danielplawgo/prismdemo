using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.Module1
{
    /// <summary>
    /// Klasa dla stałych napisowych. W kodzie aplikacji nie używamy stringów, tylko
    /// przerzucamy ich do tego typu klas. Najlepiej korzystać z const ponieważ 
    /// w trakcie kompilacji są one zamieniane na napisy w miejscu ich użycia.
    /// Jeśli coś ma być używane w xamlu to musi to być statyczna właściwość 
    /// (przykład w klasie RegionNames).
    /// </summary>
    public class Strings
    {
        public const string User = "User";
    }
}
