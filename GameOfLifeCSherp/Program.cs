using System;


public class GameOfLife
{
    private bool[,] BoardState;    //before going any further lets save some headache now and add a border to the boardstate so that we can impliment our rules check easier
    private bool[,] FutureBoard; //a copy of the BoardState that we will be manipulating in the NextStep method to save our results without changing the current boardstate we're working on
                                 //now that dynamic arrays have been tested, work can start on constructors and building and populating a board
    public GameOfLife()
    { //the default constuctor will build a 5 x 5 grid with no live cells
        BoardState = new bool[7, 7];
        FutureBoard = new bool[7, 7];
    }
    public GameOfLife(int width, int length)
    { //a very boring consturctor I decided to add at the last minute thats the same as the randomiser but without the option for randomisation
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
    {//constructor that will accept a character array as the starting board state, similar to the requested output
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
    { //just need to change this over to return the trimmed boardstate TODO: Update all the tests
        char[,] ReturnValue = new char[BoardState.GetLength(0) - 2, BoardState.GetLength(1) - 2]; //same size as the actual board minus the trimmings
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

    public void SetBoardState(char[,] InputBoard)
    {
        BoardState = new bool[InputBoard.GetLength(0) + 2, InputBoard.GetLength(1) + 2];
        FutureBoard = new bool[InputBoard.GetLength(0) + 2, InputBoard.GetLength(1) + 2];
        for (int i = 1; i < InputBoard.GetLength(0) + 1; i++)
        { //literally just a return of the char constuctor above for setting boardstate
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
                    { // so we've got nested loops in loops that are refrencing the loops that they're in because we need the location of the current cell to be able to loop through its neighbours (yo dog I heard you like loops....)
                      //TODO fix this so it doesnt count the cell its in, otherwise its not going to work properly (dead cells are the same but alive cells would count themselves and thats not how the game works)
                        if (k != i | l != j) { neighbours += Convert.ToInt32(BoardState[k, l]); }
                    }
                }
                //      Console.Write(neighbours); //testing code to be removed
                switch (neighbours)
                { //implementing ruleset for game of life, 2 to keep alive and 3 to become alive, everything else is death so that will be the default
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
