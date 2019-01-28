using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BattleShip.Models;
using BattleShip.Models.Board;
using BattleShip.Models.Collision;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Utilities;
using Position = BattleShip.Models.Board.Position;

namespace BattleShip.Controllers
{


    public class GameController : Controller
    {

        private readonly BattleShipContext _context;
//        public List<SimpleShip> shipsSetting = new List<SimpleShip>();


        public GameController(BattleShipContext context)
        {
            _context = context;
//            SimpleShip simpleShip = new SimpleShip();
//            shipsSetting.Add();
//            shipsSetting.Add(2,3);
//            shipsSetting.Add(3, 3);
        }




        // GET: Game
        public ActionResult Index()
        {
            return View();
        }
        

        [HttpGet]
        public JsonResult GetGameSettings(int? id)
        {
            Game game = _context.Game.Find(id);
            List<Ship> ships = game.ships;
            List<Move> moves = game.moves;
            Boolean finished = game.finished;
            var shipsSetting = GetShipSettings();
            return Json(new
            {
                finished, ships, moves,
                shipsSetting

            });
        }


        // GET: Game/Details/5
        public ActionResult Details(int? id)
        {

            ViewBag.id = id;
            return View(id);
        }

        // POST: Game/SetShips/5
        [HttpPost]
        public IActionResult SetShips(int? id, [FromBody] List<Position> list)
        {
            Boolean result = Collision.CheckShipsCollisions(list);
            if (result)
            {
                Game game = _context.Game.Find(id);
                foreach(Position pos in list)
                {
                    Ship ship = new Ship();
                    ship.playerShip = true;
                    ship.shipNumber = pos.shipNumber;
                    ship.x = pos.key;
                    ship.y = pos.value;
                    game.ships.Add(ship);
                }
                _context.Game.Update(game);
                _context.SaveChanges();

                Dictionary<string, int> shipSettings = GetShipSettings();
                List<Position> enemyPositions = new List<Position>();
                int shipNumber = 0;
                foreach (KeyValuePair<string, int> entry in shipSettings)
                {
                    int value = entry.Value;
                    while (value > 0)
                    {
                        List<Position> shipPosition = new List<Position>();
                        int key = Int32.Parse(entry.Key);
                        Random rand = new Random();
                        bool randBool = (rand.Next(2) == 0);
                        int randX = rand.Next(10 - key);
                        int randY = rand.Next(10 - key);
                        for (int i = 0; i < key; i++)
                        {
                            int x = randX;
                            int y = randY;
                            if (randBool)
                            {
                                randY = randY + 1;
                            }
                            else
                            {
                                randX = randX + 1;
                            }
                            Position position = new Position(shipNumber, randX, randY);
                            shipPosition.Add(position);
                        }
                        enemyPositions.AddRange(shipPosition);
                        if (Collision.CheckShipsCollisions(enemyPositions))
                        {
                            shipNumber++;
                            value--;
                        }
                        else
                        {
                            foreach (var pos2 in shipPosition)
                            {
                                enemyPositions.Remove(pos2);
                            }
                        }
                    }
                }

                foreach (Position pos in enemyPositions)
                {
                    Ship ship = new Ship();
                    ship.playerShip = false;
                    ship.shipNumber = pos.shipNumber;
                    ship.x = pos.key;
                    ship.y = pos.value;
                    game.ships.Add(ship);
                }
                _context.Game.Update(game);
                _context.SaveChanges();
            }
            return Json(result);
        }

