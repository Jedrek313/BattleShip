using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.Models.Board
{
    public class EnemyShotResult
    {
        public int x { get; set; }
        public int y { get; set; }
        public Boolean success { get; set; }
        public Boolean hit { get; set; }
    }
}
