using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.Models.Board
{
    public class Ship
    {
        public int shipId { get; set; }
        public bool playerShip { get; set; }
        public int shipNumber { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int gameId { get; set; }

    }
}
