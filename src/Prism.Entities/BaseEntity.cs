using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace Prism.Entities
{
    public class BaseEntity : INotifyPropertyChanged, IDataErrorInfo
    {
        #region INotifyPropertyChanged        

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

        #region IDataErrorInfo

        public string Error
        {
            get {
                throw new NotImplementedException();
            }
        }

        public string this[string property]
        {
            get
            {
                PropertyInfo propertyInfo = this.GetType().GetProperty(property);
                var results = new List<ValidationResult>();

                var result = Validator.TryValidateProperty(
                                          propertyInfo.GetValue(this, null),
                                          new ValidationContext(this, null, null)
                                          {
                                              MemberName = property
                                          },
                                          results);

                if (!result)
                {
                    var validationResult = results.First();
                    return validationResult.ErrorMessage;
                }

                return string.Empty;
            }
        }

        public bool IsValid
        {
            get
            {
                bool isValid = true;
                foreach (var property in this.GetType().GetProperties().Where(p => p.Name != "Error" && p.Name != "IsValid" && p.Name != "Item"))
                {
                    if (string.IsNullOrEmpty(this[property.Name]) == false)
                    {
                        isValid = false;
                        break;
                    }
                }
                return isValid;
            }
        }
        #endregion   
    }
}
