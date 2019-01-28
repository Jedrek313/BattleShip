using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShip.Models.Board;

namespace BattleShip.Models.Collision
{
    public class Collision
    {
        public static Boolean CheckShipsCollisions(List<Position> positions)
        {
            Boolean result = true;
            int[,] array = new int[10, 10];

            List<KeyValuePair<int, List<Position>>> pairs = new List<KeyValuePair<int, List<Position>>>();
            foreach (Position position in positions)
            {
                Boolean addNewPair = true;
                foreach (KeyValuePair<int, List<Position>> pair in pairs)
                {
                    if (pair.Key == position.shipNumber)
                    {
                        pair.Value.Add(position);
                        addNewPair = false;
                        break;
                    }
                }

                if (addNewPair)
                {
                    List<Position> list = new List<Position>();
                    list.Add(position);
                    pairs.Add(new KeyValuePair<int, List<Position>>(position.shipNumber, list));
                }

                if (array[position.key, position.value] == 0)
                {
                    array[position.key, position.value]++;
                }
                else
                {
                    return false;
                }
            }

            foreach (KeyValuePair<int, List<Position>> pair in pairs)
            {
                if (pair.Value.Count > 1)
                {
                    List<int> x = new List<int>();
                    List<int> y = new List<int>();
                    foreach (Position position in pair.Value)
                    {
                        x.Add(position.key);
                        y.Add(position.value);
                    }

                    Boolean xflag = x.All(a => a == x.First());
                    Boolean yflag = y.All(a => a == y.First());

                    if (xflag == true && yflag == false)
                    {
                        y.Sort();
                        int previousNumber = y[0];
                        for (int i = 1; i < y.Count; i++)
                        {
                            if (++previousNumber == y[i])
                            {

                            }
                            else
                            {
                                return false;
                            }
                        }
                    }else if (xflag == false && yflag == true)
                    {
                        x.Sort();
                        int previousNumber = x[0];
                        for (int i = 1; i < x.Count; i++)
                        {
                            if (++previousNumber == x[i])
                            {

                            }
                            else
                            {
                                return false;
                            }
                        }

                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return result;
        }

        public static bool CheckShotCollision(int x, int y, List<Move> moves)
        {
            Boolean result = true;
            
            foreach (Move move in moves)
            {

                if (move.y ==y && move.x ==x)
                {
                    return false;
                }
            }
            return result;
        }

        public static int[,] ResetArrayUsedMoves(int[,] array, List<Move> moves)
        {
            foreach (Move move in moves)
            {
                array[move.x, move.y] = -1;
            }

            return array;
        }

        public static Boolean CheckEnemyShipShooted(int x, int y, List<Ship> ships)
        {
            Boolean result = true;

            foreach (Ship ship in ships)

            {

                if (ship.y == y && ship.x == x)
                {
                    return true;
                }
            }


            return false;
        }
    }
}
