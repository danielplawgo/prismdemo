﻿using System;
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

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
        private bool _viewIsAddedToRegion = false;
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (_viewIsAddedToRegion == false)
            {
                navigationContext.NavigationService.Region.Add(View);
                _viewIsAddedToRegion = true;
            }
            navigationContext.NavigationService.Region.Activate(View);
        }
    }
}