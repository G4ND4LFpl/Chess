using System;
using System.Diagnostics;
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
using Core;

namespace Chess
{
    public partial class MainWindow : Window
    {
        App app;
        ContentControl menuControlls;
        Grid chessGrid;
        Board board;

        // dev
        Label timeL;
        long sum, peek = 0;
        long count = 0;

        public MainWindow()
        {
            InitializeComponent();

            app = App.Current as App;
        }
        private void StartNewGame(object sender, RoutedEventArgs e)
        {
            // Tworzenie planszy
            board = new Board();

            // ustawianie głównej siatki
            Grid grid = new Grid()
            {
                ShowGridLines = true,
                Margin = new Thickness(20, 20, 20, 20)
            };

            // kolumny duże
            ColumnDefinition[] outColumns = new ColumnDefinition[8];
            for (int i = 0; i < 2; i++)
            {
                outColumns[i] = new ColumnDefinition();
                outColumns[i].Width = new GridLength(100, GridUnitType.Star);
                grid.ColumnDefinitions.Add(outColumns[i]);
            }

            // ustawianie siatki szachów
            chessGrid = new Grid
            {
                ShowGridLines = false,
                Margin = new Thickness(10, 10, 10, 10)
            };

            // wiersze
            RowDefinition[] rows = new RowDefinition[8];
            for (int i = 0; i < 8; i++)
            {
                rows[i] = new RowDefinition();
                rows[i].Height = new GridLength(100, GridUnitType.Star);
                chessGrid.RowDefinitions.Add(rows[i]);
            }
            // kolumny
            ColumnDefinition[] columns = new ColumnDefinition[8];
            for (int i = 0; i < 8; i++)
            {
                columns[i] = new ColumnDefinition();
                columns[i].Width = new GridLength(100, GridUnitType.Star);
                chessGrid.ColumnDefinitions.Add(columns[i]);
            }

            // Dodawanie pól
            AreaButton button;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    button = new AreaButton(i + j);
                    button.Style = (Style)Resources["customButton"];
                    button.Click += BoardClick;
                    button.SourceSet(board.GetFigureImgSource(i, j));

                    chessGrid.Children.Add(button);
                    Grid.SetColumn(button, i);
                    Grid.SetRow(button, j);
                }
            }

            grid.Children.Add(chessGrid);
            Grid.SetColumn(chessGrid, 0);
            Grid.SetColumnSpan(chessGrid, 1);

            // dev
            timeL = new Label();
            timeL.Content = "Czas";
            grid.Children.Add(timeL);
            Grid.SetColumn(timeL, 1);

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
            AreaButton sendButton = (AreaButton)sender;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            board.Click(Grid.GetColumn(sendButton), Grid.GetRow(sendButton));
            watch.Stop();
            sum += watch.ElapsedTicks;
            peek = Math.Max(peek, watch.ElapsedTicks);
            count++;
            timeL.Content = "Czas ruchu: " + watch.ElapsedTicks.ToString() + "\nŚredni czas: "+(sum/count).ToString() + "\nMax: " + peek;

            /*
             * Średni czas ~ 400
             * Max czas - 5200
            */
            UpdateBoard();
        }
        private void UpdateBoard()
        {
            foreach (AreaButton button in chessGrid.Children)
            {
                button.SourceSet(board.GetFigureImgSource(Grid.GetColumn(button), Grid.GetRow(button)));
                if (board.IsChoosen(Grid.GetColumn(button), Grid.GetRow(button))) button.LockBorderOn = true;
                else button.LockBorderOn = false;
            }
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