        // POST: Game/ShotEnemyShip/5
        [HttpPost]
        //        public Boolean SetShips(int shipNumber, int key, int value)
        public IActionResult ShotEnemyShip(int? id, int x, int y)
        {
            Game game = _context.Game.Find(id);
            List<Move> allMoves = _context.Move.ToList();
            List<Move> playerMoves = allMoves.FindAll( a => a.playerMove && a.gameId == game.gameId);
            List<Move> enemyMoves = allMoves.FindAll(a => a.playerMove == false && a.gameId == game.gameId);
            Boolean result = Collision.CheckShotCollision(x,y, playerMoves) && playerMoves.Count == enemyMoves.Count;
            if (result)
            {
                Move move = new Move();
                move.y = y;
                move.x = x;
                move.playerMove = true;
                game.moves.Add(move);
                _context.Game.Update(game);
                _context.SaveChanges();
            }

            List<Ship> enemyShips = _context.Ship.ToList().FindAll(s => s.playerShip == false && s.gameId == game.gameId
                                                                                              && s.x ==x && s.y==y);
//            Boolean shipShooted = Collision.CheckEnemyShipShooted(x, y, enemyShips);
            Boolean shipShooted = enemyShips.Count == 1;

            ShotResult finalResult = new ShotResult();
            finalResult.success = result;
            finalResult.hit = shipShooted;

            return Json(finalResult);
        }

        public IActionResult GetAliveShips(Boolean playerShips)
        {
            return null;
        }

        [HttpPost]
        public IActionResult GetEnemyShot(int? id)
        {
            Game actualGame = _context.Game.Find(id);
            Boolean shipFounded = false;
            List<Move> allMoves = _context.Move.ToList();
            List<Move> playerMoves = allMoves.FindAll(a => a.playerMove && a.gameId == actualGame.gameId);
            List<Move> enemyMoves = allMoves.FindAll(a => a.playerMove == false && a.gameId == actualGame.gameId);
            List<Ship> savedShips = new List<Ship>();
            List<Ship> playerShips2 = _context.Ship.ToList().FindAll(a => a.playerShip && a.gameId == actualGame.gameId);
            foreach(Move move in enemyMoves)
            {
                List<Ship> pS = playerShips2.FindAll(s => s.x == move.x && s.y == move.y);
                List<Ship> shipsToSave = new List<Ship>();
                if (pS.Count > 0)
                {
                    Ship a = pS[0];
                    shipsToSave.Add(a);
                    playerShips2.Remove(a);
                    List<Ship> ppp = playerShips2.FindAll(p => p.shipNumber == a.shipNumber && (p.x != a.x || p.y != a.y));
                    foreach (Ship sh in ppp)
                    {
                        playerShips2.Remove(sh);
                        if (enemyMoves.FindAll(e => e.x == sh.x && e.y == sh.y).Count == 0)
                        {

                            shipFounded = true;
                        }
                        else
                        {
                            shipsToSave.Add(sh);
                        }

                    }

                }

                if (shipFounded)
                {
                    foreach (Ship toSave in shipsToSave)
                    {
                        savedShips.Add(toSave);
                    }

                    break;
                }
            }
            Boolean result = true;
            EnemyShotResult enemyShotResult = new EnemyShotResult();

//            if(enemyMoves.FindAll(m => m.))

            if (shipFounded && playerMoves.Count > enemyMoves.Count)
            {
                if (savedShips.Count == 1)
                {
                    int x = savedShips[0].x;
                    int y = savedShips[0].y;

                    int[,] array = new int[10, 10];
                    if (x + 1 < 10)
                    {
                        array[x + 1, y] = 1;

                    }
                    if (x -1 >=0)
                    {
                        array[x - 1, y] = 1;

                    }
                    if (y + 1 < 10)
                    {
                        array[x, y + 1] = 1;

                    }
                    if (y-1 >0)
                    {
                        array[x, y - 1] = 1;

                    }
                    List<Move> moves = _context.Move.ToList().FindAll(a => a.playerMove == false && a.gameId == actualGame.gameId);

                    array = Collision.ResetArrayUsedMoves(array, moves);
                    int largest = 0;
                    List<int> largestX = new List<int>();
                    List<int> largestY = new List<int>();
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (largest < array[i, j])
                            {
                                largest = array[i, j];
                                largestX.RemoveAll(a => true);
                                largestY.RemoveAll(a => true);
                                largestX.Add(i);
                                largestY.Add(j);
                            }
                            else if (largest == array[i, j])
                            {
                                largestX.Add(i);
                                largestY.Add(j);
                            }
                        }
                    }
                    Move move = new Move();
                    Random random = new Random();
                    int randomNumber = random.Next(largestX.Count);
                    move.x = largestX[randomNumber];
                    move.y = largestY[randomNumber];
                    enemyShotResult.x = largestX[randomNumber];
                    enemyShotResult.y = largestY[randomNumber];
                    //or sunk
                    move.playerMove = false;
                    actualGame.moves.Add(move);
                    _context.Game.Update(actualGame);
                    _context.SaveChanges();

                    enemyShotResult.success = true;

                    List<Ship> playerShips = _context.Ship.ToList().FindAll(s => s.playerShip && s.gameId == actualGame.gameId
                                                                                              && s.x == enemyShotResult.x && s.y == enemyShotResult.y);
                    enemyShotResult.hit = playerShips.Count == 1;

                }
                else
                {
                    int maxY = savedShips[0].y;
                    int maxX = savedShips[0].x;

                    int minY = savedShips[0].y;
                    int minX = savedShips[0].x;

                    foreach (Ship sh in savedShips)
                    {
                        if (minX > sh.x)
                        {
                            minX = sh.x;
                        }
                        if (minY > sh.y)
                        {
                            minY = sh.y;
                        }
                        if (maxX < sh.x)
                        {
                            maxX = sh.x;
                        }
                        if (maxY < sh.y)
                        {
                            maxY = sh.y;
                        }
                    }

                    int[,] array = new int[10, 10];
                    if (maxY == minY)
                    {
                        if (maxX + 1 < 10)
                        {
                            array[maxX + 1, maxY] = 1;

                        }

                        if (minX - 1 >= 0)
                        {
                            array[minX - 1, maxY] = 1;

                        }
                    }
                    else
                    {
                        if (maxY + 1 < 10)
                        {
                            array[maxX, maxY + 1] = 1;

                        }

                        if (minY - 1 >= 0)
                        {
                            array[minX, minY - 1] = 1;

                        }
                    }

                    List<Move> moves = _context.Move.ToList().FindAll(a => a.playerMove == false && a.gameId == actualGame.gameId);

                    array = Collision.ResetArrayUsedMoves(array, moves);
                    int largest = 0;
                    List<int> largestX = new List<int>();
                    List<int> largestY = new List<int>();
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (largest < array[i, j])
                            {
                                largest = array[i, j];
                                largestX.RemoveAll(a => true);
                                largestY.RemoveAll(a => true);
                                largestX.Add(i);
                                largestY.Add(j);
                            }
                            else if (largest == array[i, j])
                            {
                                largestX.Add(i);
                                largestY.Add(j);
                            }
                        }
                    }
                    Move move = new Move();
                    Random random = new Random();
                    int randomNumber = random.Next(largestX.Count);
                    move.x = largestX[randomNumber];
                    move.y = largestY[randomNumber];
                    enemyShotResult.x = largestX[randomNumber];
                    enemyShotResult.y = largestY[randomNumber];
                    //or sunk
                    move.playerMove = false;
                    actualGame.moves.Add(move);
                    _context.Game.Update(actualGame);
                    _context.SaveChanges();

