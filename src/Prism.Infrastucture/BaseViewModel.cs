using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.Practices.Prism.Regions;

namespace Prism.Infrastucture
{
    /// <summary>
    /// Klasa bazowa dla viewmodeli.
    /// Klasa zawiera referencje do widoku oraz implementuje intefrejs INotifyPropertyChanged.
    /// Więcej informacji o interfejscie znajduje się w komentarzu do klasy BaseEntity.
    /// </summary>
    public class BaseViewModel : IViewModel
    {
        public BaseViewModel(IView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }
            View = view;
            View.ViewModel = this;
        }

        public IView View { get; set; }

        public event EventHandler<EventArgs> Closed;

        protected void Close()
        {
            var closed = Closed;
            if (closed != null)
            {
                EventArgs args = new EventArgs();
                closed(this, args);
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> propertyExpresion)
        {
            var property = propertyExpresion.Body as MemberExpression;
            if (property == null || !(property.Member is PropertyInfo) ||
                !IsPropertyOfThis(property))
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Expression must be of the form 'this.PropertyName'. Invalid expression '{0}'.",
                    propertyExpresion), "propertyExpression");
            }

            this.OnPropertyChanged(property.Member.Name);
        }

        private bool IsPropertyOfThis(MemberExpression property)
        {
            var constant = RemoveCast(property.Expression) as ConstantExpression;
            return constant != null && constant.Value == this;
        }

        private Expression RemoveCast(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Convert ||
                expression.NodeType == ExpressionType.ConvertChecked)
                return ((UnaryExpression)expression).Operand;

            return expression;
        }

        protected void OnPropertyChanged(
            string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("argument propertyName cannot by null oraz empty", "propertyName");
            }

            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler == null)
            {
                return;
            }

            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        
    }
}
