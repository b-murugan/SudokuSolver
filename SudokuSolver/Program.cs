
namespace SudokuSolverSpace
{
    public struct Element
    {
        public ushort Value;
        public bool IsOriginal;

        public Element()
        {
            Value = 0;
            IsOriginal = false;
        }
    };


    public class Program
    {
        public static bool CheckSubBox(Element[,] Grid, int row, int col, ushort value)
        {
            int BoxRowStart = (row/3)*3;
            int BoxColStart = (col/3)*3;

            for (int subrow = BoxRowStart; subrow < BoxRowStart + 3; subrow++)
            {
                for (int subcol = BoxColStart; subcol < BoxColStart + 3; subcol++)
                {
                    Element element = Grid[subrow, subcol];
                    if (element.Value == value)
                        return false;
                }
            }
            return true;
        }
        public static bool CheckCol(Element[,] Grid, int col, ushort value)
        {
            for (int Idx = 0; Idx < GRID_SIZE; Idx++)
            {
                Element element = Grid[Idx,col];

                if (element.Value == value)
                    return false;
            }

            return true;
        }
        public static bool CheckRow(Element[,] Grid, int row, ushort value)
        {
            for (int Idx = 0; Idx < GRID_SIZE; Idx++)
            {
                Element element = Grid[row, Idx];

                if (element.Value == value)
                    return false;                
            }

            return true;
        }
  

        public static readonly ushort GRID_SIZE = 9;

        private static bool IsFilledFull(Element[,] Grid)
        {
            bool isFilled = true;
            int ElementsToFill = 0;
            for (int row = 0; row < GRID_SIZE; row++)
            {                
                for (int col = 0; col < GRID_SIZE; col++)
                {
                    if (Grid[row, col].Value == 0)
                    {
                        isFilled = false;
                        ElementsToFill++;
                    }
                }
            }
            //Console.WriteLine("ElementsToFill = "+ElementsToFill);
            return isFilled;
        }
     
        private static void PrintCurrentGrid(Element[,] Grid)
        {
            Console.WriteLine("|---------+---------+---------|");

            for (int row = 0; row < GRID_SIZE; row++)
            {
                Console.Write("|");
                for (int col = 0; col < GRID_SIZE; col++)
                {
                    Console.Write(" ");
                    Console.Write((Grid[row, col].Value!=0)? Grid[row, col].Value:"-");
                    Console.Write(" ");

                    if ((col + 1) % 3 == 0)
                    {
                        Console.Write("|");
                    }
                }
                Console.WriteLine();
                if ((row + 1) % 3 == 0)
                {
                    Console.WriteLine("|---------+---------+---------|");
                }
            }
            Console.WriteLine();
        }
        private static void FillElements(ref Element element, ushort Value)
        {
            element.Value = Value;
            element.IsOriginal = true;
        }

#if false  // Problem 1
        private static void FillFixed(ref Element[,] Grid)
        {
            FillElements(ref Grid[0, 0], 6);
            FillElements(ref Grid[0, 1], 9);
            FillElements(ref Grid[0, 2], 1);

            FillElements(ref Grid[1, 0], 8);
            FillElements(ref Grid[1, 3], 7);
            FillElements(ref Grid[1, 5], 2);
            FillElements(ref Grid[1, 8], 4);

            FillElements(ref Grid[2, 5], 3);
            FillElements(ref Grid[2, 6], 5);
            FillElements(ref Grid[2, 7], 8);

            FillElements(ref Grid[3, 2], 4);
            FillElements(ref Grid[3, 3], 2);
            FillElements(ref Grid[3, 6], 3);

            FillElements(ref Grid[4, 4], 6);
            FillElements(ref Grid[4, 5], 8);
            FillElements(ref Grid[4, 6], 9);
            FillElements(ref Grid[4, 8], 1);

            FillElements(ref Grid[5, 2], 9);
            FillElements(ref Grid[5, 4], 4);
            FillElements(ref Grid[5, 8], 7);

            FillElements(ref Grid[6, 1], 1);

            FillElements(ref Grid[7, 0], 5);
            FillElements(ref Grid[7, 2], 3);
            FillElements(ref Grid[7, 3], 9);
            FillElements(ref Grid[7, 6], 4);
            FillElements(ref Grid[7, 8], 6);

            FillElements(ref Grid[8, 0], 7);
            FillElements(ref Grid[8, 3], 5);
            FillElements(ref Grid[8, 5], 1);
            FillElements(ref Grid[8, 8], 2);

        }

#endif
#if true  // problem 2
        private static void FillFixed(ref Element[,] Grid)
        {
            FillElements(ref Grid[0, 3], 4);
            FillElements(ref Grid[0, 5], 5);

            FillElements(ref Grid[1, 0], 3);
            FillElements(ref Grid[1, 6], 2);

            FillElements(ref Grid[2, 5], 8);
            FillElements(ref Grid[2, 6], 9);

            FillElements(ref Grid[3, 0], 8);
            FillElements(ref Grid[3, 1], 4);
            FillElements(ref Grid[3, 3], 1);
            FillElements(ref Grid[3, 7], 3);
            FillElements(ref Grid[3, 8], 2);

            FillElements(ref Grid[4, 1], 6);
            FillElements(ref Grid[4, 6], 8);

            FillElements(ref Grid[5, 1], 5);
            FillElements(ref Grid[5, 4], 8);
            FillElements(ref Grid[5, 5], 3);

            FillElements(ref Grid[6, 0], 4);
            FillElements(ref Grid[6, 1], 7);
            FillElements(ref Grid[6, 7], 6);

            FillElements(ref Grid[7, 5], 1);

            FillElements(ref Grid[8, 3], 7);
            FillElements(ref Grid[8, 4], 2);

        } 
#endif

