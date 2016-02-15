using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4WPF
{
    public class SquareViewModel : INotifyPropertyChanged
    {
        private int _Token;
        public int Token
        {
            get
            {
                return _Token;
            }
            set
            {
                if (_Token.Equals(value)) return;

                _Token = value;
                RaisePropertyChanged("Token");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            var handlers = PropertyChanged;
            if (handlers != null)
            {
                var args = new PropertyChangedEventArgs(property);
                handlers(this, args);
            }
        }
    }
}
