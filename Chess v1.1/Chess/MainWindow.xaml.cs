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

namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        App app;
        public MainWindow()
        {
            InitializeComponent();

            app = App.Current as App;
        }
        private void SizeAdjust(object sender, RoutedEventArgs args)
        {
            // dopasowanie wielkości interfejsu
        }

        private void PlayAction(object sender, RoutedEventArgs e)
        {
            // gra
        }
        private void PlayOnlineAction(object sender, RoutedEventArgs e)
        {
            
        }
        private void LoadAction(object sender, RoutedEventArgs e)
        {

        }
        private void OptionsOpen(object sender, RoutedEventArgs e)
        {

        }
        private void ExitAction(object sender, RoutedEventArgs e)
        {
            app.Shutdown();
        }
    }
}
