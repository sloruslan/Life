using Life.Server.Infrastructure.Managers;
using Life.Server.Core.Domain;
using System.Drawing;

namespace Life.Clients.WinForms
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private int size;
        private Cell[,] field;
        private int rows;
        private int cols;
        private int plot;

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
            field = CalcGeneration.NextGeneration(field);
            Risov(field);
            
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void genFirstFieldBT_Click(object sender, EventArgs e)
        {
            size = (int)sizeUpDown.Value;

            rows = pictureBox1.Height / size;
            cols = pictureBox1.Width / size;

            plot = (int)numDensity.Value;

            field = CalcGeneration.StartGeneration<Cell>(cols, rows, plot);
            Risov(field);
        }

        /*private Cell[,] StartGeneration()
        {
            size = (int)sizeUpDown.Value;

            rows = pictureBox1.Height / size;
            cols = pictureBox1.Width / size;

            Cell[,] res = new Cell[cols, rows];

            Random random = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    res[x, y] = new Cell() { IsLife = (random.Next((int)numDensity.Value) == 0) };
                }
            }

            return res;

        }*/

        private void Risov(Cell[,] cells)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    g.FillRectangle(cells[x, y].Color, x * size, y * size, size, size);
                }
            }
            pictureBox1.Refresh();

        }
    }
}