using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.Models.Board
{
    public class Move
    {
        public int moveId { get; set; }
        public bool playerMove { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int gameId { get; set; }
    }
}
