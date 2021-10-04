using System.Windows;

namespace Main_game
{
    public partial class Menu : Window
    {

        public Menu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Rozpoczyna nową grę
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Graj_Click(object sender, RoutedEventArgs e)
        {
            MainWindow sw = new MainWindow();
            sw.Show();
            this.Close();
        }

        /// <summary>
        /// Pokazuje wyniki
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Wyniki_Click(object sender, RoutedEventArgs e)
        {
            Wyniki sw = new Wyniki();
            sw.Show();
            this.Close();
        }

        /// <summary>
        /// Zamyka aplikację
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Wyjdz_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
