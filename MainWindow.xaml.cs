using System;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;


namespace Snake
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly int GameSize = 15;
        Tile[,] Tiles;
        Player player;
        DispatcherTimer Timer;
        bool GameStarted = false;
        public static bool Collision = false;
        public static bool DirectionChange = false;

        public MainWindow()
        {
            InitializeComponent();

            //Creating a grid with game tiles
            Tiles = new Tile[GameSize, GameSize];
            for(int i = 0; i < GameSize; i++)
            {
                GameTiles.ColumnDefinitions.Insert(i, new ColumnDefinition() { Width = new GridLength(App.TileSize)});
                GameTiles.RowDefinitions.Insert(i, new RowDefinition() { Height = new GridLength(App.TileSize)});

                for (int j = 0; j < GameSize; j++)
                {
                    Tile tile = new Tile();
                    GameTiles.Children.Add(tile);
                    Grid.SetColumn(tile, i);
                    Grid.SetRow(tile, j);

                    Tiles[i , j] = tile;
                }

            }

            GameTiles.Height = App.TileSize * GameSize;
            GameTiles.Width = App.TileSize * GameSize;

            // Creating a player
            player = new Player(Tiles);

            Tiles[9, 7].Fruit = true; //Default fruit location

        }

        //Picking location for a fruit
        void GenerateFruit()
        {
            while (true)
            {
                Random rand = new Random();
                int x = rand.Next(0, GameSize - 1);
                int y = rand.Next(0, GameSize - 1);
                if (Tiles[x, y].Snake)
                    continue;

                Tiles[x, y].Fruit = true;
                break;
            } 
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!GameStarted)
                FirstClick();

            if (DirectionChange)
                return;

            switch (e.Key)
            {
                case System.Windows.Input.Key.Right:
                    player.ChangeHeadDirection(Direction.Right);
                    break;
                case System.Windows.Input.Key.Left:
                    player.ChangeHeadDirection(Direction.Left);
                    break;
                case System.Windows.Input.Key.Down:
                    player.ChangeHeadDirection(Direction.Down);
                    break;
                case System.Windows.Input.Key.Up:
                    player.ChangeHeadDirection(Direction.Up);
                    break;
            }
        }

        void FirstClick()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 700);
            Timer.Tick += (x, y) =>
            {
                player.MoveSnake();

                if (Collision)
                {
                    Timer.Stop();
                    MessageBox.Show("Game over pajacu");
                    App.Current?.Shutdown();
                }

                for (int i = 0; i < GameSize; i++)
                {
                    for (int j = 0; j < GameSize; j++)
                    {
                        if (Tiles[i, j].Fruit)
                            return;
                    }
                }

                GenerateFruit();
            };
            Timer.Start();
        }
    }
}
