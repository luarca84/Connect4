using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Connect4WPF
{
    public class BoardViewModel : INotifyPropertyChanged
    {
        int numRows = 6;
        int numColumns = 7;
        private List<SquareViewModel> squares;
        private List<string> options = new List<string>();
        private string selectedOption = OPTION_HUMANO;
        const string OPTION_HUMANO = "HUMANO";
        const string OPTION_ORDENADOR = "ORDENADOR";

        public BoardViewModel()
        {
            options = new List<string>();
            options.Add(OPTION_HUMANO);
            options.Add(OPTION_ORDENADOR);
            NuevoJuego();
        }

        private void NuevoJuego()
        {
            var rand = new Random();
            Squares = Enumerable
                .Range(1, NumRows * NumColumns)
                .Select(a => new SquareViewModel() { Token = (int)TypeToken.TOKEN_NONE })// rand.Next(-1, 2) })
                .ToList();

            currentPlayer = TypeToken.TOKEN_BLACK;
        }

        public List<SquareViewModel> Squares
        {
            get
            {
                return squares;
            }

            set
            {
                squares = value;
                RaisePropertyChanged("Squares");
            }
        }

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

        public List<string> Options
        {
            get
            {
                return options;
            }

            set
            {
                options = value;
                RaisePropertyChanged("Options");
            }
        }

        public string SelectedOption
        {
            get
            {
                return selectedOption;
            }

            set
            {
                selectedOption = value;
                RaisePropertyChanged("SelectedOption");
            }
        }


        #region ClickCommand

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => MyAction(), CanExecuteAction()));
            }
        }



        private bool CanExecuteAction()
        {
            return true;
        }

        public void MyAction()
        {
            NuevoJuego();
        }

        #endregion

        public void ClickBtnColumn(int column)
        {
            SquareViewModel s = GetEmptySquareInColumn(column);
            if (s != null)
            {
                s.Token = (int)currentPlayer;

                bool checkEndGame_CurrentPlayerWins = CheckEndGame_CurrentPlayerWins();
                bool checkEndGame_NoEmptySquares = CheckEndGame_NoEmptySquares();

                if (checkEndGame_CurrentPlayerWins)
                {
                    string msg = "Red player wins!";
                    if (currentPlayer == TypeToken.TOKEN_BLACK)
                        msg = "Black player wins!";
                    MessageBox.Show(msg);
                    NuevoJuego();
                }
                else if (checkEndGame_NoEmptySquares)
                {
                    MessageBox.Show("Draw, Empate");
                    NuevoJuego();
                }
                else
                {
                    if (currentPlayer == TypeToken.TOKEN_BLACK)
                        currentPlayer = TypeToken.TOKEN_RED;
                    else
                        currentPlayer = TypeToken.TOKEN_BLACK;
                }

                if (SelectedOption == OPTION_ORDENADOR && currentPlayer == TypeToken.TOKEN_RED)
                {
                    OrdenadorIA ordenadorIA = new OrdenadorIA(Squares, NumRows, NumColumns);
                    int columnSelectedByIA = ordenadorIA.GetSelectedColumn();
                    //Random r = new Random();
                    //int columnSelectedByIA = 0;
                    //do
                    //{
                    //    columnSelectedByIA = r.Next(0, NumColumns);
                    //}
                    //while (GetEmptySquareInColumn(columnSelectedByIA) == null);
                    ClickBtnColumn(columnSelectedByIA);
                }
            }
        }

        private bool CheckEndGame_NoEmptySquares()
        {
            foreach (SquareViewModel s in Squares)
                if (s.Token == (int)TypeToken.TOKEN_NONE)
                    return false;
            return true;
        }

        private bool CheckEndGame_CurrentPlayerWins()
        {
            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumColumns; col++)
                {
                    //Horizontal
                    SquareViewModel s1H = GetSquare(row, col);
                    SquareViewModel s2H = GetSquare(row, col + 1);
                    SquareViewModel s3H = GetSquare(row, col + 2);
                    SquareViewModel s4H = GetSquare(row, col + 3);
                    bool squaresSameColorH = CheckSquaresSameColor(s1H, s2H, s3H, s4H);
                    if (squaresSameColorH)
                        return true;
                    //Vertical
                    SquareViewModel s1V = GetSquare(row, col);
                    SquareViewModel s2V = GetSquare(row + 1, col);
                    SquareViewModel s3V = GetSquare(row + 2, col);
                    SquareViewModel s4V = GetSquare(row + 3, col);
                    bool squaresSameColorV = CheckSquaresSameColor(s1V, s2V, s3V, s4V);
                    if (squaresSameColorV)
                        return true;

                    //Diagonal
                    SquareViewModel s1D = GetSquare(row, col);
                    SquareViewModel s2D = GetSquare(row + 1, col + 1);
                    SquareViewModel s3D = GetSquare(row + 2, col + 2);
                    SquareViewModel s4D = GetSquare(row + 3, col + 3);
                    bool squaresSameColorD = CheckSquaresSameColor(s1D, s2D, s3D, s4D);
                    if (squaresSameColorD)
                        return true;

                    //Diagonal2
                    SquareViewModel s1D2 = GetSquare(row, col);
                    SquareViewModel s2D2 = GetSquare(row + 1, col - 1);
                    SquareViewModel s3D2 = GetSquare(row + 2, col - 2);
                    SquareViewModel s4D2 = GetSquare(row + 3, col - 3);
                    bool squaresSameColorD2 = CheckSquaresSameColor(s1D2, s2D2, s3D2, s4D2);
                    if (squaresSameColorD2)
                        return true;
                }
            }
            return false;
        }

        private bool CheckSquaresSameColor(SquareViewModel s1, SquareViewModel s2, SquareViewModel s3, SquareViewModel s4)
        {
            if (s1 != null && s2 != null && s3 != null && s4 != null)
            {
                return ((int)currentPlayer == s1.Token)
                    && ((int)currentPlayer == s2.Token)
                    && ((int)currentPlayer == s3.Token)
                    && ((int)currentPlayer == s4.Token);
            }
            return false;
        }

        TypeToken currentPlayer = TypeToken.TOKEN_BLACK;

        public SquareViewModel GetSquare(int row, int column)
        {
            if (row >= 0 && row < NumRows && column >= 0 && column < NumColumns)
            {
                int x = row * NumColumns + column;
                if (x >= 0 && x < Squares.Count)
                    return Squares[x];
            }

            return null;
        }

        public SquareViewModel GetEmptySquareInColumn(int column)
        {
            SquareViewModel s = null;
            for (int row = NumRows - 1; row >= 0; row--)
            {
                if (GetSquare(row, column).Token == (int)TypeToken.TOKEN_NONE)
                {
                    s = GetSquare(row, column);
                    break;
                }
            }
            return s;
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

    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
