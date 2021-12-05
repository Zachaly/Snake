using System.Windows.Controls;
using System.Windows.Media;


namespace Snake
{
    /// <summary>
    /// Logika interakcji dla klasy Tile.xaml
    /// </summary>
    public partial class Tile : UserControl
    {
        bool _snake = false, _fruit = false;

        //Is there a snake on a tile 
        public bool Snake
        {
            get { return _snake; }
            set
            {
                if (value)
                    ChangeColor(Colors.Blue);
                else
                    ChangeColor(Colors.Gray);

                _snake = value;
            }
        }

        //Does a tile have a fruit
        public bool Fruit
        {
            get { return _fruit; }
            set
            {
                if (value)
                    ChangeColor(Colors.Green);
                else
                    ChangeColor(Colors.Gray);

                _fruit = value;
            }
        }
        //Both properties change color of a tile

        public Tile()
        {
            InitializeComponent();
            Width = App.TileSize;
            Height = App.TileSize;
        }

        
        void ChangeColor(Color color)
        {
            Resources.Remove("color");
            Resources.Add("color", new SolidColorBrush(color));
        }
    }
}
