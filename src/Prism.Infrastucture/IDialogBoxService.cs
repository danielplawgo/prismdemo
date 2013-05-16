using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Infrastucture
{
    /// <summary>
    /// Interfejs dla usługi odpowiedzialnej za wyświetlanie widoków w oknie dialogowym.
    /// </summary>
    public interface IDialogBoxService
    {
        bool? ShowDialog(IViewModel viewModel);
    }
}
