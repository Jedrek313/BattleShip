using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BattleShip.Models.Board;

namespace BattleShip.Models
{
    public class BattleShipContext : DbContext
    {

        public BattleShipContext (DbContextOptions<BattleShipContext> options)
            : base(options)
        {
        }

        public DbSet<BattleShip.Models.Board.Player> Player { get; set; }
        public DbSet<BattleShip.Models.Board.Game> Game { get; set; }
        public DbSet<BattleShip.Models.Board.Move> Move { get; set; }
        public DbSet<BattleShip.Models.Board.Ship> Ship { get; set; }
    }
}
