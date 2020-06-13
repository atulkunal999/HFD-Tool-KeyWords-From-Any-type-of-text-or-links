using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace HFD_TOOL
{
    public partial class HFD_TOOL : Form
    {
        private string data = "";
        private string getdat = "";
        public HFD_TOOL()
        {
            InitializeComponent();
            saveFileDialog1.FileName = "Genrated0.txt";
            openFileDialog1.FileName = "Load Your File";
            labelp.Text = progressBar1.Value.ToString() + "%";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                getdat = File.ReadAllText(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog()==DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, getdat);
            }
        }

        private void inputtype_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void inputtype2_CheckedChanged(object sender, EventArgs e)
        {

        }
        public static string Removedata(string key)
        {
            return Regex.Replace(
                Regex.Replace(
                    Regex.Replace(
                        Regex.Replace(
                            Regex.Replace(
                                Regex.Replace(
                                    Regex.Replace(
                                        Regex.Replace(
                                            Regex.Replace(
                                                Regex.Replace(key, @"[@]", " "), @"[+]", " "), @"www", ""), @"[.]", " "), @"-", " "), @"_", " "), @"https://", ""), @"http://", ""), @"/", " "), @"[^a-zA-Z \n]+", "");
        }

        private void start_Click(object sender, EventArgs e)
        {
            // Start BackgroundWorker
            if (!backgroundWorker1.IsBusy)
            {
                label5.Text = "Working";
                backgroundWorker1.RunWorkerAsync();
                
            }
            else
            {
                MessageBox.Show("Worker is already Busy!");
            }
        }
        private string remdublicates(string data)
        {
            string[] maindata = Regex.Split(data," ");
            HashSet<string> set = new HashSet<string>();
            for (int i = 0; i < data.Length; i++)
            {
                try
                {
                    set.Add(maindata[i]);
                }
                catch(Exception ex)
                {
                    //nothing to do
                }
            }
            return string.Join(Environment.NewLine, set.ToArray());
        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            data = Removedata(textBox1.Text);
            string retdata = "";
            string[] splitted = data.Split('\n');
            for (int i = 0; i < splitted.Length; i++)
            {
                string[] spacesplit = splitted[i].Split(' ');
                for (int j = 0; j < spacesplit.Length; j++)
                {
                    if (spacesplit[j].Length > numericUpDown1.Value && spacesplit[j].Length < numericUpDown2.Value)
                        retdata = retdata + spacesplit[j] + " ";
                    if (backgroundWorker1.CancellationPending)
                    {
                        // Set Cancel property of DoWorkEventArgs object to true
                        e.Cancel = true;
                        // Reset progress percentage to ZERO and return
                        backgroundWorker1.ReportProgress(0);
                        return;
                    }
                }
                double progress = ((double)i / (double)splitted.Length)*100.0;
                backgroundWorker1.ReportProgress((int)progress);
            }
            e.Result = remdublicates(retdata);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            labelp.Text = e.ProgressPercentage.ToString() + "%";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                label5.Text = "Processing cancelled";
            }
            else if (e.Error != null)
            {
                label5.Text = e.Error.Message;
            }
            else
            {
                getdat = e.Result.ToString();
                getdat = Regex.Replace(getdat, @" ", "");
                progressBar1.Value = 100;
                labelp.Text = "100%";
                label5.Text = "Done";
                MessageBox.Show("Made By Atul Suthar\nDone", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void cancel_Click(object sender, EventArgs e)
        {
            // Cancel BackgroundWorker
            if (backgroundWorker1.IsBusy)
                backgroundWorker1.CancelAsync();
            else
            {
                MessageBox.Show("No Task in Progress");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] splitted = Regex.Split(getdat, @"\n");
            Array.Sort(splitted, StringComparer.InvariantCulture);
            getdat = string.Join(Environment.NewLine, splitted);
        }

        private void getdata_Click(object sender, EventArgs e)
        {
            textBox1.Text = getdat;
            MessageBox.Show("Getting Data Succesful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] splitted = Regex.Split(getdat, @"\n");
            Random rand = new Random();
            for(int i=0;i<splitted.Length;i++)
            {
                int val = (rand.Next() + splitted.Length) % splitted.Length;
                string one = splitted[i];
                splitted[i] = splitted[val];
                splitted[val] = one;
            }
            getdat = string.Join(Environment.NewLine, splitted);
        }

        private void loaddata_Click(object sender, EventArgs e)
        {
            getdat = textBox1.Text;
            MessageBox.Show("Loading Data Succesful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            string[] spli = Regex.Split(text, @"\n");
            linesno.Text = spli.Length.ToString();
        }
    }
}
