using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.Models.Board
{
    public class Game
    {

        public Game()
        {
            this.finished = false;
            ships = new List<Ship>();
            moves = new List<Move>();
        }

        public int gameId { get; set; }
        public bool finished { get; set; }
        public List<Ship> ships { get; set; }

        public List<Move> moves { get; set; }
        public int playerId { get; set; }



        public Boolean startGame()
        {

            return false;
        }

    }

}
