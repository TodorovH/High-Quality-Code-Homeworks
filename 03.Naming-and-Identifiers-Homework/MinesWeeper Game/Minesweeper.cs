namespace MinesweeperGame
{
    using System;
    using System.Collections.Generic;

    public class Minesweeper
    {
        public class Ranking
        {
            private string name;

            private int points;

            public string Player
            {
                get
                {
                    return this.name;
                }

                set
                {
                    this.name = value;
                }
            }

            public int Points
            {
                get
                {
                    return this.points;
                }

                set
                {
                    this.points = value;
                }
            }

            public Ranking()
            {
            }

            public Ranking(string name, int points)
            {
                this.Player = name;
                this.Points = points;
            }
        }

        private static void Main()
        {
            string command = string.Empty;
            char[,] gameField = CreateGameField();
            char[,] mines = SetMines();
            int counter = 0;
            bool explosion = false;
            List<Ranking> champions = new List<Ranking>(6);
            int row = 0;
            int colomn = 0;
            bool redFlag = true;
            const int maxNumberOfMines = 35;
            bool whiteFlag = false;

            do
            {
                if (redFlag)
                {
                    Console.WriteLine(
                        "Hajde da igraem na “Mini4KI”. Probvaj si kasmeta da otkriesh poleteta bez mini4ki."
                        + " Komanda 'top' pokazva klasiraneto, 'restart' po4va nova igra, 'exit' izliza i hajde 4ao!");
                    Dumpp(gameField);
                    redFlag = false;
                }

                Console.Write("Daj red i kolona : ");
                command = Console.ReadLine().Trim();
                if (command.Length >= 3)
                {
                    if (int.TryParse(command[0].ToString(), out row) && int.TryParse(command[2].ToString(), out colomn)
                        && row <= gameField.GetLength(0) && colomn <= gameField.GetLength(1))
                    {
                        command = "turn";
                    }
                }

                switch (command)
                {
                    case "top":
                        GetRanking(champions);
                        break;
                    case "restart":
                        gameField = CreateGameField();
                        mines = SetMines();
                        Dumpp(gameField);
                        explosion = false;
                        redFlag = false;
                        break;
                    case "exit":
                        Console.WriteLine("4a0, 4a0, 4a0!");
                        break;
                    case "turn":
                        if (mines[row, colomn] != '*')
                        {
                            if (mines[row, colomn] == '-')
                            {
                                ChangeTurn(gameField, mines, row, colomn);
                                counter++;
                            }

                            if (maxNumberOfMines == counter)
                            {
                                whiteFlag = true;
                            }
                            else
                            {
                                Dumpp(gameField);
                            }
                        }
                        else
                        {
                            explosion = true;
                        }

                        break;
                    default:
                        Console.WriteLine("\nGreshka! nevalidna Komanda\n");
                        break;
                }

                if (explosion)
                {
                    Dumpp(mines);
                    Console.Write("\nHrrrrrr! Umria gerojski s {0} to4ki. " + "Daj si niknejm: ", counter);
                    string nickName = Console.ReadLine();
                    Ranking t = new Ranking(nickName, counter);
                    if (champions.Count < 5)
                    {
                        champions.Add(t);
                    }
                    else
                    {
                        for (int i = 0; i < champions.Count; i++)
                        {
                            if (champions[i].Points < t.Points)
                            {
                                champions.Insert(i, t);
                                champions.RemoveAt(champions.Count - 1);
                                break;
                            }
                        }
                    }

                    champions.Sort((Ranking r1, Ranking r2) => r2.Player.CompareTo(r1.Player));
                    champions.Sort((Ranking r1, Ranking r2) => r2.Points.CompareTo(r1.Points));
                    GetRanking(champions);

                    gameField = CreateGameField();
                    mines = SetMines();
                    counter = 0;
                    explosion = false;
                    redFlag = true;
                }

                if (whiteFlag)
                {
                    Console.WriteLine("\nBRAVOOOS! Otvori 35 kletki bez kapka kryv.");
                    Dumpp(mines);
                    Console.WriteLine("Daj si imeto, batka: ");
                    string nameOfNewChampion = Console.ReadLine();
                    Ranking pointsOfNewChampion = new Ranking(nameOfNewChampion, counter);
                    champions.Add(pointsOfNewChampion);
                    GetRanking(champions);
                    gameField = CreateGameField();
                    mines = SetMines();
                    counter = 0;
                    whiteFlag = false;
                    redFlag = true;
                }
            }
            while (command != "exit");
            Console.WriteLine("Made in Bulgaria - Uauahahahahaha!");
            Console.WriteLine("AREEEEEEeeeeeee.");
            Console.Read();
        }

        private static void GetRanking(List<Ranking> pointsOfNewChampion)
        {
            Console.WriteLine("\nTo4KI:");
            if (pointsOfNewChampion.Count > 0)
            {
                for (int i = 0; i < pointsOfNewChampion.Count; i++)
                {
                    Console.WriteLine("{0}. {1} --> {2} kutii", i + 1, pointsOfNewChampion[i].Player, pointsOfNewChampion[i].Points);
                }

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("prazna klasaciq!\n");
            }
        }

        private static void ChangeTurn(char[,] gameField, char[,] mines, int row, int colomn)
        {
            char numnerOfMines = CountMines(mines, row, colomn);
            mines[row, colomn] = numnerOfMines;
            gameField[row, colomn] = numnerOfMines;
        }

        private static void Dumpp(char[,] board)
        {
            int boardRows = board.GetLength(0);
            int boardColomns = board.GetLength(1);
            Console.WriteLine("\n    0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine("   ---------------------");
            for (int i = 0; i < boardRows; i++)
            {
                Console.Write("{0} | ", i);
                for (int j = 0; j < boardColomns; j++)
                {
                    Console.Write(string.Format("{0} ", board[i, j]));
                }

                Console.Write("|");
                Console.WriteLine();
            }

            Console.WriteLine("   ---------------------\n");
        }

        private static char[,] CreateGameField()
        {
            int boardRows = 5;
            int boardColumns = 10;
            char[,] board = new char[boardRows, boardColumns];
            for (int i = 0; i < boardRows; i++)
            {
                for (int j = 0; j < boardColumns; j++)
                {
                    board[i, j] = '?';
                }
            }

            return board;
        }

        private static char[,] SetMines()
        {
            int rows = 5;
            int colomns = 10;
            char[,] gameField = new char[rows, colomns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < colomns; j++)
                {
                    gameField[i, j] = '-';
                }
            }

            List<int> mines = new List<int>();
            while (mines.Count < 15)
            {
                Random random = new Random();
                int nextMine = random.Next(50);
                if (!mines.Contains(nextMine))
                {
                    mines.Add(nextMine);
                }
            }

            foreach (int mine in mines)
            {
                int kol = mine / colomns;
                int red = mine % colomns;
                if (red == 0 && mine != 0)
                {
                    kol--;
                    red = colomns;
                }
                else
                {
                    red++;
                }

                gameField[kol, red - 1] = '*';
            }

            return gameField;
        }

        private static void Calculate(char[,] field)
        {
            int colomn = field.GetLength(0);
            int row = field.GetLength(1);

            for (int i = 0; i < colomn; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    if (field[i, j] != '*')
                    {
                        char count = CountMines(field, i, j);
                        field[i, j] = count;
                    }
                }
            }
        }

        private static char CountMines(char[,] field, int currentRow, int currentColomn)
        {
            int minesCounter = 0;
            int rows = field.GetLength(0);
            int colomns = field.GetLength(1);

            if (currentRow - 1 >= 0)
            {
                if (field[currentRow - 1, currentColomn] == '*')
                {
                    minesCounter++;
                }
            }

            if (currentRow + 1 < rows)
            {
                if (field[currentRow + 1, currentColomn] == '*')
                {
                    minesCounter++;
                }
            }

            if (currentColomn - 1 >= 0)
            {
                if (field[currentRow, currentColomn - 1] == '*')
                {
                    minesCounter++;
                }
            }

            if (currentColomn + 1 < colomns)
            {
                if (field[currentRow, currentColomn + 1] == '*')
                {
                    minesCounter++;
                }
            }

            if ((currentRow - 1 >= 0) && (currentColomn - 1 >= 0))
            {
                if (field[currentRow - 1, currentColomn - 1] == '*')
                {
                    minesCounter++;
                }
            }

            if ((currentRow - 1 >= 0) && (currentColomn + 1 < colomns))
            {
                if (field[currentRow - 1, currentColomn + 1] == '*')
                {
                    minesCounter++;
                }
            }

            if ((currentRow + 1 < rows) && (currentColomn - 1 >= 0))
            {
                if (field[currentRow + 1, currentColomn - 1] == '*')
                {
                    minesCounter++;
                }
            }

            if ((currentRow + 1 < rows) && (currentColomn + 1 < colomns))
            {
                if (field[currentRow + 1, currentColomn + 1] == '*')
                {
                    minesCounter++;
                }
            }

            return char.Parse(minesCounter.ToString());
        }
    }
}