using System;
using System.Text;
using System.Threading;

// In diesem Code wird das “Game of Life” simuliert, ein zellulärer Automat,
// der von dem britischen Mathematiker John Horton Conway im Jahr 1970 erfunden wurde.
// Das Spiel wird auf einem zweidimensionalen Gitter von Zellen gespielt,
// wobei jede Zelle entweder lebendig oder tot ist. Jede Zelle interagiert mit ihren acht Nachbarn,
// die horizontal, vertikal oder diagonal angrenzend sind.
// Die Regeln des Spiels sind in der CalculateNextGeneration-Methode implementiert. 



namespace GameOfLife
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Setze die Ausgabecodierung der Konsole auf UTF8, um Unicode-Zeichen korrekt darzustellen
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Erstelle ein Spielfeld (ein zweidimensionales Array von bools) mit der Größe des Konsolenfensters
            bool[,] gameBoard = new bool[Console.WindowHeight - 1, Console.WindowWidth];

            // Lösche den Inhalt der Konsole und mache den Cursor unsichtbar
            Console.Clear();
            Console.CursorVisible = false;

            // Initialisiere das Spielfeld mit zufälligen Werten
            InitializeRandomBoard(gameBoard);

            // Drucke das Spielfeld auf die Konsole
            PrintBoard(1, 0, gameBoard);

            // Führe das Spiel aus, bis eine Taste gedrückt wird
            do
            {
                // Berechne die nächste Generation des Spielfelds
                gameBoard = CalculateNextGeneration(gameBoard);

                // Drucke das aktualisierte Spielfeld auf die Konsole
                PrintBoard(1, 0, gameBoard);

                // Warte eine kurze Zeit, bevor die nächste Generation berechnet wird
                Thread.Sleep(250);
            } while (!Console.KeyAvailable);

            // Mache den Cursor wieder sichtbar, wenn das Spiel beendet ist
            Console.CursorVisible = true;
        }

        // Diese Methode berechnet die nächste Generation des Spielfelds basierend auf den Regeln des Spiels
        static bool[,] CalculateNextGeneration(bool[,] currentBoard)
        {
            // Dieses Array enthält die Koordinaten der acht Nachbarn jeder Zelle
            (int, int)[] neighborsDelta = new (int, int)[]
            {
                (-1,-1), ( 0,-1), ( 1,-1), (-1, 0),
                ( 1, 0), (-1, 1), ( 0, 1), ( 1, 1)
            };

            // Erstelle ein neues Spielfeld für die nächste Generation
            int rows = currentBoard.GetLength(0);
            int cols = currentBoard.GetLength(1);
            bool[,] newBoard = new bool[rows, cols];

            // Durchlaufe jede Zelle im aktuellen Spielfeld
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    // Zähle die Anzahl der lebenden Nachbarn der aktuellen Zelle
                    int liveNeighbors = 0;
                    for (int i = 0; i < neighborsDelta.Length; i++)
                    {
                        int x1 = x + neighborsDelta[i].Item1;
                        int y1 = y + neighborsDelta[i].Item2;
                        if (x1 >= 0 && y1 >= 0 && x1 < rows && y1 < cols)
                        {
                            liveNeighbors += currentBoard[x1, y1] ? 1 : 0;
                        }
                    }

                //  Wende die Regeln des Spiels an, um zu bestimmen, ob die Zelle in der nächsten Generation lebt oder stirbt

                //  Neue Zelle     = falls die alte Zelle | lebt                                     | nicht lebt
                //                                        | 2 oder 3 lebende Nachbarn                | 3 lebende Nachbarn
                    newBoard[x, y] = currentBoard[x, y]   ? liveNeighbors == 2 || liveNeighbors == 3 : liveNeighbors == 3;
                }
            }

            // Gebe das neue Spielfeld zurück
            return newBoard;
        }

        // Diese Methode initialisiert das Spielfeld mit zufälligen Werten
        static void InitializeRandomBoard(bool[,] board)
        {
            // Erstelle ein neues Random-Objekt
            Random rand = new Random();

            // Durchlaufe jede Zelle im Spielfeld und setze sie auf einen zufälligen Wert
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    board[x, y] = (rand.Next(2) == 0);
                }
            }
        }

        // Diese Methode druckt das Spielfeld auf die Konsole
        static void PrintBoard(int xPos, int yPos, bool[,] board)
        {
            // Erstelle einen neuen StringBuilder für die String-Erzeugung
            StringBuilder sb = new StringBuilder();

            // Durchlaufe jede Zelle im Spielfeld
            for (int x = 0; x < board.GetLength(0); x++)
            {
                // Lösche den Inhalt des StringBuilders
                sb.Clear();

                for (int y = 0; y < board.GetLength(1); y++)
                {
                    // Füge das entsprechende Zeichen für die aktuelle Zelle zum StringBuilder hinzu
                    sb.Append(board[x, y] ? "\u2588" : " ");
                }

                // Setze die Position des Cursors und drucke die Zeile auf die Konsole
                if (yPos + x < Console.WindowHeight) Console.SetCursorPosition(xPos, yPos + x);
                Console.Write(sb.ToString());
            }
        }
    }
}