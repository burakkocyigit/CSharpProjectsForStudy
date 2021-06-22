using NavigateMe.Core.Abstract;
using NavigateMe.Places;
using NavigateMe.Places.Stores;
using NavigateMe.Places.Ways;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NavigateMe.UI.WinForm
{
    public partial class Form1 : Form
    {
        List<Way> ways = new List<Way>();
        ComboBox comboBox = new ComboBox();
        ComboBox comboBox2 = new ComboBox();
        Label label = new Label();

        Button button = new Button();
        Button bFloor1 = new Button();
        Button bFloor2 = new Button();
        //kendi toollarımızıda formun constructorunda initialize ettim aslında bunu partial class ta da yapabilirdim daha temiz olurdu fakat design file ının bozulmaması için burada yaptım.
        public Form1()
        {
            InitializeComponent();
            Ktn ktn = new Ktn();
            Lcw lcw = new Lcw();
            Stb stb = new Stb();
            Bgr bgr = new Bgr();
            Elevator elevator = new Elevator();
            Stairs stairs = new Stairs();
            Kiosk kiosk1 = new Kiosk(1);
            Kiosk kiosk2 = new Kiosk(2);
            Kiosk kiosk3 = new Kiosk(3);

            ways.Add(ktn);
            ways.Add(lcw);
            ways.Add(stb);
            ways.Add(bgr);
            ways.Add(elevator);
            ways.Add(stairs);
            ways.Add(kiosk1);
            ways.Add(kiosk2);
            ways.Add(kiosk3);

            comboBox.Location = new Point(panel1.Width * 3 / 2 - 25, 100);
            comboBox.Items.Add(kiosk1);
            comboBox.Items.Add(kiosk2);
            comboBox.Items.Add(kiosk3);
            comboBox.SelectedIndex = 0;
            panel1.Controls.Add(comboBox);

            comboBox2.Location = new Point(panel1.Width * 3 / 2 - 25, 200);
            comboBox2.Items.Add(ktn);
            comboBox2.Items.Add(lcw);
            comboBox2.Items.Add(stb);
            comboBox2.Items.Add(bgr);
            comboBox2.SelectedIndex = 0;
            panel1.Controls.Add(comboBox2);

            button.Location = new Point(panel1.Width * 3 / 2, 250);
            button.Size = new Size(75, 75);
            button.Text = "YOLU GÖSTER";
            button.Click += Button_Click;
            panel1.Controls.Add(button);

            bFloor1.Location = new Point(panel1.Width * 3 / 2 - 200, 200);
            bFloor1.Size = new Size(75, 75);
            bFloor1.Text = "1.KAT";
            bFloor1.Click += BFloor1_Click;
            panel1.Controls.Add(bFloor1);

            bFloor2.Location = new Point(panel1.Width * 3 / 2 - 200, 100);
            bFloor2.Size = new Size(75, 75);
            bFloor2.Text = "2.KAT";
            bFloor2.Click += BFloor2_Click;
            panel1.Controls.Add(bFloor2);


            label.Location = new Point(panel1.Width * 3 / 2 - 25, 350);
            label.AutoSize = true;
            panel1.Controls.Add(label);
        }
        //kat 2 yi göstermek için kullanılıyor
        private void BFloor2_Click(object sender, EventArgs e)
        {
            Draw(2);
        }
        //kat biri göstermek için kullanılıyor
        private void BFloor1_Click(object sender, EventArgs e)
        {
            Draw(1);
        }
        //bu method buttonun eventine bağlı bi şekilde ascii tablodan yararlanarak kurduğumuz algoritma ile gidilmesi gereken rotayı label a yazdırıyor
        private void Button_Click(object sender, EventArgs e)
        {
            label.Text = string.Empty;
            Tuple<Point, Point> targetPath = ((Kiosk)comboBox.SelectedItem).FindShortestPath((Node)comboBox2.SelectedItem);
            for (int j = 0; j <= Math.Abs(targetPath.Item1.Y); j++)
            {
                for (int i = (j == 0) ? 0 : Math.Abs(targetPath.Item1.X); i <= Math.Abs(targetPath.Item1.X); i++)//algoritma tekrar etmesin diye i' ye atama yaparken ternary if ekledim burada j'nin sıfır olmadığı durumlarda i kendini sıfırlamayacak
                {
                    label.Text += ((char)(((Kiosk)comboBox.SelectedItem).Column + ((targetPath.Item1.X > 0) ? i : -i))).ToString() + (((Kiosk)comboBox.SelectedItem).Row + ((targetPath.Item1.Y > 0) ? j : -j)) + ((Math.Abs(targetPath.Item1.Y) != j) ? "," : "");
                    if (((Node)comboBox2.SelectedItem).Floor == 2 && j == Math.Abs(targetPath.Item1.Y))
                    {
                        string secondFloor = string.Empty;
                        for (int y = 0; y <= Math.Abs(targetPath.Item2.Y); y++)
                        {
                            for (int x = (y == 0) ? 0 : Math.Abs(targetPath.Item2.X); x <= Math.Abs(targetPath.Item2.X); x++)
                            {
                                secondFloor += ((char)(label.Text.Split(',').Last().ToCharArray()[0] + ((targetPath.Item2.X > 0) ? x : -x))).ToString() + ((int.Parse(label.Text.Split(',').Last().ToCharArray()[1].ToString()) + ((targetPath.Item2.Y > 0) ? y : -y)) + ((Math.Abs(targetPath.Item2.Y) != y) ? "," : ""));
                            }
                        }
                        label.Text += (label.Text.Split(',').Last() == "E5") ? "\nAsansörü kullanın\n" : "\nMerdiveni kullanın\n";
                        label.Text += secondFloor;
                    }
                }
            }
            ShowPath(targetPath, (Kiosk)comboBox.SelectedItem);
        }
        //bu method ekranda ızgara üzerinde gidilmesi gereken yolu turkuaz renkle çiziyor
        public async void ShowPath(Tuple<Point, Point> tuple, Node node)
        {
            Draw(1);
            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Aqua, 7);

            Point start = new Point(((panel1.Height - 100) / 11 * (node.Column - 65) + 70), (panel1.Height - 100) / 11 * (node.Row - 1) + 50);


            for (int i = 0; i <= Math.Abs(tuple.Item1.X); i++)
            {
                Point X1 = start;
                Point X2 = new Point(((panel1.Height - 100) / 11 * (node.Column - 65 + ((tuple.Item1.X >= 0) ? i : -i)) + 70), (panel1.Height - 100) / 11 * (node.Row - 1) + 50);
                g.DrawLine(p, X1, X2);
                await Task.Delay(500);
                if (Math.Abs(tuple.Item1.X) == i)
                    start = X2;
            }
            for (int i = 0; i <= Math.Abs(tuple.Item1.Y); i++)
            {
                Point Y1 = start;
                Point Y2 = new Point(start.X, (panel1.Height - 100) / 11 * (node.Row - 1 + ((tuple.Item1.Y >= 0) ? i : -i)) + 50);
                g.DrawLine(p, Y1, Y2);
                await Task.Delay(500);
                if (Math.Abs(tuple.Item1.Y) == i)
                    start = Y2;
            }
            await Task.Delay(1500);
            if (((Node)comboBox2.SelectedItem).Floor == 2)
            {
                Draw(2);


                for (int i = 0; i <= Math.Abs(tuple.Item2.X); i++)
                {
                    Point X1 = start;
                    Point X2 = new Point(((panel1.Height - 100) / 11 * (node.Column - 65 + tuple.Item1.X + ((tuple.Item2.X >= 0) ? i : -i)) + 70), (panel1.Height - 100) / 11 * (node.Row + tuple.Item1.Y - 1) + 50);
                    g.DrawLine(p, X1, X2);
                    await Task.Delay(500);
                    if (Math.Abs(tuple.Item2.X) == i)
                        start = X2;
                }
                for (int i = 0; i <= Math.Abs(tuple.Item2.Y); i++)
                {
                    Point Y1 = start;
                    Point Y2 = new Point(start.X, (panel1.Height - 100) / 11 * (node.Row + tuple.Item1.Y - 1 + ((tuple.Item2.Y >= 0) ? i : -i)) + 50);
                    g.DrawLine(p, Y1, Y2);
                    await Task.Delay(500);
                }
            }
        }
        //method her çalıştığında paneli temizleyip daha sonrasında istenen katı çiziyor
        public void Draw(int floor)
        {
            panel1.CreateGraphics().Clear(Color.White);
            for (int i = 0; i <= 9; i++)
            {
                panel1.Controls.RemoveByKey(i.ToString());
            }

            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Black, 7);
            for (int j = 0; j < 12; j++)
            {
                for (int i = 0; i < 12; i++)
                {                    
                    Point Y1 = new Point(((panel1.Height - 100) / 11 * i + 70), (panel1.Height - 100) / 11 * j + 50);//kontrollerin boyutlarıyla hizalama yapıldığı için uygulama her boyuta göre kendini ayarlabilmektedir
                    Point Y2 = new Point(((panel1.Height - 100) / 11 * i + 70), (panel1.Height - 100) / 11 * (11 - j) + 50);
                    Point X1 = new Point(((panel1.Height - 100) / 11 * j + 70), (panel1.Height - 100) / 11 * i + 50);
                    Point X2 = new Point(((panel1.Height - 100) / 11 * (11 - j) + 70), (panel1.Height - 100) / 11 * i + 50);
                    g.DrawLine(p, Y1, Y2);
                    g.DrawLine(p, X1, X2);
                    if (j == 0)
                        g.DrawString(((char)(i + 65)).ToString(), new Font(new FontFamily("Calibri"), 20), new SolidBrush(Color.Black), new Point(Y1.X - 8, Y1.Y - 40));
                    if (i == 0)
                        g.DrawString((12 - j).ToString(), new Font(new FontFamily("Calibri"), 20), new SolidBrush(Color.Black), new Point(Y2.X - 40, Y2.Y - 18));
                    foreach (Way item in ways)
                    {
                        if (item.Row == j + 1 && item.Column == i + 65)
                        {
                            if (item.GetType().Name == "Lcw" && floor == 1)
                            {
                                panel1.Controls.Add(new Button() { Size = new Size((panel1.Height - 100) / 11 - 10, (panel1.Height - 100) / 11 - 10), ImageList = ımageList1, Location = new Point(Y1.X + 5, Y1.Y + 5), ImageKey = "Lcw.png", Name = "1" });
                            }
                            else if (item.GetType().Name == "Ktn" && floor == 1)
                            {
                                panel1.Controls.Add(new Button() { Size = new Size((panel1.Height - 100) / 11 - 10, (panel1.Height - 100) / 11 - 10), ImageList = ımageList1, Location = new Point(Y1.X + 5, Y1.Y + 5), ImageKey = "Ktn.jpg", Name = "2" });
                            }
                            else if (item.GetType().Name == "Stb" && floor == 1)
                            {
                                panel1.Controls.Add(new Button() { Size = new Size((panel1.Height - 100) / 11 - 10, (panel1.Height - 100) / 11 - 10), ImageList = ımageList1, Location = new Point(Y1.X + 5, Y1.Y + 5), ImageKey = "Stb.jpg", Name = "3" });
                            }
                            else if (item.GetType().Name == "Bgr" && floor == 2)
                            {
                                panel1.Controls.Add(new Button() { Size = new Size((panel1.Height - 100) / 11 - 10, (panel1.Height - 100) / 11 - 10), ImageList = ımageList1, Location = new Point(Y1.X + 5, Y1.Y + 5), ImageKey = "Bgr.png", Name = "4" });
                            }
                            else if (item.GetType().Name == "Elevator")
                            {
                                panel1.Controls.Add(new Button() { Size = new Size((panel1.Height - 100) / 11 - 10, (panel1.Height - 100) / 11 - 10), ImageList = ımageList1, Location = new Point(Y1.X + 5, Y1.Y + 5), ImageKey = "Elevator.jpg", Name = "8" });
                            }
                            else if (item.GetType().Name == "Stairs")
                            {
                                panel1.Controls.Add(new Button() { Size = new Size((panel1.Height - 100) / 11 - 10, (panel1.Height - 100) / 11 - 10), ImageList = ımageList1, Location = new Point(Y1.X + 5, Y1.Y + 5), ImageKey = "Stairs.png", Name = "9" });
                            }
                            else if (item.GetType().Name == "Kiosk" && ((Kiosk)item).Number == 1 && floor == 1)
                            {
                                panel1.Controls.Add(new Button() { Size = new Size((panel1.Height - 100) / 11 - 10, (panel1.Height - 100) / 11 - 10), ImageList = ımageList1, Location = new Point(Y1.X + 5, Y1.Y + 5), ImageKey = "Kiosk.jpg", Name = "5" });
                            }
                            else if (item.GetType().Name == "Kiosk" && ((Kiosk)item).Number == 2 && floor == 1)
                            {
                                panel1.Controls.Add(new Button() { Size = new Size((panel1.Height - 100) / 11 - 10, (panel1.Height - 100) / 11 - 10), ImageList = ımageList1, Location = new Point(Y1.X + 5, Y1.Y + 5), ImageKey = "Kiosk.jpg", Name = "6" });
                            }
                            else if (item.GetType().Name == "Kiosk" && ((Kiosk)item).Number == 3 && floor == 1)
                            {
                                panel1.Controls.Add(new Button() { Size = new Size((panel1.Height - 100) / 11 - 10, (panel1.Height - 100) / 11 - 10), ImageList = ımageList1, Location = new Point(Y1.X + 5, Y1.Y + 5), ImageKey = "Kiosk.jpg", Name = "7" });
                            }
                        }
                    }
                }
            }
        }
        // formu küçültüp büyüttüğümüzde çizilen ve panele eklenen controllerin yerlerinin değiştirilmesini sağlıyor
        private async void Form1_Resize(object sender, EventArgs e)
        {
            if (panel1.Width > 1000)
            {
                comboBox.Location = new Point(panel1.Width * 2 / 3 - 25, 100);
                comboBox2.Location = new Point(panel1.Width * 2 / 3 - 25, 200);
                button.Location = new Point(panel1.Width * 2 / 3, 250);
                bFloor1.Location = new Point(panel1.Width * 2 / 3 - 200, 200);
                bFloor2.Location = new Point(panel1.Width * 2 / 3 - 200, 100);
                label.Location = new Point(panel1.Width * 2 / 3 - 25, 350);
            }
            else
            {
                comboBox.Location = new Point(panel1.Width - 200, 100);
                comboBox2.Location = new Point(panel1.Width - 200, 200);
                button.Location = new Point(panel1.Width - 175, 250);
                bFloor1.Location = new Point(panel1.Width - 300, 200);
                bFloor2.Location = new Point(panel1.Width - 300, 100);
                label.Location = new Point(panel1.Width - 200, 350);
            }
            await Task.Delay(100);
            Draw(1);
        }
    }
}
