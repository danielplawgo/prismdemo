using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Entities;
using System.ComponentModel.DataAnnotations;

namespace Prism.Entities.Users
{
    public class User : BaseEntity
    {
        private string _name;

        [Required(ErrorMessage="Name is required")]
        [StringLength(3, ErrorMessage="to long")]        
        public string Name
        {
            get { return _name; }
            set {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(() => this.Name);
                }
            }
        }
        
    }
}
