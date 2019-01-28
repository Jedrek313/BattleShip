using System;
using System.Collections.Generic;
using BattleShip.Models.Board;
using BattleShip.Models.Collision;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CollisionShipPositive()
        {
            List<Position> list = new List<Position>();
            Position position = new Position(0, 2, 2);
            Position position2 = new Position(0, 2, 4);
            Position position3 = new Position(0, 2, 3);
            list.Add(position);
            list.Add(position2);
            list.Add(position3);
            Boolean result = Collision.CheckShipsCollisions(list);
            Assert.True(result);
        }
        [Test]
        public void CollisionMultipleShipsPositive()
        {
            List<Position> list = new List<Position>();
            list.Add(new Position(0, 2, 2));
            list.Add(new Position(0, 2, 3));
            list.Add(new Position(0, 2, 4));

            list.Add(new Position(1, 5, 4));
            list.Add(new Position(1, 5, 3));
            list.Add(new Position(1, 5, 5));

            list.Add(new Position(2, 8, 5));
            list.Add(new Position(2, 7, 5));
            list.Add(new Position(2, 6, 5));
            Boolean result = Collision.CheckShipsCollisions(list);
            Assert.True(result);
        }

        [Test]
        public void CollisionShipLength1Positive()
        {
            List<Position> list = new List<Position>();
            Position position = new Position(0, 2, 2);
            list.Add(position);
            Boolean result = Collision.CheckShipsCollisions(list);
            Assert.True(result);
        }
        [Test]
        public void CollisionShipPositive2()
        {
            List<Position> list = new List<Position>();
            Position position = new Position(0, 2, 1);
            Position position2 = new Position(0, 3, 1);
            list.Add(position);
            list.Add(position2);
            Boolean result = Collision.CheckShipsCollisions(list);
            Assert.True(result);
        }
        [Test]
        public void CollisionShipNegative()
        {
            List<Position> list = new List<Position>();
            Position position = new Position(0, 2, 1);
            Position position2 = new Position(0, 3, 4);
            list.Add(position);
            list.Add(position2);
            Boolean result = Collision.CheckShipsCollisions(list);
            Assert.False(result);
        }
        [Test]
        public void CollisionShipLength3Negative()
        {
            List<Position> list = new List<Position>();
            Position position = new Position(0, 2, 1);
            Position position2 = new Position(0, 3, 4);
            Position position3 = new Position(0, 3, 4);
            list.Add(position);
            list.Add(position2);
            list.Add(position3);
            Boolean result = Collision.CheckShipsCollisions(list);
            Assert.False(result);
        }
    }
}