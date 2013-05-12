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
    /// <summary>
    /// Klasa bazowa dla obiektów enyjnych np. User.
    /// Klasa zawiera implementacje dwóch istotnych intefrejsów z punktu widzenia WPFA.
    /// INotifyPropertyChanged służy do informowania wpfa, że właściwość się zmieniła,
    /// gdy wykorzystujemy binding. Przez co wpf zaaktualizuje odpowiednio widok.
    /// W klasie bazowej dodane są dwie wersje metody OnPropertyChanged, które wywołują zdarzenie
    /// z interfejsu INotifyPropertyChanged.
    /// Obie metody oczekują w parametrze metody nazwę właściwości, która się zmieniła.
    /// Różnicą jest forma parametru.
    /// Pierwsza metody przyjmuje napis w postaci stringa (nie zalecana).
    /// Druga metoda przyjmuje wyrażenia lamda (zalecana).
    /// Plusem drugiej wersji metody jest to, że mamy sprawdzanie poprawności na etapie
    /// komplikacji koodu. Przez co literówka zostanie duzo szybciej wychwycona niż w 
    /// przypadku stringa.Doatkowo refaktoryzacja nazwy właściwości zamieni również w
    /// wyrażeniu lamda nazwę (przy stringach trzeba robić to ręcznie).
    /// Wersja z wyrażeniem lamda jest trochę mniej wydajne ale nie na tyle, aby
    /// przeważyć plusy wykorzystania.
    /// 
    /// IDataErrorInfo służy do wykonywania walidacji właściwości, gdy jest wykorzystywany
    /// binding. Dzięki wykorzystaniu tego intefrejsu możemy określić reguły walidacyjne na 
    /// poziomie encji i będą one wykorzystywane w interfejscie (o ile wykorzystamy binding).
    /// Plusem jest że w jednym miejscu mamy wszystkie reguły i każdy widok będzie je wykorzystywał.
    /// Zmiana reguł spowudje, że każdy widok automatycznie będzie wykorzystywał nowe reguły.
    /// Do określenia reguł wykorzystywane są atrybuty DataAnnotations.
    /// Można też tworzyć swoje własne attrybuty.
    /// </summary>
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
