using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BattleShipContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BattleShipContext>>()))
            {
                // Look for any movies.
                if (context.Player.Any())
                {
                    return;   // DB has been seeded
                }
//
//                List<Board.Game> TempGames = new List<Board.Game>();
//                TempGames.Add(new Board.Game
//                {
//
//                });


                context.Player.AddRange(
                    new Board.Player {
                        playerName = "DefaulPlayer",
                        games = new List<Board.Game>
                        {
                            new Board.Game
                            {
                                finished = true,
                                ships = new List < Board.Ship >
                                {
                                    new Board.Ship
                                    {
                                        playerShip = true,
                                        shipNumber = 0,
                                        x = 2,
                                        y = 2
                                    },
                                    new Board.Ship
                                    {
                                        playerShip = true,
                                        x = 2,
                                        y = 3
                                    },
                                    new Board.Ship
                                    {
                                        playerShip = false,
                                        shipNumber = 0,
                                        x = 4,
                                        y = 4
                                    },
                                    new Board.Ship
                                    {
                                        playerShip = false,
                                        x = 4,
                                        y = 5
                                    }
                                },
                                moves = new List<Board.Move>
                                {
                                    new Board.Move
                                    {
                                        playerMove = true,
                                        x = 8,
                                        y = 8
                                    },
                                    new Board.Move
                                    {
                                        playerMove = false,
                                        x = 2,
                                        y = 2
                                    },
                                    new Board.Move
                                    {
                                        playerMove = true,
                                        x = 8,
                                        y = 7
                                    },
                                    new Board.Move
                                    {
                                        playerMove = false,
                                        x = 2,
                                        y = 4
                                    },
                                    new Board.Move
                                    {
                                        playerMove = true,
                                        x = 4,
                                        y = 5
                                    },
                                    new Board.Move
                                    {
                                        playerMove = false,
                                        x = 2,
                                        y = 3
                                    }
                                }
                            }
                        }
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
