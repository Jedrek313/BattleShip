using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.Models.Board
{
    [Serializable]
    public class Position
    {
        public int shipNumber { get; set; }
        public int key { get; set; }
        public int value { get; set; }

        public Position(int shipNumber, int key, int value)
        {
            this.shipNumber = shipNumber;
            this.key = key;
            this.value = value;
        }

    }

}
