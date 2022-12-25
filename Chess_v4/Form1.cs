using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
//using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Threading;
//using System.Threading.Tasks;
using System.Windows.Forms;
using MyClasses;
using MyClasses.Figures;
using MyClasses.Languages;

namespace Chess
{
    public partial class Form1 : Form
    {
        PlayBoard board;
        ImageFrame MenuBackground;
        int deltaX, deltaY;
        Clock clock1, clock2;
        Language lang;
        const string saveformat = "0.3";
        
        //Konstruktor
        public Form1()
        {
            InitializeComponent();
            button2.MouseDown += MoveWindowBegin;
            button2.MouseMove += MoveWindow;
            // Panel
            PanelBuild();
            // Language
            LangComboBoxSet();
            TimeComboBoxSet();

            // Zdjęcie w tle
            MenuBackground = new ImageFrame();
            MenuBackground.Location = new Point(0, 30);
            MenuBackground.Size = new Size(645, 645);
            MenuBackground.SetImage("BackMenu.png");
            Controls.Add(MenuBackground);

            mainLabel.BackColor = Color.Transparent;
        }

        //Program główny
        private void Start(object sender, EventArgs e)
        {
            //Zmiany Interfesju
            MenuShow(false, sender);
            GameInterfaceShow(true, sender);
            MenuBackground.Visible = false;

            //Delegaci
            MyClasses.Action resoult = End;
            MyClasses.Action st = SwitchTimer;

            // Inicjalizacja tablicy
            board = new PlayBoard(resoult, st,Size);
            board.Turn = ChessColor.White;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j].Click += new EventHandler(board.OnClick);
                    Controls.Add(board[i, j]);
                }
            }
            for (int i = 0; i < 15; i++)
            {
                Controls.Add(board.UpSlots[i]);
                Controls.Add(board.DownSlots[i]);
            }
            if (sender == StartButton)
            {
                GenerateSetUp(board);
                //Zegar
                if (comboBox2.SelectedIndex != 6)
                {
                    clock1 = new Clock(new ChessTime(Clock.GetTime(comboBox2.Text), 0));
                    Controls.Add(clock1);
                    clock1.UpdateText();
                    timer1.Tick += new EventHandler(clock1.Tick);

                    clock2 = new Clock(new ChessTime(Clock.GetTime(comboBox2.Text), 0));
                    Controls.Add(clock2);
                    clock2.UpdateText();
                    timer2.Tick += new EventHandler(clock2.Tick);
                    ClockSet();

                    label2.Visible = true;
                    Clock.IsTimeActive = true;
                    SwitchTimer(1);
                }
            }
        }
        private void GenerateSetUp(PlayBoard tab)
        {
            for (int i = 0; i < 8; i++)
            {
                //Białe piony
                tab[i, 6].SetFigure(new Pawn(true));
                //Czarne piony
                tab[i, 1].SetFigure(new Pawn(false));
            }
            //Białe
            tab[2, 7].SetFigure(new Bishop(true));
            tab[5, 7].SetFigure(new Bishop(true));
            tab[0, 7].SetFigure(new Rook(true));
            tab[7, 7].SetFigure(new Rook(true));
            tab[1, 7].SetFigure(new Knight(true));
            tab[6, 7].SetFigure(new Knight(true));
            tab[3, 7].SetFigure(new Queen(true));
            tab[4, 7].SetFigure(new King(true));
            //Czarne
            tab[2, 0].SetFigure(new Bishop(false));
            tab[5, 0].SetFigure(new Bishop(false));
            tab[0, 0].SetFigure(new Rook(false));
            tab[7, 0].SetFigure(new Rook(false));
            tab[1, 0].SetFigure(new Knight(false));
            tab[6, 0].SetFigure(new Knight(false));
            tab[3, 0].SetFigure(new Queen(false));
            tab[4, 0].SetFigure(new King(false));
        }
        private void End(int i)
        {
            if (i > 0) mainLabel.Text = "Wygrywa Biały";
            if (i < 0) mainLabel.Text = "Wygrywa Czarny";
            if (i == 0) mainLabel.Text = "Pat";

            //Zmiany w inetrfajsie
            mainLabel.Visible = true;
            button1.Visible = true;
            SaveButton.Visible = false;
            button5.Visible = false;
            label2.Visible = false;

            SwitchTimer(0);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //Panel
            panel1.Visible = true;
        }
        private void BackToMenu(object sender, EventArgs e)
        {
            if(sender is Button)
            {
                Button bb = (Button)sender;
                if(bb.Name == "PanelButton1")
                {
                    sender = button5;
                    panel1.Visible = false;
                }
            }
            GameInterfaceShow(false, sender);
            MenuShow(true, sender);
            MenuBackground.Visible = true;

            //Usuwanie planszy
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Controls.Remove(board[i, j]);
                }
            }
            for (int i = 0; i < 15; i++)
            {
                Controls.Remove(board.UpSlots[i]);
                Controls.Remove(board.DownSlots[i]);
            }
            board = null;
        }
        
        //Wyświetlanie Interfejsu
        private void MenuShow(bool visible, object sender)
        {
            if (visible)
            {
                //Start, Wczytaj, Ustawienia, Wyjście
                StartButton.Visible = true;
                LoadButton.Visible = true;
                Button3.Visible = true;
                ExitButton.Visible = true;
                //Autor, wersja
                label1.Visible = true;
                label6.Visible = true;
                //Obrazek w menu
                //MenuBackground.Visible = true;
            }
            else
            {
                //Start, Wczytaj, Ustawienia, Wyjście
                StartButton.Visible = false;
                LoadButton.Visible = false;
                Button3.Visible = false;
                ExitButton.Visible = false;
                //Autor, wersja
                label1.Visible = false;
                label6.Visible = false;
                //Obrazek w menu
                //MenuBackground.Visible = false;
            }
        }
        private void GameInterfaceShow(bool visible, object sender)
        {
            if (visible)
            {
                //Przyciski z prawej strony
                SaveButton.Visible = true;
                button5.Visible = true;
                //Tura, Wynik
                //label2.Visible = true;
                //mainLabel.Visible = true;
                //Tło
                BackgroundImage = Image.FromFile(@"images\BackGround1.png");
                BackgroundImageLayout = ImageLayout.Stretch;
                //Dalej
                //button1.Visible = true;
            }
            else
            {
                //Zapis
                SaveButton.Visible = false;
                button5.Visible = false;
                //Tura, Wynik
                label2.Visible = false;
                mainLabel.Visible = false;
                //Tło
                BackgroundImage = null;
                //Dalej
                button1.Visible = false;
                //Zegar
                if (sender == button5)
                {
                    label2.Visible = false;

                    SwitchTimer(0);
                }
            }
        }
        private void PanelHide(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        //Wczytywanie i zapis
        private void Save(object sender, EventArgs e)
        {
            Button bb = (Button)sender;
            if (bb.Name == "PanelButton3")
            {
                panel1.Visible = false;
            }

            //Pobieranie ścieżki
            string path = Directory.GetCurrentDirectory();
            saveFileDialog1.Title = "Zapisywanie";
            saveFileDialog1.InitialDirectory = path + "\\saves";

            DateTime time = DateTime.Now;

            //Ustawianie nazwy pliku
            string day, month, year;
            if (time.Day < 10) day = "0" + time.Day;
            else day = time.Day.ToString();
            if (time.Month < 10) month = "0" + time.Month;
            else month = time.Month.ToString(); ;
            year = (time.Year - 2000).ToString();

            saveFileDialog1.FileName = day + "." + month + "." + year + "_" + time.Hour + "." + time.Minute + "." + time.Second;
            saveFileDialog1.Filter = "*.dat|*.dat*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                if (filename.Contains(".dat") == false) filename += ".dat";
                //Przygotowanie struktury
                SaveAreas areas = (SaveAreas)board;
                areas.IsTime = Clock.IsTimeActive;
                if(Clock.IsTimeActive == true)
                {
                    areas.clock1 = clock1.Time;
                    areas.clock2 = clock2.Time;
                }
                areas.version = saveformat;
                //Serializacja
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream;
                try
                {
                    stream = new FileStream(filename, FileMode.Create);
                    formatter.Serialize(stream, areas);
                }
                catch (Exception x)
                {
                    throw x; //nie powinno się wydarzyć
                }
                stream.Close();
                if(bb.Name == "PanelButton3")
                {
                    BackToMenu(button5, e);
                }
            }
        }
        private void LoadGame(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            openFileDialog1.Title = "Wczytywanie";
            openFileDialog1.InitialDirectory = path + "\\saves";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "*.dat|*.dat*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Start(sender, e);
                //Deserializacja
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream;
                SaveAreas areas;
                try
                {
                    stream = new FileStream(openFileDialog1.FileName, FileMode.Open);
                    areas = (SaveAreas)formatter.Deserialize(stream);
                    stream.Close();

                    //Sprawdzanie poprawności
                    if (areas.version != saveformat)
                    {
                        string msg = "Zapis stanu gry jest w starym formacie i jego wczytanie może być niemożliwe.\nCzy chcesz podjąć próbe jego załadowania?";
                        DialogResult result = MessageBox.Show(msg, "Niezgodność wersji", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No) throw new FileFormatException();
                    }

                    LoadIn(ref areas);
                }
                catch (SerializationException x)
                {
                    string msg = "Wybrany plik jest niekompletny, posiada nieprawidłowy format lub został uszkodzony.";
                    if(DialogResult.OK == MessageBox.Show(msg, "Nieodpowiedni format!", MessageBoxButtons.OK, MessageBoxIcon.Error))
                        BackToMenu(x, null);
                }
                catch (IOException x)
                {
                    string msg = "Wybrany plik jest aktualnie otwarty w innym programie i nie może zostać wczytany.\nZamknij inne programy korzystające z tego pliku i spróbuj ponownie.";
                    if (DialogResult.OK == MessageBox.Show(msg, "Plik jest zajęty!", MessageBoxButtons.OK, MessageBoxIcon.Warning))
                        BackToMenu(x, null);
                }
                catch (FileFormatException x)
                {
                    BackToMenu(x, null);
                }
            }
        }
        private void LoadIn(ref SaveAreas areas)
        {
            //Przywracanie planszy
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    board[i, j].SetFigure(areas.tab[i, j]);
            }
            for (int i = 0; i < 15; i++)
            {
                board.UpSlots[i].SetImage(areas.slots1[i]);
                if (board.UpSlots[i].Image != null) board.UpSlots[i].Visible = true;
                board.DownSlots[i].SetImage(areas.slots2[i]);
                if (board.DownSlots[i].Image != null) board.DownSlots[i].Visible = true;
            }
            board.Turn = areas.turn;
            //Ustawianie czasu
            if(areas.IsTime == true)
            {
                clock1 = new Clock(areas.clock1);
                Controls.Add(clock1);
                clock1.UpdateText();
                timer1.Tick += new EventHandler(clock1.Tick);

                clock2 = new Clock(areas.clock2);
                Controls.Add(clock2);
                clock2.UpdateText();
                timer2.Tick += new EventHandler(clock2.Tick);
                ClockSet();

                label2.Visible = true;
                Clock.IsTimeActive = true;
                if (areas.turn == ChessColor.White)
                {
                    SwitchTimer(1);
                }
                else SwitchTimer(2);
            }
        }
        
        //Ustawienia
        private void OptionIn(object sender, EventArgs e)
        {
            MenuShow(false, sender);

            //Ustawianie interfejsu - ustawienia
            button4.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            comboBox1.Visible = true;
            comboBox2.Visible = true;
        }
        private void OptionOut(object sender, EventArgs e)
        {
            //Chowanie interfaju ustawień
            button4.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            comboBox1.Visible = false;
            comboBox2.Visible = false;

            MenuShow(true, sender);
        }
        
        //Inne
        private void MouseIn(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.BackColor = Color.FromArgb(90, 50, 25);
        }
        private void MouseOut(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.BackColor = Color.FromArgb(120, 60, 25);
        }
        private void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void SwitchTimer(int t)
        {
            if (Clock.IsTimeActive == false) return;
            if(t == 1)
            {
                timer2.Enabled = false;
                timer1.Enabled = true;
                clock2.Visible = false;
                clock1.Visible = true;
                label2.Text = lang.turn[0];
            }
            else if(t == 2)
            {
                timer1.Enabled = false;
                timer2.Enabled = true;
                clock1.Visible = false;
                clock2.Visible = true;
                label2.Text = lang.turn[1];
            }
            else //reset
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
                clock1.Visible = false;
                clock2.Visible = false;
            }
        }
        private void PanelBuild()
        {
            Label label = new Label();
            label.Name = "PanelLabel";
            label.Location = new Point(20, 20);
            label.AutoSize = true;
            //label.Text = lang.ask;
            label.Font = new Font("Segoe UI", 18, FontStyle.Regular, GraphicsUnit.Point);
            panel1.Controls.Add(label);
            //Tak
            Button yesButton = new Button();
            yesButton.Name = "PanelButton1";
            yesButton.Location = new Point(20, 70);
            yesButton.Size = new Size(130, 50);
            //yesButton.Text = lang.answers[0];
            yesButton.Font = new Font("Segoe UI", 18, FontStyle.Regular, GraphicsUnit.Point);
            yesButton.Click += new EventHandler(BackToMenu);
            panel1.Controls.Add(yesButton);
            //Nie
            Button noButton = new Button();
            noButton.Name = "PanelButton2";
            noButton.Location = new Point(170, 70);
            noButton.Size = new Size(130, 50);
            //noButton.Text = lang.answers[1];
            noButton.Font = new Font("Segoe UI", 18, FontStyle.Regular, GraphicsUnit.Point);
            noButton.Click += new EventHandler(PanelHide);
            panel1.Controls.Add(noButton);
            //Zapisz            
            Button saveButton = new Button();
            saveButton.Name = "PanelButton3";
            saveButton.Location = new Point(320, 70);
            saveButton.Size = new Size(200, 50);
            //saveButton.Text = lang.answers[2];
            saveButton.Font = new Font("Segoe UI", 18, FontStyle.Regular, GraphicsUnit.Point);
            saveButton.Click += new EventHandler(Save);
            panel1.Controls.Add(saveButton);
        }
        private void PanelUpdate()
        {
            panel1.Controls["PanelLabel"].Text = lang.ask;
            panel1.Controls["PanelButton1"].Text = lang.answers[0];
            panel1.Controls["PanelButton2"].Text = lang.answers[1];
            panel1.Controls["PanelButton3"].Text = lang.answers[2];
        }
        private void LangComboBoxSet()
        {
            LangList langlist = LangList.Load("langs\\List_languages.json");
            
            comboBox1.Items.AddRange(langlist.names);
            comboBox1.SelectedIndex = 1;
        }
        private void TimeComboBoxSet(int idx = 4)
        {
            comboBox2.Items.AddRange(new object[] {
            "5 "+lang.minute,
            "10 "+lang.minute,
            "20 "+lang.minute,
            "30 "+lang.minute,
            "60 "+lang.minute,
            "90 "+lang.minute,
            "Brak limitu"});

            //combo box 2
            comboBox2.SelectedIndex = idx;
        }
        private void SetLanguage(object sender, EventArgs e)
        {
            //potęcjalna optymalizacja
            LangList langlist = LangList.Load("langs\\List_languages.json");
            string path = langlist.paths[comboBox1.SelectedIndex];
            lang = Language.Load(path);
            //Ustawianie języka
            StartButton.Text = lang.play;
            LoadButton.Text = lang.load;
            Button3.Text = lang.options;
            ExitButton.Text = lang.exit;
            SaveButton.Text = lang.save;
            button4.Text = lang.back;
            button5.Text = lang.back;

            label4.Text = lang.lang;
            label5.Text = lang.time;
            int index = comboBox2.SelectedIndex;
            comboBox2.Items.Clear();
            TimeComboBoxSet(index);

            PanelUpdate();

            label1.Text = lang.author + "Piotr Majewski";
            label6.Text = lang.version + "0.4";
        }
        
        //Obudowa okienka
        private void Minimalize(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void FullScrean(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            //Dostosowanie wielkości
            MenuBackground.Size = new Size(this.Size.Height - 30, this.Size.Height - 30);
            label1.Location = new Point(this.Size.Width * 1050 / 1200, this.Size.Height * 650 / 675);
            label6.Location = new Point(this.Size.Width * 910 / 1200, this.Size.Height * 650 / 675);
            SaveButton.Location = new Point(this.Size.Width * 1050 / 1200, this.Size.Height * 70 / 675);
            SaveButton.Size = new Size(this.Size.Width * 150 / 1200, this.Size.Height * 120 / 675);
            button5.Location = new Point(this.Size.Width * 1050 / 1200, this.Size.Height * 185 / 675);
            button5.Size = new Size(this.Size.Width * 150 / 1200, this.Size.Height * 120 / 675);
            //Pasek zadań
            button2.Size = new Size(this.Size.Width, 30);
            icon1.Location = new Point(this.Size.Width - 30, 0);
            icon2.Location = new Point(this.Size.Width - 60, 0);
            icon3.Location = new Point(this.Size.Width - 90, 0);
            //Menu
            StartButton.Location = new Point(this.Size.Width * 730 / 1200, this.Size.Height * 60 / 675);
            StartButton.Size = new Size(this.Size.Width * 400 / 1200, this.Size.Height * 80 / 675);
            LoadButton.Location = new Point(this.Size.Width * 730 / 1200, this.Size.Height * 160 / 675);
            LoadButton.Size = new Size(this.Size.Width * 400 / 1200, this.Size.Height * 80 / 675);
            Button3.Location = new Point(this.Size.Width * 730 / 1200, this.Size.Height * 260 / 675);
            Button3.Size = new Size(this.Size.Width * 400 / 1200, this.Size.Height * 80 / 675);
            ExitButton.Location = new Point(this.Size.Width * 730 / 1200, this.Size.Height * 360 / 675);
            ExitButton.Size = new Size(this.Size.Width * 400 / 1200, this.Size.Height * 80 / 675);
            //Szachownica
            if (board != null)
                board.AdjustSize(this.Size);
            //Zegar
            label2.Location = new Point(this.Size.Width * 715 / 1200, this.Size.Height * 370 / 675);
            label2.Size = new Size(this.Size.Width * 200 / 1200, 40);
            if(clock1 != null && clock2 != null)
            {
                ClockSet();
            }
        }
        private void ClockSet()
        {
            clock1.Location = new Point(this.Size.Width * 730 / 1200, this.Size.Height * 290 / 675);
            clock2.Location = clock1.Location;
            clock1.Size = new Size(this.Size.Width * 155 / 1200, this.Size.Height * 65 / 675);
            clock2.Size = clock1.Size;
            clock1.Font = new Font("Segoe UI", this.Size.Width * 36 / 1200, FontStyle.Bold, GraphicsUnit.Point);
            clock2.Font = clock1.Font;
        }
        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Location = new Point(Location.X + e.X - deltaX, Location.Y + e.Y - deltaY);
        }
        private void MoveWindowBegin(object sender, MouseEventArgs e)
        {
            deltaX = MousePosition.X - Location.X;
            deltaY = MousePosition.Y - Location.Y;
        }
    }
}