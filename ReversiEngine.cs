using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    public class ReversiEngine
    {
        public int FieldWidth { get; private set; }
        public int FieldHeight { get; private set; }
        public int NextMovePlayerNumber { get; private set; } = 1;

        private int[,] field;

        public ReversiEngine(int PlayerStartNumber, int fieldWidth = 8, int fieldHeight = 8)
        {
            if (PlayerStartNumber < 1 || PlayerStartNumber > 2)
                throw new Exception("Nieprawidłowy numer gracza rozpoczynajacego grę");

            FieldWidth = fieldWidth;
            FieldHeight = fieldHeight;
            field = new int[FieldWidth, FieldHeight];

            PrepareField();
            NextMovePlayerNumber = PlayerStartNumber;
        }

        private void PrepareField()
        {
            for (int i = 0; i < FieldWidth; i++)
            {
                for (int j = 0; j < FieldHeight; j++)
                    field[i, j] = 0;
            }

            var widthMiddle = FieldWidth / 2;
            var heightMiddle = FieldHeight / 2;
            field[widthMiddle - 1, heightMiddle - 1] = field[widthMiddle, heightMiddle] = 1;
            field[widthMiddle - 1, heightMiddle] = field[widthMiddle, heightMiddle - 1] = 2;
        }

        private static int OponentNumber(int playerNumber)
        {
            return (playerNumber == 1) ? 2 : 1;
        }

        private bool IsFieldCoordinatesCorrect(int horiz, int vert)
        {
            return horiz >= 0 && horiz < FieldWidth &&
                vert >= 0 && vert < FieldHeight;
        }

        public int GetFieldState(int horiz, int vert)
        {
            if (!IsFieldCoordinatesCorrect(horiz, vert))
                throw new Exception("Nieprawidłowe współrzędne pola");
            return field[horiz, vert];
        }
    }
}
