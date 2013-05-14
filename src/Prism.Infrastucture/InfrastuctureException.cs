using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Infrastucture
{
    public class InfrastuctureException : Exception
    {
        public InfrastuctureException()
            : base(){}

        public  InfrastuctureException(string message)
            : base(message){}

        public InfrastuctureException(string message, Exception innerException)
            : base(message, innerException){}
    }
}
