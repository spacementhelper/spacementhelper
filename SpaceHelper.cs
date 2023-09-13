using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpacemeshHelper.Properties;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static SpacemeshHelper.F2;
using Timer = System.Windows.Forms.Timer;

namespace SpacemeshHelper
{
    public partial class SpaceHelper : Form
    {
        public Timer timer1;

        public static List<string> Config = new List<string>();

        public SpaceHelper()
        {
            InitializeTimer();
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectVgaCard("add");
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectVgaCard("remove");
        }
        void selectVgaCard(string c)
        {
            if (c == "add")
            {
                try
                {
                    var info = this.listBox1.SelectedItem.ToString();
                    this.listBox1.Items.Remove(info);
                    this.listBox2.Items.Add(info);
                }
                catch (Exception)
                {


                }
                return;
            }
            if (c == "remove")
            {
                try
                {
                    var info = this.listBox2.SelectedItem.ToString();
                    this.listBox2.Items.Remove(info);
                    this.listBox1.Items.Add(info);

                }
                catch (Exception)
                {


                }
                return;
            }



        }
        bool autorun = false;
        private void InitializeTimer()
        {
            timer1 = new Timer();
            timer1.Interval = 60000;
            timer1.Tick += timer1_Tick;

        }
        async Task AddtoPostAsync()
        {

            tabControl1.SelectedTab = tabPage2;

            this.listBox3.Items.Add(FilePath);
            for (int i = countdownSeconds; i >= 0; i--)
            {
                button5.Text = i.ToString();
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            button5.Text = "RunAllNode";
            button5.PerformClick();


        }
        private async void timer1_Tick(object? sender, EventArgs e)
        {


            if (checkBox5.Checked)
            {
                if (CardCount == creatover.Count && CardCount != 0)
                {
                    await Task.Run(async () =>
                    {
                        await AddtoPostAsync();
                    });

                    timer1.Stop();

                    foreach (Control control in flowLayoutPanel1.Controls)
                    {
                        if (control is UC userControl)
                        {
                            await Task.Run(() =>
                              {
                                  userControl.KillProcess();
                              });

                        }
                    }
                }
            }

        }

        private async void SpaceHelper_Load(object sender, EventArgs e)
        {



            int size = 64;
            List<string> lst = new List<string>();
            for (int i = 4; i < 200; i++)
            {
                var tp = (size * i).ToString();
                lst.Add(tp + "GB");
            }
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.AddRange(lst.ToArray());
            this.comboBox2.SelectedIndex = 0;

            getlocalVer();


            var nodeaddress = iniHelper.ReadValue("CONFIG", "nodeadd");
            if (nodeaddress != "")
            {
                this.textBox3.Text = nodeaddress;
            }

            this.label10.Text = Postver;
            this.label16.Text = Gover;
            this.button2.Enabled = false;

            var goconf = iniHelper.ReadValue("GO", "Path");

            if (goconf != "")
            {
                for (int i = 1; i < goconf.Split('|').Length; i++)
                {
                    this.listBox3.Items.Add(goconf.Split('|')[i]);

                }

            }
            foreach (var it1 in listBox3.Items)
            {
                if (!postdir.Contains(it1))
                {
                    postdir.Add(it1.ToString());

                }
            }


            setpostport();
            getCardList();


            this.textBox4.Text = iniHelper.ReadValue("GO", "coinbase");

            var restore = iniHelper.ReadValue("CONFIG", "restore");
            if (restore == "true")
            {

                FilePath = iniHelper.ReadValue("CONFIG", "FilePath");
                NodeID = iniHelper.ReadValue("CONFIG", "nID");
                CommID = iniHelper.ReadValue("CONFIG", "CommID");

                this.textBox1.Text = FilePath;
                this.label4.Text = $"[commitmentAtxId]:[{CommID}]";
                this.label5.Text = $"[ID]:[{NodeID}]";

                this.comboBox1.Text = iniHelper.ReadValue("CONFIG", "CreatSize") + "GB";
                var iCardcount = int.Parse(iniHelper.ReadValue("CONFIG", "CardCount"));

                foreach (var item in listBox1.Items)
                {
                    if (listBox2.Items.Count < iCardcount)
                    {
                        listBox2.Items.Add(item);
                    }

                }
                foreach (var item in listBox2.Items)
                {
                    listBox1.Items.Remove(item);
                }

                setFileCount();

            }

            var goautorun = iniHelper.ReadValue("GO", "GOautoRun");
            if (goautorun == "true")
            {
                this.checkBox4.Checked = true;
            }

            if (iniHelper.ReadValue("CONFIG", "autorun") == "true")
            {
                autorun = true;
                this.checkBox2.Checked = true;
                tabControl1.SelectedTab = tabPage1;
                await Task.Run(() =>
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        button1.PerformClick();
                    });
                });

