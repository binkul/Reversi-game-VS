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
        public int CountOfEmptyField { get { return takenField[0]; } }
        public int CountOfPlayerOneField { get { return takenField[1]; } }
        public int CountOfPlayerTwoField { get { return takenField[2]; } }

        private int[,] field;
        private int[] takenField = new int[3];

        public ReversiEngine(int PlayerStartNumber, int fieldWidth = 8, int fieldHeight = 8)
        {
            if (PlayerStartNumber < 1 || PlayerStartNumber > 2)
                throw new Exception("Nieprawidłowy numer gracza rozpoczynajacego grę");

            FieldWidth = fieldWidth;
            FieldHeight = fieldHeight;
            field = new int[FieldWidth, FieldHeight];

            PrepareField();
            CountTakenField();
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

        private void CountTakenField()
        {
            for (int i = 0; i < takenField.Length; i++) takenField[i] = 0;

            for (int i = 0; i < FieldWidth; i++)
                for (int j = 0; j < FieldHeight; j++)
                    takenField[field[i, j]]++;
        }

        private void ChangeActualPlayer()
        {
            NextMovePlayerNumber = OponentNumber(NextMovePlayerNumber);
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

        public bool PutStone(int horiz, int vert)
        {
            return PutStone(horiz, vert, false) > 0;
        }

        protected int PutStone(int horiz, int vert, bool test)
        {
            if (IsFieldCoordinatesCorrect(horiz, vert))
                throw new Exception("Nieprawidłowe współrzędne pola");

            if (field[horiz, vert] != 0) return -1;

            int ReversedFieldCount = 0;
            for (int horizDirection = -1; horizDirection <= 1; horizDirection++)
            {
                for (int vertDirection = -1; vertDirection <= 1; vertDirection++)
                {
                    if (horizDirection == 0 && vertDirection == 0) continue;

                    int i = horiz;
                    int j = vert;
                    bool enemyStoneFound = false;
                    bool nextMovePlayerStoneFound = false;
                    bool emptyFieldFound = false;
                    bool edgeFieldFound = false;
                    do
                    {
                        i += horizDirection;
                        j += vertDirection;
                        if (!IsFieldCoordinatesCorrect(i, j))
                            edgeFieldFound = true;
                        if (!edgeFieldFound)
                        {
                            if (field[i, j] == NextMovePlayerNumber)
                                nextMovePlayerStoneFound = true;
                            if (field[i, j] == 0) emptyFieldFound = true;
                            if (field[i, j] == OponentNumber(NextMovePlayerNumber))
                                enemyStoneFound = true;
                        }
                    }
                    while (!(edgeFieldFound || nextMovePlayerStoneFound || emptyFieldFound));

                    bool isPossibleTuPutStone = enemyStoneFound && nextMovePlayerStoneFound && !emptyFieldFound;

                    if (isPossibleTuPutStone)
                    {
                        int maxIndex = Math.Max(Math.Abs(i - horiz), Math.Abs(j - vert));
                        if (!test)
                        {
                            for (int index = 0; index < maxIndex; index++)
                            {
                                field[horiz + index * horizDirection, vert + index * vertDirection] = NextMovePlayerNumber;
                            }
                        }
                        ReversedFieldCount += maxIndex - 1;
                    }
                }
            }

            if (ReversedFieldCount > 0 && !test)
                ChangeActualPlayer();
            CountTakenField();

            return ReversedFieldCount;
        }
    }
}
