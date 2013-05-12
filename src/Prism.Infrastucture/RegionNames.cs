using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.Infrastucture
{
    /// <summary>
    /// Klasa stałych określających nazwy regionów w shellu.
    /// W kodzie nie korzystamy z stringów tylko z tych stałych.
    /// Przez co mamy wyłapywane literówki na etapie kompilacji, a nie działaniu.
    /// Dodatkowo refaktoryzacja zamieni wystąpienie, gdy będziemy chceli zmienić nazwę regionu,
    /// co w przypadku stringów trzeba by robić ręcznie.
    /// 
    /// Tutaj nie korzystam z statyły const, które są efektywniejsze ponieważ te wartości będą
    /// wykorzystywane również w xamlu.
    /// </summary>
    public class RegionNames
    {
        public static readonly string Main = "Main";
        public static readonly string Menu = "Menu";
        public static readonly string StatusBar = "StatusBar";
    }
}
