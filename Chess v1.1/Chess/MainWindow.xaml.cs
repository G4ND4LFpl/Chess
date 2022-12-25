//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
using General;

namespace Chess
{
    public partial class MainWindow : Window
    {
        App app;
        ContentControl menuControlls;
        Board board;

        public MainWindow()
        {
            InitializeComponent();

            app = App.Current as App;
        }

        private void PlayAction(object sender, RoutedEventArgs e)
        {
            // ustawianie głównej siatki
            Grid grid = new Grid()
            {
                ShowGridLines = true,
                Margin = new Thickness(20, 20, 20, 20)
            };

            ColumnDefinition column0 = new ColumnDefinition
            {
                Width = new GridLength(100, GridUnitType.Star)
            };
            grid.ColumnDefinitions.Add(column0);
            ColumnDefinition column1 = new ColumnDefinition
            {
                Width = new GridLength(100, GridUnitType.Star)
            };
            grid.ColumnDefinitions.Add(column1);

            // ustawianie siatki szachów
            Grid chessGrid = new Grid
            {
                ShowGridLines = false,
                Margin = new Thickness(10, 10, 10, 10)
            };
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
            AreaButton button;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    button = new AreaButton(i+j);
                    button.Style = (Style)Resources["customButton"];
                    button.Click += BoardClick;
                    button.SourceSet(board[i, j].GetPath());

                    chessGrid.Children.Add(button);
                    Grid.SetColumn(button, i);
                    Grid.SetRow(button, j);
                }
            }
            grid.Children.Add(chessGrid);
            Grid.SetColumn(chessGrid, 0);

            // Stack panel i content controlls - definicje
            StackPanel contentPanel = new StackPanel();
            contentPanel.Children.Add(grid);

            menuControlls = new ContentControl();
            menuControlls.Content = this.Content;
            this.Content = contentPanel;
        }
        private void BoardClick(object sender, RoutedEventArgs e)
        {
            // brak 
            MessageBox.Show("Ta funkcja nie zastała jeszcze zaimplementowana!", "Funkcja nie istnieje", MessageBoxButton.OK, MessageBoxImage.Information);
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
