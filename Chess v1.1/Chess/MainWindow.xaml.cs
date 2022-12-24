using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using General;

namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        App app;
        ContentControl menuControlls;

        public MainWindow()
        {
            InitializeComponent();

            app = App.Current as App;
        }

        private void PlayAction(object sender, RoutedEventArgs e)
        {
            Grid chessGrid = new Grid();
            chessGrid.ShowGridLines = true;
            chessGrid.Margin = new Thickness(20, 20, 20, 20);
            Image[,] boardImages = new Image[8, 8];
            Button[,] boardClick = new Button[8, 8];
            // kolumny
            ColumnDefinition[] columns = new ColumnDefinition[8];
            for (int i = 0; i < 8; i++)
            {
                columns[i] = new ColumnDefinition();
                columns[i].Width = new GridLength(100, GridUnitType.Star);
                chessGrid.ColumnDefinitions.Add(columns[i]);
            }
            // wiersze
            RowDefinition[] rows = new RowDefinition[8];
            for (int i = 0; i < 8; i++)
            {
                rows[i] = new RowDefinition();
                rows[i].Height = new GridLength(100, GridUnitType.Star);
                chessGrid.RowDefinitions.Add(rows[i]);
            }

            // Dodawanie pól
            Button button;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    button = new Button();
                    button.Click += BoardClick;
                    button.Content = "area " + Convert.ToString(i) + " " + Convert.ToString(j);
                    //Image img = new Image();
                    //img.Source = new BitmapImage(new Uri("ErrorImage.png", UriKind.Relative));
                    chessGrid.Children.Add(button);
                    Grid.SetColumn(button, i);
                    Grid.SetRow(button, j);

                    //boardImages[i, j] = new Image();
                    //boardImages[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    //boardImages[i, j].VerticalAlignment = VerticalAlignment.Top;
                    //boardImages[i, j].Width = 60;
                    //boardImages[i, j].Height = 60;
                    //boardImages[i, j].Margin = new Thickness(20 + 60 * i, 20 + 60 * j, 0, 0);
                    //Uri dirtUri = new Uri("pack://aplication:,,,/");
                    //boardImages[i, j].Source = new BitmapImage(new Uri("armor_stand.png", UriKind.Relative));
                    //imageBoard[i, j].Source = bee.Source;
                    //boardImages[i, j].CacheMode;
                    //grid.Children.Add(boardImages[i, j]);
                }
            }

            StackPanel contentPanel = new StackPanel();
            //grid.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/dirt.png", UriKind.Relative)));
            contentPanel.Children.Add(chessGrid);

            menuControlls = new ContentControl();
            menuControlls.Content = this.Content;
            this.Content = contentPanel;

            MessageBox.Show("Ta funkcja nie zastała jeszcze zaimplementowana!", "Funkcja nie istnieje", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void BoardClick(object sender, RoutedEventArgs e)
        {
            // pass
        }
        private void PlayOnlineAction(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ta funkcja nie zastała jeszcze zaimplementowana!", "Funkcja nie istnieje", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void LoadAction(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ta funkcja nie zastała jeszcze zaimplementowana!", "Funkcja nie istnieje", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void OptionsOpen(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ta funkcja nie zastała jeszcze zaimplementowana!", "Funkcja nie istnieje", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ExitAction(object sender, RoutedEventArgs e)
        {
            app.Shutdown();
        }
    }
}
