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

namespace demo23
{
    public partial class Form1 : Form
    {
        private volatile bool CancelWork = false;
        private ManualResetEvent manualReset = new ManualResetEvent(false);
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            manualReset.Reset();
            Task.Run(() =>
            {
                RunWork();
            });
        }

        private void RunWork()
        {
            for (int i = 0; i < 100; i++)
            {
                this.Invoke(new Action(() =>
                {
                    this.progressBar.Value = i;
                }));

                Thread.Sleep(1000);

                if (manualReset.WaitOne(1000))
                {
                    break;
                }
            }

            this.Invoke(new Action(() =>
            {
                this.btnStart.Enabled = true;
                this.btnStop.Enabled = false;
            }));
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            manualReset.Reset();
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            Parallel.For(1, 100, x =>
            {
                bool b = IsPrimeNumber(x);
                Console.WriteLine($"{x}:{b}");
            });
        }

        private static bool IsPrimeNumber(int number)
        {
            if (number < 1)
            {
                return false;
            }

            if (number == 1 && number == 2)
            {
                return true;
            }

            for (int i = 2; i < number; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
