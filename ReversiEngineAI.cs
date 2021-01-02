using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    public class ReversiEngineAI : ReversiEngine
    {
        public ReversiEngineAI(int playerStartNumber, int fieldWidth = 8, int fieldHeight = 8)
            : base (playerStartNumber, fieldWidth, fieldHeight)
        {
        }

        private struct MoveIsPossible : IComparable<MoveIsPossible>
        {
            public int horiz;
            public int vert;
            public int priority;

            public MoveIsPossible(int horiz, int vert, int priority)
            {
                this.horiz = horiz;
                this.vert = vert;
                this.priority = priority;
            }

            public int CompareTo(MoveIsPossible other)
            {
                return other.priority - this.priority;
            }
        }

        public void ProposeBestMove(out int bestHorizMove, out int bestVertMove)
        {
            List<MoveIsPossible> possibleMovies = new List<MoveIsPossible>();
            int priorityStep = FieldWidth * FieldHeight;

            for (var horiz = 0; horiz < FieldWidth; horiz++)
            {
                for (var vert = 0; vert < FieldHeight; vert++)
                {
                    if (GetFieldState(horiz, vert) == 0)
                    {
                        int priority = PutStone(horiz, vert, true);
                        if(priority > 0)
                        {
                            MoveIsPossible nr = new MoveIsPossible(horiz, vert, priority);

                            // corner
                            if ((nr.horiz == 0 || nr.horiz == FieldWidth - 1) &&
                                (nr.vert == 0 || nr.vert == FieldHeight - 1))
                                nr.priority += priorityStep * priorityStep;

                            // corner neighbor
                            if ((nr.horiz == 1 || nr.horiz == FieldWidth - 2) &&
                                (nr.vert == 1 || nr.vert == FieldHeight - 2))
                                nr.priority -= priorityStep * priorityStep;

                            // corner neighbor in vert
                            if ((nr.horiz == 0 || nr.horiz == FieldWidth - 1) &&
                                (nr.vert == 1 || nr.vert == FieldHeight - 2))
                                nr.priority -= priorityStep * priorityStep;

                            // corner neighbor in horiz
                            if ((nr.horiz == 1 || nr.horiz == FieldWidth - 2) &&
                                (nr.vert == 0 || nr.vert == FieldHeight - 1))
                                nr.priority -= priorityStep * priorityStep;

                            // edge
                            if ((nr.horiz == 0 || nr.horiz == FieldWidth - 1) ||
                                (nr.vert == 0 || nr.vert == FieldHeight - 1))
                                nr.priority += priorityStep;

                            // middle
                            if ((nr.horiz == 1 || nr.horiz == FieldWidth - 2) ||
                                (nr.vert == 1 || nr.vert == FieldHeight - 2))
                                nr.priority -= priorityStep;

                            possibleMovies.Add(nr);
                        }
                    }
                }
            }
            if (possibleMovies.Count > 0)
            {
                possibleMovies.Sort();
                bestHorizMove = possibleMovies[0].horiz;
                bestVertMove = possibleMovies[0].vert;
            }
            else
                throw new Exception("Brak mozliwych ruchów");

        }
   
    }
}