                    enemyShotResult.success = true;

                    List<Ship> playerShips = _context.Ship.ToList().FindAll(s => s.playerShip && s.gameId == actualGame.gameId
                                                                                              && s.x == enemyShotResult.x && s.y == enemyShotResult.y);
                    enemyShotResult.hit = playerShips.Count == 1;

                }

            }else if (playerMoves.Count > enemyMoves.Count)
            {

                List<Game> allFinishedGames =
                    _context.Game.ToList().FindAll(g => g.finished && g.playerId == actualGame.playerId);

                int[,] array = new int[10, 10];
                foreach (Game game in allFinishedGames)
                {
                    List<Ship> ships = _context.Ship.ToList();
                    ships = ships.FindAll(s => s.gameId == game.gameId && s.playerShip);
                    foreach (Ship ship in ships)
                    {
                        if (array[ship.x, ship.y] >= 0)
                        {
                            array[ship.x, ship.y]++;
                        }
                    }
                }
                List<Move> moves = _context.Move.ToList().FindAll(a => a.playerMove==false && a.gameId==actualGame.gameId);
                array = Collision.ResetArrayUsedMoves(array, moves);
                int largest = 0;
                List<int> largestX = new List<int>();
                List<int> largestY = new List<int>();
                for (int i = 0; i < 10; i++) {
                    for(int j = 0; j < 10; j++) {
                        if (largest < array[i, j])
                        {
                            largest = array[i, j];
                            largestX.RemoveAll(a => true);
                            largestY.RemoveAll(a => true);
                            largestX.Add(i);
                            largestY.Add(j);
                        }
                        else if(largest == array[i, j])
                        {
                            largestX.Add(i);
                            largestY.Add(j);
                        }
                    }
                }
                Move move = new Move();
                Random random = new Random();
                int randomNumber = random.Next(largestX.Count);
                move.x = largestX[randomNumber];
                move.y = largestY[randomNumber];
                enemyShotResult.x = largestX[randomNumber];
                enemyShotResult.y = largestY[randomNumber];
                //or sunk
                move.playerMove = false;
                actualGame.moves.Add(move);
                _context.Game.Update(actualGame);
                _context.SaveChanges();

                enemyShotResult.success = true;

                List<Ship> playerShips = _context.Ship.ToList().FindAll(s => s.playerShip && s.gameId == actualGame.gameId
                                                                                          && s.x== enemyShotResult.x && s.y == enemyShotResult.y);
                enemyShotResult.hit = playerShips.Count == 1;
            }
            else
            {
                enemyShotResult.success = false;
                enemyShotResult.hit = false;
            }
            return Json(enemyShotResult);
        }

        // POST: Game/IsFinished/id
        [HttpPost]
        public JsonResult IsFinished(int? id)
        {
            Game game = _context.Game.Find(id);

            Boolean isFinished = game.finished;
            if (!isFinished)
            {
                List<Move> playerMoves = _context.Move.ToList().FindAll(s => s.playerMove && s.gameId == game.gameId);
                List<Ship> enemyShips = _context.Ship.ToList().FindAll(s => s.playerShip == false && s.gameId == game.gameId);
                Boolean playerWins = false;
                foreach (Ship ship in enemyShips)
                {
                    if (playerMoves.FindAll(m => m.x == ship.x && m.y == ship.y).Count > 0)
                    {
                        playerWins = true;
                    }
                    else
                    {
                        playerWins = false;
                        break;
                    }
                }
                List<Move> enemyMoves = _context.Move.ToList().FindAll(s => s.playerMove == false && s.gameId == game.gameId);
                List<Ship> playerShips = _context.Ship.ToList().FindAll(s => s.playerShip && s.gameId == game.gameId);
                Boolean enemyWins = false;
                foreach (Ship ship in playerShips)
                {
                    if (enemyMoves.FindAll(m => m.x == ship.x && m.y == ship.y).Count > 0)
                    {
                        enemyWins = true;
                    }
                    else
                    {
                        enemyWins = false;
                        break;
                    }
                }

                if (playerWins || enemyWins)
                {
                    isFinished = true;
                    game.finished = true;
                    _context.Game.Update(game);
                    _context.SaveChanges();
                }
            }

            return Json(isFinished);
        }


        // POST: Game/Create
        [HttpPost]
        public JsonResult Create()
        {
            try
            {
                Game game = new Game();

                Player player = _context.Player.Find(getCurrentUserId());
                if (player.games == null)
                {
                    player.games = new List<Game>();
                }
                player.games.Add(game);
                _context.Player.Update(player);
                _context.SaveChanges();

                return Json(new{gameId = game.gameId});
            }
            catch
            {
                var gameId = -1;
                return Json(gameId);
            }
        }

        private Dictionary<String, int> GetShipSettings()
        {
            return new Dictionary<String, int>()
        {
            {"1", 0},
            { "2", 1},
            { "3", 1},
            { "4", 1},
            { "5", 1},
            { "6", 0}
        };
    }

            

        private int getCurrentUserId()
        {
            return 1;
            //return System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId()).UserName;
        }
    }
}