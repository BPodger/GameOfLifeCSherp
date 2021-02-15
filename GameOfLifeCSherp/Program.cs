using System;


public class GameOfLife
{
    private bool[,] BoardState;  //the base boardstate that will represent live cells with true values and dead cells with false values
    private bool[,] FutureBoard; //a copy of the BoardState that we will be manipulating in the NextStep method to save our results without changing the current boardstate we're working on
                                 //now that dynamic arrays have been tested, work can start on constructors and building and populating a board
    public GameOfLife()
    { //the default constuctor will build a 5 x 5 grid with no live cells
        BoardState = new bool[7, 7];
        FutureBoard = new bool[7, 7];
    }
    public GameOfLife(int width, int length)
    { //a very boring basic consturctor that makes a dead board with a play size as indicated
        BoardState = new bool[width + 2, length + 2];
        FutureBoard = new bool[width + 2, length + 2];
    }

    public GameOfLife(bool[,] BoardInput)
    {//constructor that will accept a boolean array as the starting board state
        BoardState = new bool[BoardInput.GetLength(0) + 2, BoardInput.GetLength(1) + 2];
        FutureBoard = new bool[BoardInput.GetLength(0) + 2, BoardInput.GetLength(1) + 2];
        for (int i = 1; i < BoardInput.GetLength(0) + 1; i++)
        {
            for (int j = 1; j < BoardInput.GetLength(1) + 1; j++)
            {
                BoardState[i, j] = BoardInput[i - 1, j - 1];
            }
        }
    }

    public GameOfLife(char[,] BoardInput)
    {//constructor that will accept a character array as the starting board state
        BoardState = new bool[BoardInput.GetLength(0) + 2, BoardInput.GetLength(1) + 2];
        FutureBoard = new bool[BoardInput.GetLength(0) + 2, BoardInput.GetLength(1) + 2];
        for (int i = 1; i < BoardInput.GetLength(0) + 1; i++)
        {
            for (int j = 1; j < BoardInput.GetLength(1) + 1; j++)
            {
                if (BoardInput[i - 1, j - 1] == '*')
                { //the board will accept *'s as true and -'s or anything else as false
                    BoardState[i, j] = true;
                }
                else
                {
                    BoardState[i, j] = false;
                }
            }
        }
    }

    public GameOfLife(int columns, int rows, bool randomise)
    {//constructor that will accept two ints and a single bool, generating either an empty(false) boardstate or a randomly generated boardstate depending on the state of the bool
        columns += 2;
        rows += 2;
        BoardState = new bool[columns, rows];
        FutureBoard = new bool[columns, rows];
        if (randomise)
        {
            Random rng = new Random();
            for (int i = 1; i < columns - 1; i++)
            { //too much off the top here, just needed to reduce the trimming
                for (int j = 1; j < rows - 1; j++)
                {
                    BoardState[i, j] = Convert.ToBoolean(rng.Next(0, 2));
                }
            }
        }
    }

    public char[,] GetBoardState()
    { //returns the boardstate as a character array with * for live cells and - for dead cells
        char[,] ReturnValue = new char[BoardState.GetLength(0) - 2, BoardState.GetLength(1) - 2]; //same size as the actual board minus the border
        for (int i = 0; i < ReturnValue.GetLength(0); i++)
        {
            for (int j = 0; j < ReturnValue.GetLength(1); j++)
            {
                if (BoardState[i + 1, j + 1] == true)
                {
                    ReturnValue[i, j] = '*';
                }
                else
                {
                    ReturnValue[i, j] = '-';
                }
            }
        }
        return ReturnValue;
    }

    public void SetBoardState(char[,] InputBoard) //allows the game board to be set to a different value using an existing board
    {
        BoardState = new bool[InputBoard.GetLength(0) + 2, InputBoard.GetLength(1) + 2];
        FutureBoard = new bool[InputBoard.GetLength(0) + 2, InputBoard.GetLength(1) + 2];
        for (int i = 1; i < InputBoard.GetLength(0) + 1; i++)
        { 
            for (int j = 1; j < InputBoard.GetLength(1) + 1; j++)
            {
                if (InputBoard[i - 1, j - 1] == '*')
                {
                    BoardState[i, j] = true;
                }
                else
                {
                    BoardState[i, j] = false;
                }
            }
        }
    }
    public void NextStep()
    { //the actual method that when called will update the boardstate with a pass through the game of life rules
      /*loop through board state, check each neighbour and add up the number of alive neighboring cells, 
          then either keep the current cell the same, or change its state depending on the number of neighbours (case statement)
      */
        int neighbours = 0;
        for (int i = 1; i < BoardState.GetLength(0) - 1; i++)
        { //loop through the cells we care about, the boarder cells should only be visible to the check part of the algorithm
            for (int j = 1; j < BoardState.GetLength(1) - 1; j++)
            {
                for (int k = i - 1; k <= i + 1; k++)
                { //we're going though each cell in the grid(minus border) and checking each neighbour,
                    for (int l = j - 1; l <= j + 1; l++)
                    {
                       if (k != i | l != j) { neighbours += Convert.ToInt32(BoardState[k, l]); }
                    }
                }
                //      Console.Write(neighbours); //testing code to be removed
                switch (neighbours)
                { //implementing ruleset for game of life, 2 neighbours to stay alive and 3 to replicate, everything else is death (over/under population) so that will be the default
                    case 2:
                        if (BoardState[i, j] == true)
                        {
                            FutureBoard[i, j] = true;
                        }
                        else
                        {
                            FutureBoard[i, j] = false;
                        }
                        break;
                    case 3:
                        FutureBoard[i, j] = true;
                        break;
                    default:
                        FutureBoard[i, j] = false;
                        break;
                }
                neighbours = 0; //got to remember to reset it after each cell
            }
            //     Console.WriteLine(); //formatting for the neighbours count
        }

        BoardState = FutureBoard.Clone() as bool[,];
    }







    /*        test code from earlier
            public static int[,] Test(int input, int input2){
            int test = input;
            int test2 = input2;
            BoardState = new int[test,test2];        
            return BoardState;

            }*/


}
