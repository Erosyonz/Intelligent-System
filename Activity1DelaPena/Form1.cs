using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Activity1DelaPena
{
    public partial class Form1 : Form
    {
        VacuumEnvironment env = new VacuumEnvironment();

        //Randomize agent behavior
        //Agent agent = new SimpleReflexAgent();
        Agent agent = new MovementReflexAgent();

        int cx;
        int cy;


        public Form1()
        {
            InitializeComponent();
            cx = 0; cy = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "Creating 2x2 world\n";
            richTextBox1.Text += env;

            for(int i = 0; i < 10; i++)
            {
                var percept = env.Percept(agent);
                var action = agent.Program(percept);
                env.ExecuteAction(agent, action);

                var tup = percept as Tuple<int, int, bool>;
                string locationText = "(?, ?)";
                if(tup != null)
                {
                    locationText = $"({tup.Item1}, {tup.Item2})";
                }

                richTextBox1.AppendText($"Steps {i + 1}: Action = {action}| Location = {locationText} | Score = {agent.Performance}\r\n");


                //try
                //{
                //    Thread.Sleep(5000);
                //}
                //catch
                //{
                //    throw;
                //}
            }

            


            richTextBox1.AppendText("\r\n --- FInal 2x2 World --\r\n");
            richTextBox1.AppendText(env.ToString());
            richTextBox1.AppendText($"Final Performance  Score: {agent.Performance}\n\r");

        }

        
       
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var percept = env.Percept(agent);
            var tup = percept as Tuple<int, int, bool>;

            Graphics g = e.Graphics;
            //g.DrawArc(Pens.RoyalBlue, cx, cy, 20, 20, 0, 360);

            g.DrawArc(Pens.RoyalBlue, tup.Item2 * 100, tup.Item1 * 100, 20, 20, 0, 360);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            cx += 10;
            cy += 5;
            this.Refresh();
        }
    }
}
