using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino.Toolkit.Wpf.Samples
{
   public abstract class ModelBase:BindableBase, INotifyDataErrorInfo
    {
        private ErrorsContainer<string> _errorsContainer;

        public bool HasErrors
        {
            get { return ErrorsContainer.HasErrors; }
        }

        public ErrorsContainer<string> ErrorsContainer
        {
            get
            {
                if (_errorsContainer == null)
                {
                    _errorsContainer =
                        new ErrorsContainer<string>(pn => RaiseErrorsChanged(pn));
                }

                return _errorsContainer;
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return ErrorsContainer.GetErrors(propertyName);
        }

        protected void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