        public static bool IsViolating(Element[,] Grid, int row, int col, ushort value)
        {
            return (!(CheckRow(Grid, row, value) && CheckCol(Grid, col, value) && CheckSubBox(Grid, row, col, value)));
        }

        public static void Main(string[] args)
        {
            Element[,] Grid = new Element[GRID_SIZE, GRID_SIZE];

            Grid.Initialize();

            FillFixed(ref Grid);

            PrintCurrentGrid(Grid);

            ushort CurrValue = 1;

            bool IsBacktracking = false;
            bool IsSolved = false;

            int CurRow = 0;
            int CurCol = -1;

            int IterationCount = 0;

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            stopwatch.Start();

            do
            {
                IterationCount++;

                if (IsBacktracking) // We are going back to test new values
                {
                    if (CurCol == 0) // At grid edge, rollover to next row
                    {
                        CurRow--;
                        CurCol = GRID_SIZE - 1;
                    }
                    else
                    {
                        CurCol--;

                        if (CurCol < 0) // Means we have backtracked all the way to first cell, and still no valid value found
                            throw new IndexOutOfRangeException();
                    }
                }
                else
                {
                    if (CurCol + 1 == GRID_SIZE) // At grid edge, rollover to next row
                    {
                        CurRow++;
                        CurCol = 0;

                        if (CurRow == GRID_SIZE) // We are at the last cell, we are done filling, Exit loop.
                            break;
                    }
                    else
                    {
                        CurCol++;

                        if (CurCol == GRID_SIZE)
                            throw new IndexOutOfRangeException();
                    }
                }

                if (Grid[CurRow, CurCol].IsOriginal == true) // The value is not filled by program, skip this cell
                {
                    continue;
                }


                if (IsBacktracking)
                {
                    IsBacktracking = false;
                    CurrValue = (ushort)(Grid[CurRow, CurCol].Value + 1); // Check next value in previous cell

                    if (CurrValue > GRID_SIZE) // Tested all possible values in this cell, go back again
                    {
                        Grid[CurRow, CurCol].Value = 0;
                        IsBacktracking = true;
                        continue;
                    }
                }

                bool CellValFound = false;

                do
                {
                    if (IsViolating(Grid, CurRow, CurCol, CurrValue)) // Cant fill current value as it is repeating in row, column or inner box
                    {
                        CurrValue++;

                        if (CurrValue > GRID_SIZE) // Tested all possible values for this cell, go back to previous
                        {
                            Grid[CurRow, CurCol].Value = 0;
                            IsBacktracking = true;
                            break;
                        }
                        continue;
                    }
                    else
                    {
                        Grid[CurRow, CurCol].Value = CurrValue;
                        CellValFound = true;
                        CurrValue = 1; // Start from 1 for next cell
                    }
                } while (!CellValFound);

                IsSolved = IsFilledFull(Grid);

            } while (!IsSolved);

            stopwatch.Stop();

            Console.WriteLine("Solution:");

            PrintCurrentGrid(Grid);

            Console.WriteLine("Total Iteration taken: " + IterationCount);
            Console.WriteLine("Time spent calculating: " + stopwatch.ElapsedMilliseconds + "ms");
            Console.ReadKey();
        }
    }
}