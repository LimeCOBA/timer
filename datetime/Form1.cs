using System;
using System.Windows.Forms;
using System.Threading;

namespace datetime
{
    public partial class Form1 : Form
    {
        bool ShowState = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateTime()
        {
            label1.Text = Program.timer.Time.ToString(@"hh\:mm\:ss\.fff");
        }

        private static void PrintIntoLabel(TimeSpan time, Label ol)
        {
            ol.Text = time.ToString(@"hh\:mm\:ss\.fff");
        }

        private static void f(string time, Label ol)
        {
            Console.WriteLine(time);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Time;

            UpdateTime();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Program.timer.Time = dateTimePicker1.Value.TimeOfDay;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.timer.OutputLabel = label1;

            if (!Program.timer.isStop)// && Program.timer.)
            {
                Program.timer.Handler -= PrintIntoLabel;
                Program.timer.Stop();
                button1.Text = "СТАРТ";
            }
            else
            {
                Program.timer.Handler += PrintIntoLabel;
                Program.timer.Start();
                button1.Text = "СТОП";
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.timer.AbortThread();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Program.timer.isStop)
            {
                Program.timer.Handler -= PrintIntoLabel;
                Program.timer.Pause();
                button1.Text = "СТАРТ";
            }
            else if (Program.timer.State == ThreadState.Suspended)
            {
                Program.timer.Handler += PrintIntoLabel;
                Program.timer.Start();
                button1.Text = "СТОП";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.timer.Time += DateTime.Now.TimeOfDay;
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            Program.timer.Interval = new TimeSpan(0, 0, 0, 0, int.Parse(maskedTextBox1.Text));
        }
        
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowState = !ShowState;

            if (ShowState)
                label2.Text = $"{Program.timer.State}";
            else
                label2.Text = String.Empty;
        }

        private void button4_Click(object sender, EventArgs e)
        {            
            Program.timer.Time = new TimeSpan(0);
            UpdateTime();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pictureBox1.Visible = !pictureBox1.Visible;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Program.timer.Time = new TimeSpan(0, 0, 0, 14, 880);
            UpdateTime();
        }
    }
}
