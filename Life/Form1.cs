using System.Drawing;

namespace Life
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private int size;
        private bool[,] field;
        private int rows;
        private int cols;

        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (timer1.Enabled)
            {
                return;
            }

            size = (int)sizeUpDown.Value;

            rows = pictureBox1.Height / size;
            cols = pictureBox1.Width / size;
            field = new bool[cols, rows];

            Random random = new Random();
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    field[i, j] = random.Next((int)numDensity.Value) == 0;
                }
            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);

            timer1.Start();
        }


        private void StopGame()
        {
            timer1.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void NextGeneration()
        {
            g.Clear(Color.Black);

            var newFiled = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var countSosedi = SosediCount(x,y);
                    var isLife = field[x, y];
                    
                    if (isLife)
                    {
                        if ((countSosedi == 2) | (countSosedi == 3))
                        {
                            newFiled[x, y] = true;
                        }
                        else newFiled[x, y] = false;
                    }
                    else
                    {
                        if (countSosedi == 3)
                        {
                            newFiled[x, y] = true;
                        }
                        else newFiled[x, y] = false;
                    }   
                    
                    if (isLife)
                    {
                        g.FillRectangle(Brushes.Green, x * size, y * size, size, size);
                    }
                }
 
            }
            field = newFiled;
            pictureBox1.Refresh();

        }

        private int SosediCount(int x, int y)
        {
            var res = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = x + i;
                    var row = y + j;
                    
                    if ((col < 0) || (row < 0) || (col >= cols) || (row >= rows) || ((col == x) && (row == y)))
                    {
                        continue;
                    }

                    if (field[col, row]) res++;
                }
            }
            return res;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            StopGame();
        }
    }
}