                if (autorun)
                {
                    tabControl1.SelectedTab = tabPage1;

                    for (int i = countdownSeconds; i >= 0; i--)
                    {
                        button2.Text = i.ToString();
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    button2.Enabled = true;
                    button2.Text = "Start";
                    await Task.Run(() =>
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            button2.PerformClick();
                        });
                    });
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }

            }



            if (goautorun == "true")
            {

                tabControl1.SelectedTab = tabPage2;
                for (int i = countdownSeconds; i >= 0; i--)
                {
                    button5.Text = i.ToString();
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                button5.Text = "RunAllNode";
                button5.PerformClick();

            }



        }
        public static string Postver = string.Empty;
        public static string Gover = string.Empty;
        public static string PostRver = string.Empty;
        public static string GRver = string.Empty;
        void getCardList()
        {
            try
            {


                string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
                var name = $"{applicationPath}\\POST_{SpaceHelper.Postver}\\postcli.exe -printProviders";
                var rawData1 = runcmd(name);
                var rawData = ExtractOutput(rawData1);
                var aaa = rawData.Split('\n');
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string ID = "";
                string Model = "";
                foreach (var item in aaa)
                {
                    if (item.Contains("ID:"))
                    {
                        ID = item.Split(' ')[4].Replace(",", "");
                        ID = ID.Trim();

                    }
                    if (item.Contains("Model:"))
                    {
                        if (item.Contains("[GPU]"))
                        {
                            Model = item.Split('"')[1].Replace(",", "");
                            Model = Model.Replace("[GPU]", "");
                            Model = Model.Replace("\"", "");
                            Model = Model.Split("/")[1];
                            Model = Model.Trim();

                            dic.Add(ID, Model);
                        }
                        else
                        {
                            ID = "";
                            Model = "";

                        }

                    }
                }

                foreach (var item in dic)
                {
                    this.listBox1.Items.Add($"{item.Key}--{item.Value}");
                }
            }
            catch (Exception)
            {


            }

        }
        async Task DownloadPost()
        {
            try
            {


                string releaseUrl = "https://api.github.com/repos/spacemeshos/post/releases/latest"; // GitHub API URL for the latest release
                string jsonResponse = string.Empty;
                string downurl = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GitHubReleaseDownloader");

                    HttpResponseMessage response = await client.GetAsync(releaseUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {

                    }
                }

                JObject releaseData = JObject.Parse(jsonResponse);
                JArray assets = (JArray)releaseData["assets"];

                foreach (JToken asset in assets)
                {
                    string downloadUrl = asset["browser_download_url"].ToString();
                    if (downloadUrl.Contains("Windows"))
                    {
                        downurl = downloadUrl;

                    }

                }

                string versionPattern = @"\/v(\d+\.\d+\.\d+)\/"; 
                Match match = Regex.Match(downurl, versionPattern);
                var ver = match.Groups[1].Value;
                string downloadPath = "postcli-Windows.zip"; 
                string extractPath = $"POST_{ver}"; 
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(downurl, downloadPath);
                }

                if (File.Exists(downloadPath))
                {
                    ZipFile.ExtractToDirectory(downloadPath, extractPath);

                    aa();

                }
                else
                {

                }
            }
            catch (Exception e)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    //this.linkLabel5.Text = "";
                    richTextBox1.AppendText(e.ToString());
                    this.label18.Text = "error";

                });
            }

        }
        async Task DownloadGo()
        {
            try
            {


                string releaseUrl = "https://api.github.com/repos/spacemeshos/go-spacemesh/releases/latest"; // GitHub API URL for the latest release
                string jsonResponse = string.Empty;

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GitHubReleaseDownloader");

                    HttpResponseMessage response = await client.GetAsync(releaseUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {

                    }
                }

                JObject releaseData = JObject.Parse(jsonResponse);


                string windowsZipUrl = releaseData["body"].ToString().Split('\n').Select(line => line.Trim()).Where(line => line.StartsWith("- Windows:")).Select(line => line.Replace("- Windows:", "").Trim()).FirstOrDefault();


                string versionPattern = @"\/v(\d+\.\d+\.\d+)\/"; 
                Match match = Regex.Match(windowsZipUrl, versionPattern);
                var ver = match.Groups[1].Value;


                string extractPath = $"GO-SPACE_{ver}"; // Folder where you want to extract files 

                string downloadPath = $"{extractPath}.zip"; // Save downloaded file as this name
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(windowsZipUrl, downloadPath);
                }

                if (File.Exists(downloadPath))
                {
                    ZipFile.ExtractToDirectory(downloadPath, extractPath);


                    string targetDirectory = extractPath;
                    string sourceDirectory = $"{extractPath}\\windows";

                    try
                    {

                        string[] files = Directory.GetFiles(sourceDirectory);
                        foreach (string file in files)
                        {
                            string fileName = Path.GetFileName(file);
                            string destinationPath = Path.Combine(targetDirectory, fileName);
                            File.Move(file, destinationPath);
                        }


                    }
                    catch (Exception ex)
                    {
                        Directory.Delete(targetDirectory, true);
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            this.label17.Text = "don't run go-space";

                        });


                    }

                    cc();



                }
                else
                {

                }
            }
            catch (Exception)
            {

            }

        }
        void getlocalVer()
        {

            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(currentDirectory);
            DirectoryInfo[] subdirectories = directoryInfo.GetDirectories();


            foreach (DirectoryInfo subdirectory in subdirectories)
            {
                //    richTextBox1.AppendText(subdirectory.Name + "\r\n");

                if (subdirectory.Name.Contains("POST_"))
                {
                    Postver = subdirectory.Name.Replace("POST_", "");
                }
                if (subdirectory.Name.Contains("GO-SPACE_"))
                {
                    Gover = subdirectory.Name.Replace("GO-SPACE_", "");
                }


            }
            this.BeginInvoke((MethodInvoker)delegate
            {
                this.label10.Text = Postver;
                this.label16.Text = Gover;

            });




        }
        async Task getPOSTReVer()
        {
            try
            {


                string releaseUrl = "https://api.github.com/repos/spacemeshos/post/releases/latest"; // GitHub API URL for the latest release
                string jsonResponse = string.Empty;
                string downurl = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GitHubReleaseDownloader");

                    HttpResponseMessage response = await client.GetAsync(releaseUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {

                    }
                }

                JObject releaseData = JObject.Parse(jsonResponse);
                JArray assets = (JArray)releaseData["assets"];

                foreach (JToken asset in assets)
                {
                    string downloadUrl = asset["browser_download_url"].ToString();
                    if (downloadUrl.Contains("Windows"))
                    {
                        downurl = downloadUrl;

                    }

                }

                string versionPattern = @"\/v(\d+\.\d+\.\d+)\/";
                Match match = Regex.Match(downurl, versionPattern);
                PostRver = match.Groups[1].Value;
            }
            catch (Exception)
            {


            }
        }

        async Task getGoReVer()
        {
            try
            {


                string releaseUrl = "https://api.github.com/repos/spacemeshos/go-spacemesh/releases/latest";
                string jsonResponse = string.Empty;
                string downurl = string.Empty;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "GitHubReleaseDownloader");

                    HttpResponseMessage response = await client.GetAsync(releaseUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {

                    }
                }

                JObject releaseData = JObject.Parse(jsonResponse);


                string windowsZipUrl = releaseData["body"].ToString().Split('\n').Select(line => line.Trim()).Where(line => line.StartsWith("- Windows:")).Select(line => line.Replace("- Windows:", "").Trim()).FirstOrDefault();




                string versionPattern = @"\/v(\d+\.\d+\.\d+)\/";
                Match match = Regex.Match(windowsZipUrl, versionPattern);
                GRver = match.Groups[1].Value;


            }
            catch (Exception)
            {


            }
        }
        void setFileCount()
        {
            try
            {
                int FileSize = int.Parse(this.comboBox2.Text.Replace("MB", ""));
                int CreatSize = int.Parse(this.comboBox1.Text.Replace("GB", "")) * 1024;
                int filecount = CreatSize / FileSize;
                this.textBox2.Text = filecount.ToString();
                aFileSize = FileSize;
                aCreatSize = CreatSize;
                aCreatFileCount = filecount;
            }
            catch (Exception)
            {


            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFileCount();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFileCount();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            folderBrowserDialog1.ShowDialog();
            this.textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        public static int aFileSize = 0;
        public static int aCreatSize = 0;
        public static int aCreatFileCount = 0;
        public static int CardCount = 0;
        public static string FilePath = "";
        public static string NodeID = "";
        public static string CommID = "";


        private int countdownSeconds = 5;
        string nodeAdd = "";
        private async void button1_ClickAsync(object sender, EventArgs e)
        {



            if (aFileSize != 0 && aCreatSize != 0 && aCreatFileCount != 0 && listBox2.Items.Count != 0 && this.textBox1.Text != "")
            {
                FilePath = this.textBox1.Text;
                CardCount = listBox2.Items.Count;
                this.flowLayoutPanel1.Controls.Clear();
                nodeAdd = this.textBox3.Text.Trim();
                iniHelper.WriteValue("CONFIG", "nodeadd", nodeAdd);
                addPL();
            }
            else
            {

                MessageBox.Show($"Please set parameter ! CardCount:{listBox2.Items.Count},aFileSize:{aFileSize},aCreatSize:{aCreatSize},aCreatFileCount:{aCreatFileCount}");
            }

            if (checkBox1.Checked)
            {
                if (checkBox2.Checked)
                {
                    if (NodeID != "")
                    {
                        //for (int i = countdownSeconds; i >= 0; i--)
                        //{
                        //    button2.Text = i.ToString();
                        //    await Task.Delay(TimeSpan.FromSeconds(1)); 
                        //}
                        //button2.Enabled = true;
                        //button2.Text = "Start";
                        //await Task.Run(() =>
                        //{
                        //    this.Invoke((MethodInvoker)delegate
                        //    {
                        //        button2.PerformClick();
                        //    });
                        //});
                    }
                    else
                    {

                        await aabAsync();
                        return;
                        for (int i = countdownSeconds; i >= 0; i--)
                        {
                            button2.Text = i.ToString();
                            await Task.Delay(TimeSpan.FromSeconds(1)); 
                        }
                        button2.Enabled = true;
                        button2.Text = "Start";




                    }

                }
                else
                {
                    if (NodeID != "")
                    {

                        this.button2.Enabled = true;
                    }
                    else
                    {
                        await aabAsync();
                    }
                }
            }
            else
            {
                await aabAsync();
            }


        }

        void addPL()
        {

            Totalnum(aCreatFileCount, CardCount);
        }

        void test()
        {

            int totalValue = 128;
            int numberOfParts = 3;

            int baseValue = totalValue / numberOfParts;
            int remainder = totalValue % numberOfParts;

            int[] distribution = new int[numberOfParts];

            for (int i = 0; i < numberOfParts; i++)
            {
                distribution[i] = baseValue;


                if (i == numberOfParts - 1)
                {
                    distribution[i] += remainder;
                }
            }



            int accumulatedValue = 0;
            for (int i = 0; i < numberOfParts; i++)
            {
                int startValue = accumulatedValue;
                int endValue = accumulatedValue + distribution[i] - 1;

                richTextBox1.AppendText($"Part {i + 1} First: {startValue}, Last: {endValue}");
                accumulatedValue = endValue + 1;
            }
        }
        void Totalnum(int totalValue, int numberOfParts)
        {



            int baseValue = totalValue / numberOfParts;
            int remainder = totalValue % numberOfParts;

            int[] distribution = new int[numberOfParts];

            for (int i = 0; i < numberOfParts; i++)
            {
                distribution[i] = baseValue;


                if (i == numberOfParts - 1)
                {
                    distribution[i] += remainder;
                }
            }
            int accumulatedValue = 0;
            for (int i = 0; i < listBox2.Items.Count; i++)
            {

                int startValue = accumulatedValue;
                int endValue = accumulatedValue + distribution[i] - 1;
                this.flowLayoutPanel1.Controls.Add(new UC(listBox2.Items[i].ToString() + "|" + startValue + "|" + endValue));

                accumulatedValue = endValue + 1;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (this.button2.Text == "StopAll")
            {
                timer1.Stop();
                try
                {
                    this.panel4.Enabled = true;
                    foreach (Control control in flowLayoutPanel1.Controls)
                    {
                        if (control is UC userControl)
                        {
                            Task.Run(() =>
                            {
                                userControl.KillProcess();
                            });

                        }
                    }

                }
                catch (Exception)
                {


                }
                this.button2.Text = "Start";
                return;
            }

            this.panel2.Enabled = false;
            this.panel4.Enabled = false;
            string PostInfo = $"NodeID:{NodeID},CommID:{CommID},FilePath:{FilePath},aCreatSize:{(aCreatSize / 1024).ToString()}\r\n";
            DateTime currentTime = DateTime.Now;
            string formattedDateTime = currentTime.ToString("yyyyMMdd_HHmmss");

            string savePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Log\\{formattedDateTime}_Post.txt";
            try
            {
                Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}\\Log");
            }
            catch (Exception)
            {

            }
            var creatsize = (aCreatSize / 1024).ToString();
            var CardCount = this.listBox2.Items.Count;
            iniHelper.WriteValue("CONFIG", "CardCount", CardCount.ToString());
            iniHelper.WriteValue("CONFIG", "nID", NodeID);
            iniHelper.WriteValue("CONFIG", "CommID", CommID);
            iniHelper.WriteValue("CONFIG", "FilePath", FilePath);
            iniHelper.WriteValue("CONFIG", "CreatSize", creatsize);
            if (checkBox1.Checked)
            {
                iniHelper.WriteValue("CONFIG", "restore", "true");
            }
            else
            {
                iniHelper.WriteValue("CONFIG", "restore", "flase");
            }

            if (checkBox2.Checked)
            {
                iniHelper.WriteValue("CONFIG", "autorun", "true");
                //autorun();
            }
            else
            {
                //removerun();
                iniHelper.WriteValue("CONFIG", "autorun", "false");
            }
            File.AppendAllText(savePath, PostInfo);
            foreach (Control control in flowLayoutPanel1.Controls)
            {


                if (control is UC userControl)
                {
                    Task.Run(() =>
                  {
                      userControl.SomeMethod(NodeID, CommID, FilePath, (aCreatSize / 1024).ToString());
                  });

                }
            }
            timer1.Start();
            this.button2.Text = "StopAll";

            Task.Run(async () =>
            {
                await Task.Delay(20000);
            });
            ////
            ////
            ///


        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        public string runcmd(string command)
        {
            string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            ProcessStartInfo startInfo = new ProcessStartInfo
            {

                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process process = new Process { StartInfo = startInfo };
            process.Start();
            process.StandardInput.WriteLine(command);
            process.StandardInput.WriteLine("exit");
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        private void button3_ClickAsync(object sender, EventArgs e)
        {
            foreach (var item in Postport)
            {
                MessageBox.Show(item);
            }




        }

        async Task aabAsync()
        {

            await Task.Run(() =>
            {

                GetID();

            });

        }

        string ConvertJsonRTID(string output)
        {

            try
            {


                string jsonx = "";
            
                string pattern = @"\{[\s\S]*""numUnits"": \d+[\s\S]*\}";
                Match match = Regex.Match(output, pattern);

                if (match.Success)
                {
                    string desiredJsonSegment = match.Value;
                    jsonx = desiredJsonSegment;
                    // Console.WriteLine(desiredJsonSegment);
                }
                else
                {
                    return "";
                    //Console.WriteLine("Desired JSON data not found in the string.");
                }

                JObject json = JObject.Parse(jsonx);
                JToken idToken = json["atx"]["id"]["id"];
                string idValue = idToken.Value<string>();
                return idValue;
            }
            catch (Exception)
            {

                return "";
            }
        }
        string GetCommitmentAtxId()
        {
            try
            {

                string spaceserver = this.textBox3.Text.Trim();
                string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
                string args = $"{applicationPath}grpcur.exe  -plaintext {spaceserver} spacemesh.v1.ActivationService.Highest";
                var base64val = runcmd(args);

                var _base64 = ConvertJsonRTID(base64val);
                if (_base64 == "")
                {
                   
                    return "";
                }
              
                byte[] byteArray = Convert.FromBase64String(_base64);
               
                var commitmentAtxId = BitConverter.ToString(byteArray).Replace("-", "").ToLower();
                CommID = commitmentAtxId;
                return commitmentAtxId;
                

            }
            catch (Exception)
            {

                return "";
            }
        }

        void GetID()
        {

            var commitmentAtxId = GetCommitmentAtxId();
            if (commitmentAtxId == "")
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    this.label4.Text = $"[CommitmentAtxId]:[Get CommitmentAtxId Error...Please Restart SPacement Server Node]";
                    this.label5.Text = "[ID]: [Get ID Error...]";

                });
            }
            else
            {
                TempGetID(commitmentAtxId);
            }


        }
        private Process _process;
        private CancellationTokenSource _cancellationTokenSource;


        void TempGetID(string commitmentAtxId)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                this.label4.Text = $"[CommitmentAtxId]:[{commitmentAtxId}]";
                this.label5.Text = "[ID]: [Get ID ......]";

            });
            string command = $" -provider 0 -commitmentAtxId {commitmentAtxId} -numUnits {(aCreatSize / 1024) / 64} -datadir {FilePath} ";

            RunCommand(command);


            //ProcessStartInfo startInfo = new ProcessStartInfo
            //{
            //    FileName = "cmd.exe",
            //    RedirectStandardInput = true,
            //    RedirectStandardOutput = true,
            //    UseShellExecute = false,
            //    CreateNoWindow = true
            //}; 
            //_process = new Process { StartInfo = startInfo }; 
            //_process.Start(); 
            //_cancellationTokenSource = new CancellationTokenSource();
            //Task outputTask = MonitorOutputAsync(_process, _cancellationTokenSource.Token); 
            //_process.StandardInput.WriteLine(command);


        }

        CancellationTokenSource cts = new CancellationTokenSource();
        private async void RunCommand(string command)
        {

            cts.CancelAfter(TimeSpan.FromSeconds(8));

            string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = $"{applicationPath}\\POST_{Postver}\\postcli.exe",
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


        void enableStartBtn(string ID)
        {
            this.label5.Text = $"[ID]:[{ID}]";
            NodeID = ID;
            this.button2.Enabled = true;

        }
        private async Task MonitorOutputAsync(Process process, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                string line = await process.StandardOutput.ReadLineAsync();
                if (line != null)
                {
                     

                    string pattern = @"generated id (\w+)";  
                    Match match = Regex.Match(line, pattern);  
                    if (match.Success)
                    {
                        string id = match.Groups[1].Value;  
                        if (File.Exists(FilePath + "\\key.bin") && File.Exists(FilePath + "\\postdata_metadata.json"))
                        {

                            _process.Kill();
                            _cancellationTokenSource.Cancel();
                            enableStartBtn(id);
                        }
                        else
                        {
                            Thread.Sleep(1000);

                            _process.Kill();
                            _cancellationTokenSource.Cancel();
                            enableStartBtn(id);
                        }

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
                   

                    string pattern = @"generated id (\w+)";  
                    Match match = Regex.Match(line, pattern);  
                    if (match.Success)
                    {
                        string id = match.Groups[1].Value; 
                        if (File.Exists(FilePath + "\\key.bin") && File.Exists(FilePath + "\\postdata_metadata.json"))
                        {

                            _process.Kill();
                            _cancellationTokenSource.Cancel();
                            enableStartBtn(id);
                        }
                        else
                        {
                            Thread.Sleep(3000);
                            _process.Kill();
                            _cancellationTokenSource.Cancel();
                            enableStartBtn(id);
                        }
                    }
                    else
                    {

                    }

                }
            }
        }
        
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (linkLabel4.Text == "Refresh")
            {
                Thread s = new Thread(cc);
                s.Start();
                return;
            }
            if (linkLabel4.Text == "Update")
            {

                this.label17.Text = $"Update...";
                Thread s = new Thread(dd);
                s.Start();


                return;
            }
        }

        private async void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (linkLabel5.Text == "Refresh")
            {
                Thread s = new Thread(aa);
                s.Start();
                return;
            }
            if (linkLabel5.Text == "Update")
            {

                this.label18.Text = $"Update...";
                Thread s = new Thread(bb);
                s.Start();


                return;
            }


        }

        private async void bb(object? obj)
        {
            await DownloadPost();
            getlocalVer();
        }
        private async void dd(object? obj)
        {
            await DownloadGo();
            getlocalVer();
        }
        void aa()
        {
            this.BeginInvoke((MethodInvoker)async delegate
            {
                await getPOSTReVer();
                this.label18.Text = $"LastVer:{PostRver}";
                if (Postver == PostRver)
                {
                    this.label18.Text = $"LastVer:{PostRver}";
                    this.linkLabel5.Text = "Refresh";
                }
                else
                {
                    this.label18.Text = $"LastVer:{PostRver}";
                    this.linkLabel5.Text = "Update";



                }
            });

        }
        void cc()
        {
            this.BeginInvoke((MethodInvoker)async delegate
            {
                await getGoReVer();
                this.label17.Text = $"LastVer:{GRver}";
                if (Gover == GRver)
                {
                    this.label17.Text = $"LastVer:{GRver}";
                }
                else
                {
                    if (GRver == "")
                    {
                        return;
                    }
                    this.label17.Text = $"LastVer:{GRver}";
                    this.linkLabel4.Text = "Update";



                }
            });

        }
        private void SpaceHelper_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure £¿", "close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

 
 if (result == DialogResult.No)
 {
     e.Cancel = true;
     return;
 }


 foreach (Control control in flowLayoutPanel1.Controls)
 {
     if (control is UC userControl)
     {
         Task.Run(() =>
         {
             userControl.KillProcess();
         });

     }
 }

 foreach (Control control in flowLayoutPanel2.Controls)
 {
     if (control is POST userControl)
     {
         Task.Run(() =>
         {
             userControl.KillProcess();
         });

     }
 }
        }
        IniHelper iniHelper = new IniHelper(Application.StartupPath + "\\Config.ini");

        public string ExtractOutput(string fullOutput)
        {
            int startIndex = fullOutput.IndexOf("([]postrs.Provider)");
            int endIndex = fullOutput.LastIndexOf("}");

            if (startIndex >= 0 && endIndex >= 0)
            {
                int length = endIndex - startIndex + 1;
                return fullOutput.Substring(startIndex, length);
            }

            return "";
        }



        void TTest()
        {

            this.panel2.Enabled = false;
            string PostInfo = $"NodeID:{NodeID},CommID:{CommID},FilePath:{FilePath},aCreatSize:{(aCreatSize / 1024).ToString()}\r\n";
            DateTime currentTime = DateTime.Now;
            string formattedDateTime = currentTime.ToString("yyyyMMdd_HHmmss");

            string savePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Log\\{formattedDateTime}_Post.txt";
            try
            {
                Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}\\Log");
            }
            catch (Exception)
            {

            }
            var creatsize = (aCreatSize / 1024).ToString();
            var CardCount = this.listBox2.Items.Count;
            iniHelper.WriteValue("CONFIG", "CardCount", CardCount.ToString());
            iniHelper.WriteValue("CONFIG", "nID", NodeID);
            iniHelper.WriteValue("CONFIG", "CommID", CommID);
            iniHelper.WriteValue("CONFIG", "FilePath", FilePath);
            iniHelper.WriteValue("CONFIG", "CreatSize", creatsize);
            if (checkBox1.Checked)
            {
                iniHelper.WriteValue("CONFIG", "restore", "true");
            }
            else
            {
                iniHelper.WriteValue("CONFIG", "restore", "flase");
            }

            if (checkBox2.Checked)
            {
                iniHelper.WriteValue("CONFIG", "autorun", "true");
                //autorun();
            }
            else
            {
                //removerun();
                iniHelper.WriteValue("CONFIG", "autorun", "true");
            }
            File.AppendAllText(savePath, PostInfo);




            foreach (Control control in flowLayoutPanel1.Controls)
            {


                if (control is UC userControl)
                {
                    Task.Run(() =>
                    {
                        userControl.SomeMethod(NodeID, CommID, FilePath, (aCreatSize / 1024).ToString());
                    });

                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //TTest();


        }

        private void SpaceHelper_Shown(object sender, EventArgs e)
        {


        }

        public class Provider
        {
            [JsonProperty("ID")]
            public uint ID { get; set; }

            [JsonProperty("Model")]
            public string Model { get; set; }

            [JsonProperty("DeviceType")]
            public string DeviceType { get; set; }
        }
        List<string> postdir = new List<string>();
        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {



        }

        private void addDiskDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                folderBrowserDialog2.ShowDialog();
                var dir = folderBrowserDialog2.SelectedPath;
                if (!postdir.Contains(dir))
                {
                    postdir.Add(dir);
                    listBox3.Items.Add(dir);
                }
            }
            catch (Exception)
            {


            }


        }

        private void removeDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var info = this.listBox3.SelectedItem.ToString();
                this.listBox3.Items.Remove(info);
                postdir.Remove(info);
            }
            catch (Exception)
            {


            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {


                string conf = string.Empty;


                foreach (var item in listBox3.Items)
                {
                    conf = conf + "|" + item.ToString();
                }

                iniHelper.WriteValue("GO", "Path", conf);

                if (checkBox4.Checked)
                {
                    iniHelper.WriteValue("GO", "GOautoRun", "true");
                }
                var coinbase = this.textBox4.Text.Trim();
                iniHelper.WriteValue("GO", "coinbase", coinbase);
            }

            this.panel6.Enabled = false;
            Post();




        }

        List<string> PostView = new List<string>();

        static double GetDirectorySizeInGB(string path)
        {
            try
            {
                long bytes = GetDirectorySize(path);
                double gigabytes = (double)bytes / (1024 * 1024 * 1024);
                return gigabytes;
            }
            catch (Exception)
            {
                return 0;

            }

        }

        static long GetDirectorySize(string path)
        {
            if (!Directory.Exists(path))
            {
               
            }

            long size = 0;

            foreach (string file in Directory.GetFiles(path))
            {
                FileInfo fileInfo = new FileInfo(file);
                size += fileInfo.Length;
            }

            foreach (string subDirectory in Directory.GetDirectories(path))
            {
                size += GetDirectorySize(subDirectory);
            }

            return size;
        }
        List<string> Postport = new List<string>();

        void setpostport()
        {
            var aa = 10200;

            for (int i = 0; i < 1000; i++)
            {

                Postport.Add($"{aa}");
                aa += 10;
            }
        }
        //public static void isover()
        //{
        //    if (creatover.Count== CardCount)
        //    {

        //    }

        //}
        public static List<string> creatover = new List<string>();
        void Post()
        {
            List<string> temppost = new List<string>();
            List<string> diff2 = postdir.Except(PostView).ToList();
            var plcount = this.flowLayoutPanel2.Controls.Count;
            int Num = 0;
            for (int i = 0; i < diff2.Count; i++)
            {

                var path = diff2[i];
                if (plcount > 0)
                {
                    Num = plcount + i;

                }
                else
                {
                    Num = i + 1;
                }

                var w = this.textBox4.Text.Trim();
                double postsize = GetDirectorySizeInGB(path);
                var post_pt = Postport[i];
                var nons = this.textBox5.Text.Trim();
                var threads = this.textBox6.Text.Trim();
                temppost.Add(post_pt);
                var info = $"{Num},{path},{postsize},{w},{post_pt},{nons},{threads}";
                this.flowLayoutPanel2.Controls.Add(new POST(info));
                PostView.Add(diff2[i]);
            }

            foreach (var item in temppost)
            {
                try
                {
                    Postport.Remove(item);
                }
                catch (Exception)
                {


                }

            }



            foreach (Control control in flowLayoutPanel2.Controls)
            {


                if (control is POST userControl)
                {
                    if (!userControl.issrunning)
                    {
                        Task.Run(() =>
                        {
                            userControl.RunPost();
                        });
                    }


                }
            }




        }

        private void removeALLToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                iniHelper.WriteValue("GO", "GOautoRun", "true");
                return;
            }
            if (!checkBox4.Checked)
            {
                iniHelper.WriteValue("GO", "GOautoRun", "false");
                return;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                iniHelper.WriteValue("CONFIG", "autorun", "true");
                return;
            }
            if (!checkBox2.Checked)
            {
                iniHelper.WriteValue("CONFIG", "autorun", "false");
                return;
            }
        }
    }


}