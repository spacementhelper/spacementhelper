using SpacemeshHelper.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SpacemeshHelper.F2;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Timer = System.Windows.Forms.Timer;

namespace SpacemeshHelper
{
    public partial class UC : UserControl
    {
        FileSystemWatcher watcher;
        public Timer timer1;

        public Process _process;
        private CancellationTokenSource _cancellationTokenSource;

        public void KillProcess()
        {
            try
            {
                _process.Kill();
                timer1.Stop();
                timer1.Dispose();
                watcher.EnableRaisingEvents = false;
                watcher.Dispose(); 
                setstate("er");
            }
            catch (Exception)
            {

                
            }
           

        }

        string info = "";
        public UC(string info)
        {
            this.info = info;
            InitializeWatcher();
            InitializeTimer();
            InitializeComponent();

            //  timer1.Start(); 
        }
        private void InitializeTimer()
        {
            timer1 = new Timer();
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;

        }
        private void InitializeWatcher()
        {
         
            watcher = new FileSystemWatcher();
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

        
            watcher.Created += Watcher_Created;
          
        }
        private void label11_Click(object sender, EventArgs e)
        {

        }
        void setCount()
        {
            try
            {
                int ff = int.Parse(this.textBox1.Text.Trim());
                int tf = int.Parse(this.textBox2.Text.Trim());
                int count = (tf - ff) + 1;
                this.label2.Text = count.ToString();
            }
            catch (Exception)
            {


            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            setCount();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            setCount();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        string cardID = string.Empty;
        string fromfile = "0";
        string tofile = "0";
        private void UC_Load(object sender, EventArgs e)
        {
            cardID = info.Split('|')[0];
            fromfile = info.Split('|')[1];
            tofile = info.Split('|')[2];
            this.label1.Text = cardID;
            this.textBox1.Text = fromfile;
            this.textBox2.Text = tofile;
            this.button1.Enabled = false;
            this.button2.Enabled = false;
            f2 = new F2(info);
            f2.Show();
            f2.Hide();
        }

        private async void button1_Click(object sender, EventArgs e)
        {


            switch (button1.Text)
            {

                case "Start":
                    try
                    {
                        await Task.Run(() =>
                        {
                            RunEx(args);
                            this.button1.Text = "Stop";
                        });
                    }
                    catch (Exception)
                    {


                    }


                    break;
                case "Stop":

                    await _StopEx();
                    this.button1.Text = "Start";
                    break;
                case "Restart":
                    try
                    {
                        await _StopEx();

                        RunEx(args);

                    }
                    catch (Exception)
                    {


                    }

                    RunEx("");
                    break;
                default:
                    break;
            }


        }


        async

        Task
_StopEx()
        {
            try
            {

                await Task.Run(() =>
                {
                    setstate("er");
                    _cancellationTokenSource.Cancel();
                    _process.StandardInput.WriteLine("exit");
                    _process.Kill();
                    _process.WaitForExit();
                });
            }
            catch (Exception)
            {


            }

        }
        void setstate(string str)
        {

            this.BeginInvoke((MethodInvoker)delegate
            {
                if (str == "ok")
                {
                    this.pictureBox1.Image = Resources.OK;
                }
                if (str == "er")
                {
                    this.pictureBox1.Image = Resources.ER;
                }
            });




        }

        string args = "";
        public string FilePath = "";
        public void SomeMethod(string id, string conmmid, string filepath, string creatsize)
        {
            oldTime = DateTime.Now;



            FilePath = filepath;
            var _creatsize = int.Parse(creatsize) / 64;
            this.BeginInvoke((MethodInvoker)delegate
            {
                this.textBox1.Enabled = false;
                this.textBox2.Enabled = false;
                this.button1.Enabled = true;
                this.button2.Enabled = true;


            });
            fromfile = this.textBox1.Text;
            tofile = this.textBox2.Text;
            string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            args = $"  -provider {cardID.Split('-')[0]} -id {id} -commitmentAtxId {conmmid}  -numUnits {_creatsize} -datadir  {filepath} -fromFile {fromfile}  tofile {tofile}";
            watcher.Path = FilePath;
            watcher.EnableRaisingEvents = true;
            RunEx(args);
            checkper();

        }

        public void RunEx(string command)
        {
            scanfile();
            string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            var name = $"{applicationPath}\\POST_{SpaceHelper.Postver}\\postcli.exe";
            File.AppendAllText($"{cardID}out.txt", $"{name}  {args}\r\n");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {


                FileName = name,
                Arguments = command,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _process = new Process { StartInfo = startInfo };

            _process.Start();

            _cancellationTokenSource = new CancellationTokenSource();

            Task outputTask = MonitorOutputAsync(_process, _cancellationTokenSource.Token);
            Task errorTask = MonitorErrorAsync(_process, _cancellationTokenSource.Token);


            // _process.StandardInput.WriteLine(command);
        }

        private async Task MonitorOutputAsync(Process process, CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                string line = await process.StandardOutput.ReadLineAsync();
                if (line != null)
                {
                    // setstate("ok");
                    if (f2 != null)
                    {
                        f2.Invoke(new DisplayOutputWithMaxLinesDelegate(f2.DisplayOutputWithMaxLines), line, 100);

                    }


                }
            }
        }


        private async Task MonitorErrorAsync(Process process, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                string line = await process.StandardError.ReadLineAsync();
                if (line != null)
                {
                    if (f2 != null)
                    {
                        f2.Invoke(new DisplayOutputWithMaxLinesDelegate(f2.DisplayOutputWithMaxLines), line, 100);

                    }

                }
            }
        }


        F2 f2;

        private void button2_Click(object sender, EventArgs e)
        {
            if (f2 != null)
            {
                f2.Show();
            }
            else
            {
                f2 = new F2(info);
                f2.Show();
            }


        }


        //private void Watcher_Created(object sender, FileSystemEventArgs e)
        //{
        //    f2.DisplayOutputWithMaxLines(e.Name,100);
        //}

        static Dictionary<string, DateTime> fileCreationTimes = new Dictionary<string, DateTime>();
        static Dictionary<string, long> fileSizes = new Dictionary<string, long>();
        static string previousFileName = null;
        //static int startNumber = 0; 
        //static int endNumber = 0; 
        //  static int FileCount = 0;
        static long fileSizeBytes = 4294967296; // 文件大小 4096MB


        public DateTime oldTime;
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {

            //

            //   f2.DisplayOutputWithMaxLines("Runtime"+RunTime(oldTime).ToString(),100);





            string fileName = e.Name;
            if (IsWithinRange(fileName))
            {


                

                var numberPart = int.Parse(Regex.Match(fileName, @"\d+").Value);

                //  f2.DisplayOutputWithMaxLines($"true  filename {numberPart}  form {fromfile} to {tofile}", 100);

                var N_countNum = N_count(numberPart);

                var elapsedTimeSeconds = RunTime(oldTime);

                // f2.DisplayOutputWithMaxLines(elapsedTimeSeconds.ToString(), 100);
                double speedMBPerSecond = 0;
                try
                {
                    speedMBPerSecond = (fileSizeBytes / 1024) / elapsedTimeSeconds;
                }
                catch (Exception ex)
                {

                    f2.DisplayOutputWithMaxLines($"Error:{ex}", 100);
                }


                
                double E_time = 0;
               
                if (speedMBPerSecond != 0)
                {
                    E_time = N_countNum * (fileSizeBytes / 1024) / speedMBPerSecond;
                    //f2.DisplayOutputWithMaxLines($"n_count{BackCo}",100);

                }
                this.BeginInvoke((MethodInvoker)delegate
                {
                    scanfile();
                    this.label_speed.Text = $"{speedMBPerSecond:F2} KB/s";
                    Times = decimal.Parse(E_time.ToString());
                    timer1.Start();

                }); oldTime = DateTime.Now;

            }
        }
        int N_count(int startNum)
        {

            var ncount = int.Parse(tofile) - startNum;
            return ncount;
        }

        void scanfile()
        {

            try
            {
                string[] files = Directory.GetFiles(FilePath);

                //  f2.DisplayOutputWithMaxLines($"scanfile", 100);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    // f2.DisplayOutputWithMaxLines(fileName, 100);

                    if (IsWithinRange(fileName))
                    {
                        var aa = Regex.Match(fileName, @"\d+").Value;
                        if (!creatFilecount.Contains(int.Parse(aa)))
                        {
                            creatFilecount.Add(int.Parse(aa));
                        }

                    }


                }

                this.BeginInvoke((MethodInvoker)delegate
                {
                    this.label_copl.Text = $"{(int.Parse(tofile) - int.Parse(fromfile) + 1).ToString()}/{creatFilecount.Count}";
                    if ((int.Parse(tofile) - int.Parse(fromfile) + 1) == creatFilecount.Count  && Times<=0)
                    {
                        if (!SpaceHelper.creatover.Contains(cardID))
                        {
                            SpaceHelper.creatover.Add(cardID);
                        }
                        
                    }
                });

                
                checkper();
            }
            catch (Exception)
            {


            }



        }
        
