using System.IO;
using System.Windows;

namespace Main_game
{
    public partial class Wyniki : Window
    {
        public Wyniki()
        {
            InitializeComponent();
            FileStream fs = new FileStream("Results.txt", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            tResults.Text = sr.ReadToEnd();
            sr.Close();
            fs.Close();
        }

        /// <summary>
        /// Powraca do Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        /// <summary>
        /// Czyści plik z wynikami gry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            FileStream fs = new FileStream("Results.txt", FileMode.Truncate);
            StreamReader sr = new StreamReader(fs);
            tResults.Text = sr.ReadToEnd();
            sr.Close();
            fs.Close();
        }
    }
}
