using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Main_game
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Przechowuje pozycję gracza
        /// </summary>
        private int pos;
        /// <summary>
        /// Pozwala na start wątkom
        /// </summary>
        private bool start = true;
        /// <summary>
        /// Przechowuje wynik gry
        /// </summary>
        private int results = 0;
        private Canvas[] canvases;
        private Player[] position;
        private Thread[] tasks;

        private Image man = new Image();


        /// <summary>
        /// Obiekty owoców i słodyczy
        /// </summary>
        private Image fruit = new Image();
        private Image fruit1 = new Image();
        private Image fruit2 = new Image();
        private Image fruit3 = new Image();
        private Image fruit4 = new Image();
        private Image candie = new Image();
        private Image candie1 = new Image();
        private Image candie2 = new Image();
        private Image candie3 = new Image();
        private Image candie4 = new Image();
        /// <summary>
        /// Obiekt tła
        /// </summary>
        private Image backg = new Image();

        /// <summary>
        /// Służy do ustawiania tła okna
        /// </summary>
        private ImageBrush myBrush = new ImageBrush();

        /// <summary>
        /// Linki do źródeł obrazków
        /// </summary>
        private Uri apple = new Uri("pack://application:,,,/Images/apple.png");
        private Uri candieu = new Uri("pack://application:,,,/Images/candies.jpg");
        private Uri chocolate = new Uri("pack://application:,,,/Images/chocolate.png");
        private Uri melon = new Uri("pack://application:,,,/Images/melon.png");
        private Uri BG1 = new Uri("pack://application:,,,/Images/beach.jpg");
        private Uri BG2 = new Uri("pack://application:,,,/Images/lake.jpg");

        /// <summary>
        /// Obiekt zegara działającego w tle
        /// </summary>
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        /// <summary>
        /// Klasa główna, inicjalizuje komonenty
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }
        

        /// <summary>
        /// Start zegara w tle wraz z uruchomieniem okna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Tick +=  new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            stamina.Value = 100;
        }
        

        /// <summary>
        /// Przygotowanie nowej gry
        /// </summary>
        private void NewGame()
        {
            man.Source = new BitmapImage(new Uri("pack://application:,,,/Images/awatar.png"));
            man.Width = 119;
            man.Height = 145;
            position = new Player[5];
            for (int i = 0; i < 5; i++)
                position[i] = Player.free;
            position[0] = Player.player;
            canvases = new Canvas[] { can00, can01, can02, can03, can04} ;
            canvases[0].Children.Add(man);
            canvases[0].Focus();
            backg.Source = new BitmapImage(BG1);
            myBrush.ImageSource = backg.Source;
            this.Background = myBrush;

            Thread task = new Thread(MoveImcan0);
            Thread task1 = new Thread(MoveImcan1);
            Thread task2 = new Thread(MoveImcan2);
            Thread task3 = new Thread(MoveImcan3);
            Thread task4 = new Thread(MoveImcan4);
            tasks = new Thread[] {task, task1, task2, task3, task4 };
            ThreadsMan(tasks);
        }


        /// <summary>
        /// Zarządzanie wątkami
        /// </summary>
        /// <param name="tasks"></param>
        private void ThreadsMan(Thread[] tasks)
        {
            for (int i = 0; i < 5; i++)
            {
                if (start)
                    tasks[i].Start();
                else
                    tasks[i].Abort();
            }
        }


        /// <summary>
        /// Funkcja znajduje pozycję gracza
        /// </summary>
        private void GetPosition()
        {
            for (int i = 0; i < 5; i++)
            {
                if (position[i] == Player.player)
                {
                    pos = i;
                    break;
                }
            }
        }


        /// <summary>
        /// Funkcja zmienia pozycję gracza
        /// </summary>
        /// <param name="x"></param>
        private void ChangePosition(bool x)
        {
            canvases[pos].Children.Remove(man);
            position[pos] = Player.free;
            if (x)
                pos--;
            else
                pos++;
            position[pos] = Player.player;
            canvases[pos].Children.Add(man);
        }


        /// <summary>
        /// Inicjalizuje zmianę pozycji gracza poprzez wciśnięcie odpowiedniego przycisku (strzałki góra dół)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private new void KeyDown(object sender, KeyEventArgs e)
        {
            GetPosition();
            if (e.Key == Key.Up && pos > 0)
            {
                ChangePosition(true);
            }
            if (e.Key == Key.Down && pos < 4)
            {
                ChangePosition(false);
            }
        }
        

        /// <summary>
        /// Funkcja wykonuje się w każdy takt zegara (co sekundę)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            stamina.Value--;
            results++;
            if(stamina.Value == 0)
            {
                start = false;
                ThreadsMan(tasks);
                MessageBoxResult koniec = MessageBox.Show("Twój wynik: " + results, "Koniec gry", MessageBoxButton.OK);
                if(koniec == MessageBoxResult.OK)
                {
                    dispatcherTimer.Stop();
                    FileStream fs = new FileStream("Results.txt", FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine("Wynik: " + results + " z godziny " + DateTime.Now);
                    sw.Close();
                    fs.Close();
                    Menu mn = new Menu();
                    mn.Show();
                    this.Close();
                }
            }
            if (results == 150)
            {
                backg.Source = new BitmapImage(BG2);
                myBrush.ImageSource = backg.Source;
                this.Background = myBrush;
            }
        }

        /// <summary>
        /// Nasłuchuje na kliknęcie i przerywa grę
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void End_Click(object sender, RoutedEventArgs e)
        {
            start = false;
            ThreadsMan(tasks);
            dispatcherTimer.Stop();
            Menu sw = new Menu();
            sw.Show();
            this.Close();
        }


        /// <summary>
        /// Ustawia wymiary renderowanego obrazka
        /// </summary>
        /// <param name="a">Obrazek, którego wymiary mają być zmienione</param>
        private void SetWH(Image a)
        {
            a.Height = 130;
            a.Width = 130;
        }

        /// <summary>
        /// Zmienia czas renderowania się obrazka
        /// </summary>
        /// <param name="time">Czas wykonywania się animacji w danej metodzie</param>
        /// <param name="y">Czas, o który animacja ma przyspieszyć lub zwolnić</param>
        private int Change_time(int time, int y)
        {
            Random random = new Random();
            int temp = random.Next(0, 10);
            if (time < 1000)
                return time = time + 500;
            else if (time > 5000)
                return time = time - 2000;
            else if (temp < 7)
                return time = time - y;
            else
                return time = time + y;
        }


        /// <summary>
        /// Renderowanie się obrazków w pierwszym rzędzie
        /// </summary>
        private void MoveImcan0()
        {
            Thread.Sleep(1000);
            int time = 3500;
            while (true)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TranslateTransform x = new TranslateTransform();
                    DoubleAnimation anim = new DoubleAnimation(1170, 0, TimeSpan.FromMilliseconds(2 * time));
                    candie.Source = new BitmapImage(candieu);
                    SetWH(candie);
                    candie.RenderTransform = x;
                    can10.Children.Add(candie);
                    x.BeginAnimation(TranslateTransform.XProperty, anim);
                }));
                Thread.Sleep(time / 2);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TranslateTransform x = new TranslateTransform();
                    DoubleAnimation anim = new DoubleAnimation(1170, 0, TimeSpan.FromMilliseconds(2 * time));
                    fruit.Source = new BitmapImage(apple);
                    SetWH(fruit);
                    fruit.RenderTransform = x;
                    can10.Children.Add(fruit);
                    x.BeginAnimation(TranslateTransform.XProperty, anim);
                }));
                Thread.Sleep((int)(time * 1.5));
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    GetPosition();
                    if (pos == 0)
                        stamina.Value = stamina.Value - 2;
                    can10.Children.Remove(candie);
                }));
                Thread.Sleep(time / 2);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (pos == 0)
                        stamina.Value++;
                    can10.Children.Remove(fruit);
                }));
                time = Change_time(time, 200);
            }
        }

        /// <summary>
        /// Renderowanie się obrazków w drugim rzędzie
        /// </summary>
        private void MoveImcan1()
        {
            Thread.Sleep(2000);
            int time = 4000;
            while (true)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TranslateTransform x = new TranslateTransform();
                    DoubleAnimation anim = new DoubleAnimation(1170, 0, TimeSpan.FromMilliseconds(2 * time));
                    fruit1.Source = new BitmapImage(apple);
                    SetWH(fruit1);
                    fruit1.RenderTransform = x;
                    can11.Children.Add(fruit1);
                    x.BeginAnimation(TranslateTransform.XProperty, anim);
                }));
                Thread.Sleep(time);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TranslateTransform x = new TranslateTransform();
                    DoubleAnimation anim = new DoubleAnimation(1170, 0, TimeSpan.FromMilliseconds(time + (time / 2)));
                    candie1.Source = new BitmapImage(chocolate);
                    SetWH(candie1);
                    candie1.RenderTransform = x;
                    can11.Children.Add(candie1);
                    x.BeginAnimation(TranslateTransform.XProperty, anim);
                }));
                Thread.Sleep(time);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    GetPosition();
                    if (pos == 1)
                        stamina.Value++;
                    can11.Children.Remove(fruit1);
                }));
                Thread.Sleep(time / 2);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (pos == 1)
                        stamina.Value = stamina.Value - 2;
                    can11.Children.Remove(candie1);
                }));
                time = Change_time(time, 200);
            }
        }

        /// <summary>
        /// Renderowanie się obrazków w trzecim rzędzie
        /// </summary>
        private void MoveImcan2()
        {
            Thread.Sleep(1000);
            int time = 3500;
            while (true)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TranslateTransform x = new TranslateTransform();
                    DoubleAnimation anim = new DoubleAnimation(1170, 0, TimeSpan.FromMilliseconds(time + (time / 4)));
                    fruit2.Source = new BitmapImage(melon);
                    SetWH(fruit2);
                    fruit2.RenderTransform = x;
                    can12.Children.Add(fruit2);
                    x.BeginAnimation(TranslateTransform.XProperty, anim);
                }));
                Thread.Sleep(time);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TranslateTransform x = new TranslateTransform();
                    DoubleAnimation anim = new DoubleAnimation(1170, 0, TimeSpan.FromMilliseconds(2 * time));
                    candie2.Source = new BitmapImage(chocolate);
                    SetWH(candie2);
                    candie2.RenderTransform = x;
                    can12.Children.Add(candie2);
                    x.BeginAnimation(TranslateTransform.XProperty, anim);
                }));
                Thread.Sleep((time / 4));
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    GetPosition();
                    if (pos == 2)
                        stamina.Value++;
                    can12.Children.Remove(fruit2);
                }));
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TranslateTransform x = new TranslateTransform();
                    DoubleAnimation anim = new DoubleAnimation(1170, 0, TimeSpan.FromMilliseconds(time + (time / 4)));
                    fruit2.Source = new BitmapImage(melon);
                    SetWH(fruit2);
                    fruit2.RenderTransform = x;
                    can12.Children.Add(fruit2);
                    x.BeginAnimation(TranslateTransform.XProperty, anim);
                }));
                Thread.Sleep(time * 5 / 4);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    GetPosition();
                    if (pos == 2)
                        stamina.Value++;
                    can12.Children.Remove(fruit2);
                }));
                Thread.Sleep(time * 2 / 4);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (pos == 2)
                        stamina.Value = stamina.Value - 2;
                    can12.Children.Remove(candie2);
                }));
                time = Change_time(time, 200);
            }
        }

        /// <summary>
        /// Renderowanie się obrazków w czwartym rzędzie
        /// </summary>
        private void MoveImcan3()
        {
            Thread.Sleep(1500);
            int time = 3000;
            while (true)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TranslateTransform x = new TranslateTransform();
                    DoubleAnimation anim = new DoubleAnimation(1170, 0, TimeSpan.FromMilliseconds(time + (time / 10)));
                    candie3.Source = new BitmapImage(chocolate);
                    SetWH(candie3);
                    candie3.RenderTransform = x;
                    can13.Children.Add(candie3);
                    x.BeginAnimation(TranslateTransform.XProperty, anim);
                }));
                Thread.Sleep(time / 10);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TranslateTransform x = new TranslateTransform();
                    DoubleAnimation anim = new DoubleAnimation(1170, 0, TimeSpan.FromMilliseconds(2 * time));
                    fruit3.Source = new BitmapImage(melon);
                    SetWH(fruit3);
                    fruit3.RenderTransform = x;
                    can13.Children.Add(fruit3);
                    x.BeginAnimation(TranslateTransform.XProperty, anim);
                }));
                Thread.Sleep(time);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    GetPosition();
                    if (pos == 3)
                        stamina.Value = stamina.Value - 2;
                    can13.Children.Remove(candie3);
                }));
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TranslateTransform x = new TranslateTransform();
                    DoubleAnimation anim = new DoubleAnimation(1170, 0, TimeSpan.FromMilliseconds(time + (time / 2)));
                    candie3.Source = new BitmapImage(chocolate);
                    SetWH(candie3);
                    candie3.RenderTransform = x;
                    can13.Children.Add(candie3);
                    x.BeginAnimation(TranslateTransform.XProperty, anim);
                }));
                Thread.Sleep(time);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (pos == 3)
                        stamina.Value++;
                    can13.Children.Remove(fruit3);
                }));
                Thread.Sleep(time / 2);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    GetPosition();
                    if (pos == 3)
                        stamina.Value = stamina.Value - 2;
                    can13.Children.Remove(candie3);
                }));
                time = Change_time(time, 200);
            }
        }

        /// <summary>
        /// Renderowanie się obrazków w piątym rzędzie
        /// </summary>
        private void MoveImcan4()
        {
            Thread.Sleep(1000);
            int time = 3000;
            while (true)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TranslateTransform x = new TranslateTransform();
                    DoubleAnimation anim = new DoubleAnimation(1170, 0, TimeSpan.FromMilliseconds(2 * time));
                    fruit4.Source = new BitmapImage(melon);
                    SetWH(fruit4);
                    fruit4.RenderTransform = x;
                    can14.Children.Add(fruit4);
                    x.BeginAnimation(TranslateTransform.XProperty, anim);
                }));
                Thread.Sleep(time);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    TranslateTransform x = new TranslateTransform();
                    DoubleAnimation anim = new DoubleAnimation(1170, 0, TimeSpan.FromMilliseconds(2 * time));
                    candie4.Source = new BitmapImage(candieu);
                    SetWH(candie4);
                    candie4.RenderTransform = x;
                    can14.Children.Add(candie4);
                    x.BeginAnimation(TranslateTransform.XProperty, anim);
                }));
                Thread.Sleep(time);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    GetPosition();
                    if (pos == 4)
                        stamina.Value++;
                    can14.Children.Remove(fruit4);
                }));
                Thread.Sleep(time);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (pos == 4)
                        stamina.Value = stamina.Value - 2;
                    can14.Children.Remove(candie4);
                }));
                time = Change_time(time, 200);
            }
        }
    }
}