        public void checkper()
        {

            if (!_process.HasExited)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    this.pictureBox1.Image = Resources.OK;
                });

            }
            else
            {
               
                this.BeginInvoke((MethodInvoker)delegate
                {
                    this.pictureBox1.Image = Resources.ER;
                });
            }
        }

        public string ConvertTime(string time)
        {
            var a = TimeSpan.FromSeconds(double.Parse(time));
            string str = string.Format("{0}:{1}:{2}:{3}", a.Days, a.Hours, a.Minutes, a.Seconds);
            return str;
        }
        private Int64 RunTime(DateTime beforDT)
        {

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            string str = ((ts.TotalMilliseconds) / 1000).ToString();
            string TimeRun = str.Split('.')[0];
            return Int64.Parse(TimeRun);

        }
        List<int> creatFilecount = new List<int>();
        private bool IsWithinRange(string fileName)
        {
            string numberPart = Regex.Match(fileName, @"\d+").Value;
        

            try
            {



                if (int.Parse(numberPart) >= int.Parse(fromfile) && int.Parse(numberPart) <= int.Parse(tofile))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }

        }
        public decimal Times = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            // f2.DisplayOutputWithMaxLines(ConvertTime(Times.ToString()), 100);
            Times--;
            if (Times >= 0)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    this.label3.Text = ConvertTime(Times.ToString());
                });

            }
            else
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    this.label3.Text = "00:00:00:00";
                });

                timer1.Stop();

            }


        }
    }
}
