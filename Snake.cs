using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    public class Player
    {
        //Snake consists of array of tiles with snake, head is the first one, tail in the last one
        SnakeTile Head;
        SnakeTile Tail;
        List<SnakeTile> SnakeTiles;
        Tile[,] GameTiles;


        public Player(Tile[,] Tiles)
        {
            //Player start with a 3-lenght snake
            GameTiles = Tiles;

            Head = new SnakeTile(7, 7);
            GameTiles[Head.XPosition, Head.YPosition].Snake = true;

            SnakeTiles = new List<SnakeTile>();
            SnakeTiles.Add(Head);

            SnakeTile MiddleTile = new SnakeTile(6, 7);
            SnakeTiles.Add(MiddleTile);
            GameTiles[MiddleTile.XPosition, MiddleTile.YPosition].Snake = true;

            Tail = new SnakeTile(5, 7);
            SnakeTiles.Add(Tail);
            GameTiles[Tail.XPosition, Tail.YPosition].Snake = true;
        }

        //Changing direction of snake head, returns if direction change makes no sense
        public void ChangeHeadDirection(Direction direction)
        {
            
            if (Head.Direction == direction)
                return;

            if (Head.Direction == Direction.Right && direction == Direction.Left)
                return;

            if (Head.Direction == Direction.Left && direction == Direction.Right)
                return;

            if (Head.Direction == Direction.Up && direction == Direction.Down)
                return;

            if (Head.Direction == Direction.Down && direction == Direction.Up)
                return;

            Head.Direction = direction;
            MainWindow.DirectionChange = true;
        }

        //Moves whole snake by one tile, checks if player picks a fruit
        public void MoveSnake()
        {
            try
            {
                for (int i = 0; i < SnakeTiles.Count; i++)
                {

                    GameTiles[SnakeTiles[i].XPosition, SnakeTiles[i].YPosition].Snake = false;

                    SnakeTiles[i].Move();

                    
                    if (i == 0)
                        if (GameTiles[SnakeTiles[i].XPosition, SnakeTiles[i].YPosition].Snake)
                        {
                            MainWindow.Collision = true;

                        }

                    GameTiles[SnakeTiles[i].XPosition, SnakeTiles[i].YPosition].Snake = true;

                    Thread.Sleep(25 * i);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                MainWindow.Collision = true;
                return;
            }

            ChangeTilesDirection();

            if (GameTiles[Head.XPosition, Head.YPosition].Fruit)
            {
                PickFruit();
                GameTiles[Head.XPosition, Head.YPosition].Fruit = false;
            }

            MainWindow.DirectionChange = false;
        }

        //Makes snake 1 tile longer after picking a fruit
        void PickFruit()
        {
            int x = Tail.XPosition;
            int y = Tail.YPosition;
            Direction direction = Tail.Direction;

            SnakeTile tile = null;

            if (direction == Direction.Right)
                tile = new SnakeTile(--x, y);

            else if (direction == Direction.Left)
                tile = new SnakeTile(++x, y);

            else if (direction == Direction.Up)
                tile = new SnakeTile(x, ++y);
            else
                tile = new SnakeTile(x, --y);

            tile.Direction = Tail.Direction;

            SnakeTiles.Add(tile);
            Tail = tile;
            GameTiles[Tail.XPosition, Tail.YPosition].Snake = true;
        }
        
        //Changing direction of each snake tile while moving
        void ChangeTilesDirection()
        {
            SnakeTile PrevTile;
            SnakeTile tile;

            for (int i = 1; i < SnakeTiles.Count; i++)
            {
                tile = SnakeTiles[i];
                PrevTile = SnakeTiles[i - 1];

                if (PrevTile.XPosition > tile.XPosition)
                    tile.Direction = Direction.Right;
                else if (PrevTile.XPosition < tile.XPosition)
                    tile.Direction = Direction.Left;

                if (PrevTile.YPosition < tile.YPosition)
                    tile.Direction = Direction.Up;
                else if (PrevTile.YPosition > tile.YPosition)
                    tile.Direction = Direction.Down;
            }
        }
    }

    class SnakeTile
    {
        //Each tile of a snake has x/y axis position and direction at whitch it moves at
        public int XPosition;
        public int YPosition;
        public Direction Direction;
         

        public SnakeTile(int x, int y)
        {
            XPosition = x;
            YPosition = y;
        }

        public SnakeTile(int x, int y, Direction direction) : this(x,y)
        {
            Direction = direction;
        }
        
        //Changing tile position based on its direction
        public void Move()
        {
            switch (Direction)
            {
                case Direction.Right:
                    XPosition++;
                    break;

                case Direction.Left:
                    XPosition--;
                    break;

                case Direction.Up:
                    YPosition--;
                    break;

                case Direction.Down:
                    YPosition++;
                    break;
            }

        }
    }
}
