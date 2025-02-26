using System.Windows.Forms;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;

namespace SolarSystem
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer;
        private float mercury = 0;
        private float venus = 0;
        private float earth = 0;
        private float mars = 0;
        private float moon = 0;
        private PointF[] stars;
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();

            this.Size = new Size(800, 800);
            this.Paint += new PaintEventHandler(SolarSystemDraw);
            this.Resize += (sender, e) => { GenerateStars(); this.Invalidate(); };

            GenerateStars();

            // Draw stars
            int numStars = 100;
            stars = new PointF[numStars];
            for (int i = 0; i < numStars; i++)
            {
                stars[i] = new PointF(random.Next(0, this.ClientSize.Width), random.Next(0, this.ClientSize.Height));
            }

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 50;
            timer.Tick += (sender, e) => { this.Invalidate(); };
            timer.Start();
        }

        private void GenerateStars()
        {
            stars = new PointF[100];
            for (int i = 0; i < 100; i++)
            {
                stars[i] = new PointF(random.Next(0, this.ClientSize.Width), random.Next(0, this.ClientSize.Height));
            }
        }

        private void SolarSystemDraw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            DrawStars(g);

            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            // Star Sun
            int sunSize = 50;
            g.FillEllipse(Brushes.Yellow, centerX - sunSize / 2, centerY - sunSize / 2, sunSize, sunSize);

            // Planets
            PointF earthPos = DrawPlanet(g, 140, earth, 15, "Earth", Brushes.Blue, centerX, centerY);

            DrawPlanet(g, 60, mercury, 10, "Mercury", Brushes.Gray, centerX, centerY);
            DrawPlanet(g, 100, venus, 15, "Venus", Brushes.Orange, centerX, centerY);
            DrawPlanet(g, 180, mars, 10, "Mars", Brushes.Red, centerX, centerY);
            DrawMoon(g, 25, moon, 6, "Moon", Brushes.White, (int)earthPos.X, (int)earthPos.Y);

            if (PlanetAlignment())
            {
                string text = "Ultimo alinhamento planetário: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                g.DrawString(text, new Font("Arial", 12), Brushes.White, 10, 20);
            } else
            {
                g.DrawString("Aguardando alinhamento planetário...", new Font("Arial", 12), Brushes.White, 10, 20);
            }

            mercury += 0.02f;
            venus += 0.015f;
            earth += 0.01f;
            mars += 0.005f;
            moon += 0.05f;
        }

        private PointF DrawPlanet(Graphics g, float distance, float angle, float size, string name, Brush color, int centerX, int centerY)
        {
            float x = 400 + distance * (float)System.Math.Cos(angle);
            float y = 400 + distance * (float)System.Math.Sin(angle);
            g.FillEllipse(color, x - size / 2, y - size / 2, size, size);

            g.DrawString(name, new Font("Arial", 8), Brushes.White, x - 15, y - 20);

            return new PointF(x, y);
        }

        private void DrawMoon(Graphics g, float distance, float angle, float size, string name, Brush color, int centerX, int centerY)
        {
            float x = centerX + distance * (float)System.Math.Cos(angle);
            float y = centerY + distance * (float)System.Math.Sin(angle);
            g.FillEllipse(color, x, y, size, size);
            g.DrawString(name, new Font("Arial", 8), Brushes.White, x - 10, y - 15);
        }

        private void DrawStars(Graphics g)
        {
            foreach (PointF star in stars)
            {
                int size = random.Next(1, 4);
                g.FillEllipse(Brushes.White, star.X, star.Y, size, size);
            }
        }

        private bool PlanetAlignment()
        {
            float tolerance = 0.1f;
            float[] angles = { mercury, venus, earth, mars, moon };
            Array.Sort(angles);

            for (int i = 1; i < angles.Length; i++)
            {
                if (Math.Abs(angles[i] - angles[i - 1]) > tolerance)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
