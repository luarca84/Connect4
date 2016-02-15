using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4WPF
{
    public class BoardViewModel : INotifyPropertyChanged
    {

        int numRows = 6;
        int numColumns = 7;

        public BoardViewModel()
        {
            var rand = new Random();
            Squares = Enumerable
                .Range(1, NumRows * NumColumns)
                .Select(a => new SquareViewModel() { Token = -1 })// rand.Next(-1, 2) })
                .ToList();
        }

        public List<SquareViewModel> Squares { get; set; }

        public int NumRows
        {
            get
            {
                return numRows;
            }

            set
            {
                numRows = value;
                RaisePropertyChanged("NumRows");
            }
        }

        public int NumColumns
        {
            get
            {
                return numColumns;
            }

            set
            {
                numColumns = value;
                RaisePropertyChanged("NumColumns");
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
