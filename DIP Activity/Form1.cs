using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DIP_Activity
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed;
        bool[,] isCoin;
        bool[,] solid;
        int[,] gray;
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }

        int n5c, n10c, n25c, n1p, n5p;

        private void classifyCoins()
        {
            n5c = n10c = n25c = n1p = n5p = 0;

            foreach (CoinInfo c in coins)
            {
                if (c.HoleArea >= 20) n5c++;    // holed coin = 5 centavo
                else if (c.Area < 1300) n10c++;
                else if (c.Area < 1850) n25c++;
                else if (c.Area < 2450) n1p++;
                else n5p++;
            }

            double total = n5c * 0.05 + n10c * 0.10 + n25c * 0.25 + n1p * 1.00 + n5p * 5.00;

            richTextBox1.Text =
                "5 centavo:  " + n5c + Environment.NewLine +
                "10 centavo: " + n10c + Environment.NewLine +
                "25 centavo: " + n25c + Environment.NewLine +
                "1 peso:     " + n1p + Environment.NewLine +
                "5 peso:     " + n5p + Environment.NewLine +
                "Total coins: " + coins.Count + Environment.NewLine +
                "Total value: P" + total.ToString("F2");

            label2.Text = "P" + total.ToString("F2");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            isCoin = new bool[loaded.Width, loaded.Height];
            binaryTool();
            fillHoles();
            labelCoins();
            classifyCoins();

            Bitmap preview = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                    preview.SetPixel(x, y, solid[x, y] ? Color.Black : Color.White);
            pictureBox1.Image = preview;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (loaded == null) return;

            Bitmap processed = new Bitmap(loaded.Width, loaded.Height);

            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);

                    // Quadrant 1 (Bottom-Right): Invert Colors
                    if (x >= 400 && y >= 300)
                    {
                        processed.SetPixel(x, y, Color.FromArgb(pixel.A, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                    }
                    // Quadrant 2 (Bottom-Left): Grayscale
                    else if (x < 400 && y >= 300)
                    {
                        int gray = (pixel.R + pixel.G + pixel.B) / 3;
                        processed.SetPixel(x, y, Color.FromArgb(pixel.A, gray, gray, gray));
                    }
                    // Quadrant 3 (Top-Left): Original Copy
                    else if (x < 400 && y < 300)
                    {
                        processed.SetPixel(x, y, pixel);
                    }
                    // Quadrant 4 (Top-Right): Vertical Mirror of Bottom-Right Quadrant
                    else
                    {
                        int mirrory = 599 - y; // Corrected mirror formula (maps 0..299 to 599..300)
                        Color bottomPixel = loaded.GetPixel(x, mirrory);
                        processed.SetPixel(x, y, bottomPixel);
                    }
                }
            }
        }

        private void binaryTool()
        {
            processed = new Bitmap(loaded.Width, loaded.Height);

            Color pixel;
            int ave;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    ave = (pixel.R + pixel.G + pixel.B) / 3;
                    if (ave < 180)
                    {
                        processed.SetPixel(x, y, Color.Black);
                        isCoin[x, y] = true;
                    }
                    else
                    {
                        processed.SetPixel(x, y, Color.White);
                        isCoin[x, y] = false;
                    }

                }
            }

        }
        private void fillHoles()
        {
            int w = loaded.Width, h = loaded.Height;
            bool[,] isBackground = new bool[w, h];
            Queue<Point> queue = new Queue<Point>();

            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };

            if (!isCoin[0, 0])
            {
                isBackground[0, 0] = true;
                queue.Enqueue(new Point(0, 0));
            }

            while (queue.Count > 0)
            {
                Point p = queue.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    int nx = p.X + dx[i];
                    int ny = p.Y + dy[i];
                    
                    if (nx < 0 || ny < 0 || nx >= w || ny >= h) continue;   
                    if (isCoin[nx, ny] || isBackground[nx, ny]) continue;   

                    isBackground[nx, ny] = true;
                    queue.Enqueue(new Point(nx, ny));
                }
            }

            solid = new bool[w, h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    solid[x, y] = !isBackground[x, y];
        }



        class CoinInfo
        {
            public int Area;       
            public int HoleArea;   
            public int MinX, MaxX, MinY, MaxY;
            public int Width { get { return MaxX - MinX + 1; } }
            public int Height { get { return MaxY - MinY + 1; } }
        }

        List<CoinInfo> coins = new List<CoinInfo>();

        private void labelCoins()
        {
            int w = loaded.Width, h = loaded.Height;
            bool[,] visited = new bool[w, h];
            coins.Clear();

            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (!solid[x, y] || visited[x, y]) continue;

                    CoinInfo c = new CoinInfo();
                    c.MinX = c.MaxX = x;
                    c.MinY = c.MaxY = y;

                    Queue<Point> queue = new Queue<Point>();
                    visited[x, y] = true;
                    queue.Enqueue(new Point(x, y));

                    while (queue.Count > 0)
                    {
                        Point p = queue.Dequeue();

                        c.Area++;
                        if (!isCoin[p.X, p.Y]) c.HoleArea++;

                        if (p.X < c.MinX) c.MinX = p.X;
                        if (p.X > c.MaxX) c.MaxX = p.X;
                        if (p.Y < c.MinY) c.MinY = p.Y;
                        if (p.Y > c.MaxY) c.MaxY = p.Y;

                        for (int i = 0; i < 4; i++)
                        {
                            int nx = p.X + dx[i];
                            int ny = p.Y + dy[i];

                            if (nx < 0 || ny < 0 || nx >= w || ny >= h) continue;   
                            if (!solid[nx, ny] || visited[nx, ny]) continue;        

                            visited[nx, ny] = true;
                            queue.Enqueue(new Point(nx, ny));
                        }
                    }

                    coins.Add(c);
                }
            }
        }

    }
}
