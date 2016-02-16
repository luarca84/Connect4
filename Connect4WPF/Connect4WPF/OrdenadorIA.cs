using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4WPF
{
    public enum WinState { BLACKWIN, REDWIN, TIE };

    class OrdenadorIA
    {
        private const int DEEPNESS = 2;
        int Strength = 30000;
        Board board = null;
        int selectedColumn = 0;
        MinMax alg = new MinMax();
        public OrdenadorIA(List<SquareViewModel> squares, int numRows, int numColumns)
        {
            Strength = 40000;
            alg.SetStrength(Strength);

            board = new Board(squares, numRows, numColumns);
            List<int> possibleMoves = board.GetPossibleMoves();
            // represents the best move value (start at obsurdly low)
            float best = float.MinValue;

            // the column that the AI will play
            int bestColumn = -1;

            // loop through each column and check the value of the move
            foreach (int col in possibleMoves)
            {
                float value = FindMoveValue(board, col, 0);

                // if the column has a better value for the AI than the current best move
                // use that column as the best
                if (value > best)
                {
                    best = value;
                    bestColumn = col;
                }
            }

            // return the best column fo the AI to play
            selectedColumn = bestColumn;
        }

        // finds the value of a specific move in a column
        // deep is how many moves ahead the alg is looking
        public float FindMoveValue(Board GameBoard, int col, int deep)
        {
            // create temporary Board, make the best move and check for next best
            //Board newBoard = GameBoard.Copy();

            // check if the move will win the game
            //WinState? win = newBoard.CheckWinState();
            
            WinState? win = GameBoard.CheckWinState();

            // if the game is going to end with the move
            if (win != null)
            {
                // check if it will end with draw
                if (win == WinState.TIE)
                    return 0f;
                // return 1 (best) for win, and -1 (worst) for lose
                //else if (win == WinState.BLACKWIN && Game1.AIColor == BoardState.BLACK)
                else if (win == WinState.BLACKWIN && GameBoard.CurrentPlayer == TypeToken.TOKEN_BLACK)
                {
                    //if (deep == 1)
                    //    return 10000f;
                    return 1f;
                }
                //else if (win == WinState.REDWIN && Game1.AIColor == BoardState.RED)
                else if (win == WinState.REDWIN && GameBoard.CurrentPlayer == TypeToken.TOKEN_RED)
                {
                    //if (deep == 1)
                    //    return 10000f;
                    return 1f;
                }
                else
                    return -1f;
            }

            // if we have looked forward the maximum amount
            // return the value of the move
            if (deep == DEEPNESS)
            {
                // MCScore
                //int newStrength = Convert.ToInt32(Strength / ((double)Math.Pow(7, DEEPNESS)));
                //alg.SetStrength(newStrength);
                alg.SetStrength(5);

                return alg.FindDeepValue(GameBoard, col);
            }

            //newBoard.MakeMove(col);
            GameBoard.MakeMove(col);

            // Get the possible moves for the newBoard (the next move would be players)
            List<int> possibleMoves = GameBoard.GetPossibleMoves(); //newBoard.GetPossibleMoves();

            // start looking into deeper moves
            float value = float.MinValue;
            foreach (int col2 in possibleMoves)
                value = Math.Max(value, -1f * FindMoveValue(GameBoard, col2, deep + 1));

            // remove the last move made so it doesnt stay permanent
            GameBoard.Unmove(col);

            return value;
        }

        public int GetSelectedColumn()
        {
            return selectedColumn;
        }
    }

    class Square
    {
        int token;

        public int Token
        {
            get
            {
                return token;
            }

            set
            {
                token = value;
            }
        }

        public bool IsTokenNone() { return token == (int)TypeToken.TOKEN_NONE; }
        public bool IsTokenRedComputer() { return token == (int)TypeToken.TOKEN_RED; }
        public bool IsTokenBlackHuman() { return token == (int)TypeToken.TOKEN_BLACK; }
    }

    class Board
    {
        List<Square> squares = new List<Square>();
        int numRows;
        int numColumns;
        TypeToken currentPlayer;
        TypeToken opponentPlayer;

        internal List<Square> Squares
        {
            get
            {
                return squares;
            }

            set
            {
                squares = value;
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
            }
        }

        internal TypeToken CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }

            set
            {
                currentPlayer = value;
            }
        }

        internal TypeToken OpponentPlayer
        {
            get
            {
                return opponentPlayer;
            }

            set
            {
                opponentPlayer = value;
            }
        }

        public WinState? Winner { get; private set; }

        public bool isGameOver { get; private set; }

        public Board(List<SquareViewModel> squares, int numRows, int numColumns)
        {
            this.squares = new List<Square>();
            foreach (SquareViewModel s in squares)
                this.squares.Add(new Square() { Token = s.Token });
            this.NumColumns = numColumns;
            this.NumRows = numRows;
            this.CurrentPlayer = TypeToken.TOKEN_RED;
            this.OpponentPlayer = TypeToken.TOKEN_BLACK;
            Winner = null;
            isGameOver = false;
        }

        public List<int> GetPossibleMoves()
        {
            List<int> lstAvailableColumns = new List<int>();
            for (int c = 0; c < NumColumns; c++)
                if (GetEmptySquareInColumn(c) != null)
                    lstAvailableColumns.Add(c);
            return lstAvailableColumns;
        }

        //internal int CalculateBestColumn()
        //{
        //    List<int> lstAvailableColumns = new List<int>();
        //    for (int c = 0; c < NumColumns; c++)
        //        if (GetEmptySquareInColumn(c) != null)
        //            lstAvailableColumns.Add(c);

        //    //Random r = new Random();
        //    //int selectedIndex = r.Next(0, lstAvailableColumns.Count);
        //    //int selectedCol = lstAvailableColumns[selectedIndex];
        //    foreach (int col in lstAvailableColumns)
        //    {
        //        Board b = new Board()
        //    }


        //    return selectedCol;
        //}

        public Board() { }

        public Board Copy()
        {
            Board b = new Board();
            b.numColumns = this.numColumns;
            b.NumRows = this.NumRows;
            b.currentPlayer = this.currentPlayer;
            b.opponentPlayer = this.opponentPlayer;
            b.squares = new List<Square>();
            foreach (Square s in this.squares)
                b.squares.Add(new Square() { Token = s.Token });
            b.isGameOver = this.isGameOver;
            b.Winner = this.Winner;
            return b;
        }

        public bool CheckForWinner()
        {
            if ((Winner = CheckWinState()) != null)
            {
                isGameOver = true;
                return true;
            }
            else
                return false;
        }

        public void MakeMove(int col)
        {
            Square s = GetEmptySquareInColumn(col);
            s.Token = (int)currentPlayer;

            NextPlayer();
        }

        private void NextPlayer()
        {
            currentPlayer = currentPlayer == TypeToken.TOKEN_BLACK ? TypeToken.TOKEN_RED : TypeToken.TOKEN_BLACK;
            opponentPlayer = opponentPlayer == TypeToken.TOKEN_BLACK ? TypeToken.TOKEN_RED : TypeToken.TOKEN_BLACK;
        }

        public WinState? CheckWinState()
        {
            // check the current player for winstate
            // if the winstate is still null after return, move to next player

            // check for a draw
            bool draw = true;
            for (int col = 0; col < NumColumns; col++)
            {
                if (GetEmptySquareInColumn(col) != null)
                    draw = false;
            }
            if (draw)
                return WinState.TIE;

            if (CurrentPlayer == TypeToken.TOKEN_RED)
            {
                if (CheckEndGame_CurrentPlayerWins())
                    return WinState.REDWIN;
            }
            else
            {
                if (CheckEndGame_CurrentPlayerWins())
                    return WinState.BLACKWIN;
            }

            return null;
        }

        public void Unmove(int col)
        {
            Square s = GetLastSquareInColumn(col);

            s.Token = (int)TypeToken.TOKEN_NONE;

            NextPlayer();
        }

        public Square GetSquare(int row, int column)
        {
            if (row >= 0 && row < NumRows && column >= 0 && column < NumColumns)
            {
                int x = row * NumColumns + column;
                if (x >= 0 && x < Squares.Count)
                    return Squares[x];
            }

            return null;
        }

        public Square GetEmptySquareInColumn(int column)
        {
            Square s = null;
            for (int row = NumRows - 1; row >= 0; row--)
            {
                if (GetSquare(row, column).IsTokenNone())
                {
                    s = GetSquare(row, column);
                    break;
                }
            }
            return s;
        }

        public Square GetLastSquareInColumn(int column)
        {
            Square s = null;
            for (int row = 0; row < NumRows; row++)
            {
                if (!GetSquare(row, column).IsTokenNone())
                {
                    s = GetSquare(row, column);
                    break;
                }
            }
            return s;
        }


        private bool CheckEndGame_CurrentPlayerWins()
        {
            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumColumns; col++)
                {
                    //Horizontal
                    Square s1H = GetSquare(row, col);
                    Square s2H = GetSquare(row, col + 1);
                    Square s3H = GetSquare(row, col + 2);
                    Square s4H = GetSquare(row, col + 3);
                    bool squaresSameColorH = CheckSquaresSameColor(s1H, s2H, s3H, s4H);
                    if (squaresSameColorH)
                        return true;
                    //Vertical
                    Square s1V = GetSquare(row, col);
                    Square s2V = GetSquare(row + 1, col);
                    Square s3V = GetSquare(row + 2, col);
                    Square s4V = GetSquare(row + 3, col);
                    bool squaresSameColorV = CheckSquaresSameColor(s1V, s2V, s3V, s4V);
                    if (squaresSameColorV)
                        return true;

                    //Diagonal
                    Square s1D = GetSquare(row, col);
                    Square s2D = GetSquare(row + 1, col + 1);
                    Square s3D = GetSquare(row + 2, col + 2);
                    Square s4D = GetSquare(row + 3, col + 3);
                    bool squaresSameColorD = CheckSquaresSameColor(s1D, s2D, s3D, s4D);
                    if (squaresSameColorD)
                        return true;

                    //Diagonal2
                    Square s1D2 = GetSquare(row, col);
                    Square s2D2 = GetSquare(row + 1, col - 1);
                    Square s3D2 = GetSquare(row + 2, col - 2);
                    Square s4D2 = GetSquare(row + 3, col - 3);
                    bool squaresSameColorD2 = CheckSquaresSameColor(s1D2, s2D2, s3D2, s4D2);
                    if (squaresSameColorD2)
                        return true;
                }
            }
            return false;
        }

        private bool CheckSquaresSameColor(Square s1, Square s2, Square s3, Square s4)
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
    }



    class MinMax
    {
        int Strength = 30000;
        Random rnd = new Random();

        public void SetStrength(int str)
        {
            Strength = str;
        }

        public float FindDeepValue(Board GameBoard, int col)
        {
            int value = 0;

            for (int i = 0; i < Strength; i++)
            {
                Board newBoard = GameBoard.Copy();
                newBoard.MakeMove(col);

                WinState? winner = CheckNextMoves(newBoard);

                // if the AI would win, increase the value of the move
                // if ai would lose, or tie, decrese the value
                //if (winner = WinState.BLACKWIN && Game1.AIColor == BoardState.BLACK)
                if (winner == WinState.BLACKWIN && GameBoard.CurrentPlayer == TypeToken.TOKEN_BLACK)
                    value++;
                //else if (winner = WinState.REDWIN && Game1.AIColor == BoardState.RED)
                else if (winner == WinState.REDWIN && GameBoard.CurrentPlayer == TypeToken.TOKEN_RED)
                    value++;
                else if (winner == WinState.TIE)
                    value = 0;
                else
                    value--;
            }

            // make the return value either -1 or 1
            return (value / (float)Strength);
        }

        /* Checks all the next possible moves given a Board. */
        WinState? CheckNextMoves(Board GameBoard)
        {
            // will be randomized from all possible moves
            int nextMove;

            // play a single move, then keep playing checking
            // every move to find the best ones
            while (!GameBoard.CheckForWinner())
            {
                List<int> possibleMoves = GameBoard.GetPossibleMoves();

                // choose a random move from the possible moves
                nextMove = rnd.Next(0, possibleMoves.Count);

                GameBoard.MakeMove(possibleMoves[nextMove]);
            }

            // return who would win the game
            return GameBoard.Winner;
        }
    }
}
