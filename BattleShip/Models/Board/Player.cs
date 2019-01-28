using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.Models.Board
{
    public class Player
    {

        public Player()
        {
        }
        
        public int playerId { get; set; }

        public string playerName { get; set; }

        public List<Game> games { get; set; }

    }
}
