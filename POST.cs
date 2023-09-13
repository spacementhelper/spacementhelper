using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpacemeshHelper.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SpacemeshHelper.F2;

namespace SpacemeshHelper
{

    public partial class POST : UserControl
    {
        FileSystemWatcher watcher;
        public Process _process;
        private CancellationTokenSource _cancellationTokenSource;


        string info = string.Empty;
        string Num = string.Empty;
        string postPath = string.Empty;
        string PostSize = string.Empty;
        string W = string.Empty;
        string port_pt = string.Empty;
        string nons = string.Empty;
        string threads = string.Empty;  


        public void KillProcess()
        {
            try
            {
                _process.Kill();
            }
            catch (Exception)
            {


            }

        }

        string jsonText = @"{
  ""main"": {
    ""layer-duration"": ""5m"",
    ""layers-per-epoch"": 4032,
    ""poet-server"": [
      ""https://mainnet-poet-0.spacemesh.network"",
      ""https://mainnet-poet-1.spacemesh.network"",
      ""https://mainnet-poet-2.spacemesh.network"",
      ""https://poet-110.spacemesh.network"",
      ""https://poet-111.spacemesh.network""
    ]
  },
  ""post"": {
    ""post-labels-per-unit"": 4294967296,
    ""post-max-numunits"": 1048576
  },
  ""poet"": {
    ""cycle-gap"": ""12h""
  },
  ""genesis"": {
    ""genesis-time"": ""2023-07-14T08:00:00Z"",
    ""genesis-extra-data"": ""00000000000000000001a6bc150307b5c1998045752b3c87eccf3c013036f3cc"",
    ""accounts"": {
      ""sm1qqqqqqylyl2l0zsmmax0wnutt4dwnrkcwef5eeq3xladz"": 2743200000000000,
      ""sm1qqqqqqyp8ueuuh2dgrc2g6ps4xvueyjpky6rfaqnxdy97"": 5867100000000000,
      ""sm1qqqqqqzgmt5vv4jgucas8vvrlu4daa4r29cunwqpv0trt"": 1022800000000000,
      ""sm1qqqqqq80we5pmwztmqgpxu6xasapgn65r4xjczqxu39a2"": 409000000000000,
      ""sm1qqqqqqy6anfdew2sdtvuuaffjy0l7ssu9r8vjsss5c442"": 2045400000000000,
      ""sm1qqqqqqyw9lvmmayckrxlnf8u7850tsjdg8zz6dg956gxg"": 270600000000000,
      ""sm1qqqqqq9a8g5act6ewmmmmmux8l570kr6l68htzsq94wg4"": 4090900000000000,
      ""sm1qqqqqqrgqc65x5q6exujgjs970fvcakd790na3gsr3uu7"": 333300000000000,
      ""sm1qqqqqqpc4ppx8s4gmdaa5tzg35s6l3v6ujg6hmqz3s4lc"": 859100000000000,
      ""sm1qqqqqq8za0geafhj4avegdwhtaw9fmgjh07s55cufk695"": 293300000000000,
      ""sm1qqqqqqpf6djx3axy7aag8zhyf84ljsulhfypfxgpw5y0u"": 1990600000000000,
      ""sm1qqqqqq827v998nt99vupxlrfucdk0tapp2hjyygmn3kyd"": 409100000000000,
      ""sm1qqqqqqpc55ghjq6sxf5k77yc8n82fkwhlj0jedcgw2zck"": 4909100000000000,
      ""sm1qqqqqqxq54zvz484hhcnrghnqrjlw26twwld32slz3lxa"": 191800000000000,
      ""sm1qqqqqqyf5uc2n8mutm3tuateu5efcm9awvrclmcm5mhdf"": 2933540000000000,
      ""sm1qqqqqq99klpy92mwlfcft5lmz8q5sef2v2qvtucd9y55v"": 2933540000000000,
      ""sm1qqqqqqyjpjgup8fz32cufcv2nlqrr3nyvge7akqt0daea"": 2933540000000000,
      ""sm1qqqqqq8zukfwtggnfq4jaqpv6m8xgtg5ay2ezaqpr2w6y"": 2933540000000000,
      ""sm1qqqqqqrhftrq9knsetema7dt0qfzgd5a20m9rcczk0gk5"": 2933540000000000,
      ""sm1qqqqqqyfq5f522mmrzs4lczhaf30jh4pmqyfrzcg8vrpc"": 3303792000000000,
      ""sm1qqqqqqx55z5795569fq5kym3gw2h6zp6ajeh46c5wtrzf"": 455300000000000,
      ""sm1qqqqqqyvet26gqsxjt6w50nnp80jvajr3n25xzsdpxn65"": 831250000000000,
      ""sm1qqqqqqzgqpjxdw77aw74f8mz540rykda4x2jgjgaca7z5"": 184375000000000,
      ""sm1qqqqqq9s5l9tc87wspycr68dfagmzxplzdn7zlcymnkup"": 15000000000000,
      ""sm1qqqqqqptx3mdg4gm67arv4ykau6nfy6w9v03x9s49wmru"": 100000000000000,
      ""sm1qqqqqq9fwfymdr7qv0tfc3ppa4q8ara6qm7kwugw9gdme"": 500000000000000,
      ""sm1qqqqqqy3fc8nvdetan6qjz5cju7h4c60mjyvdlqnlqpxu"": 15688500000000000,
      ""sm1qqqqqqrt64knhuxu3kzq50ak04nrkk9yf2zxprshmvkcy"": 88818783000000000
    }
  },
  ""api"": {
    ""grpc-public-services"": [
      ""debug"",
      ""global"",
      ""mesh"",
      ""node"",
      ""transaction"",
      ""activation""
    ],
    ""grpc-public-listener"": ""0.0.0.0:3012"",
    ""grpc-private-services"": [
      ""smesher"",
      ""admin""
    ],
    ""grpc-private-listener"": ""127.0.0.1:3013"",
    ""grpc-json-listener"": ""0.0.0.0:3014""
  },
  ""p2p"": {
    ""disable-reuseport"": false,
    ""p2p-disable-legacy-discovery"": true,
    ""autoscale-peers"": true,
    ""bootnodes"": [
      ""/dns4/mainnet-bootnode-0.spacemesh.network/tcp/5000/p2p/12D3KooWPStnitMbLyWAGr32gHmPr538mT658Thp6zTUujZt3LRf"",
      ""/dns4/mainnet-bootnode-2.spacemesh.network/tcp/5000/p2p/12D3KooWAsMgXLpyGdsRNjHBF3FaXwnXhyMEqWQYBXUpvCHNzFNK"",
      ""/dns4/mainnet-bootnode-4.spacemesh.network/tcp/5000/p2p/12D3KooWRcTWDHzptnhJn5h6CtwnokzzMaDLcXv6oM9CxQEXd5FL"",
      ""/dns4/mainnet-bootnode-6.spacemesh.network/tcp/5000/p2p/12D3KooWRS47KAs3ZLkBtE2AqjJCwxRYqZKmyLkvombJJdrca8Hz"",
      ""/dns4/mainnet-bootnode-8.spacemesh.network/tcp/5000/p2p/12D3KooWFYv99aGbtXnZQy6UZxyf72NpkWJp3K4HS8Py35WhKtzE"",
      ""/dns4/mainnet-bootnode-10.spacemesh.network/tcp/5000/p2p/12D3KooWHK5m83sNj2eNMJMGAngcS9gBja27ho83t79Q2CD4iRjQ"",
      ""/dns4/mainnet-bootnode-12.spacemesh.network/tcp/5000/p2p/12D3KooWG4gk8GtMsAjYxHtbNC7oEoBTMRLbLDpKgSQMQkYBFRsw"",
      ""/dns4/mainnet-bootnode-14.spacemesh.network/tcp/5000/p2p/12D3KooWRkZMjGNrQfRyeKQC9U58cUwAfyQMtjNsupixkBFag8AY"",
      ""/dns4/mainnet-bootnode-16.spacemesh.network/tcp/5000/p2p/12D3KooWDAFRuFrMNgVQMDy8cgD71GLtPyYyfQzFxMZr2yUBgjHK"",
      ""/dns4/mainnet-bootnode-18.spacemesh.network/tcp/5000/p2p/12D3KooWMJmdfwxDctuGGoTYJD8Wj9jubQBbPfrgrzzXaQ1RTKE6""
    ],
    ""min-peers"": 30,
    ""low-peers"": 80,
    ""high-peers"": 80,
    ""inbound-fraction"": 1.1,
    ""outbound-fraction"": 1.1
  },
  ""logging"": {
      ""p2p"": ""error""
  },
  ""smeshing"": {
    ""smeshing-opts"": {
      ""smeshing-opts-datadir"": ""Z:\\P001_512"",
      ""smeshing-opts-maxfilesize"": 4294967296,
      ""smeshing-opts-numunits"": 8,
      ""smeshing-opts-provider"": 0,
      ""smeshing-opts-throttle"": false,
      ""smeshing-opts-compute-batch-size"": 1048576
    },
    ""smeshing-coinbase"": ""you coinbase"",
    ""smeshing-proving-opts"": {
      ""smeshing-opts-proving-nonces"": 288,
      ""smeshing-opts-proving-threads"": 6
    },
    ""smeshing-start"": true
  }
}";
      
        void scanfile()
        {

            try
            {
                string[] files = Directory.GetFiles(postPath);
                List<string> lines = new List<string>();
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);

                    if (fileName == "post.bin")
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            this.pictureBox2.Image = Resources.OK;
                        });
                        continue;
                    }
                    try
                    {


                        if (fileName == "nipost_builder_state.bin")
                        {
                            lines.Add(fileName);
                        }
                        if (fileName == "nipost_challenge.bin")
                        {
                            lines.Add(fileName);
                        }
                    }
                    catch (Exception)
                    {


                    }
                }

                if (lines.Contains("nipost_builder_state.bin") && lines.Contains("nipost_challenge.bin"))
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        this.pictureBox1.Image = Resources.OK;
                    });
                }


            }
            catch (Exception)
            {


            }



        }
        private void InitializeWatcher()
        {
           
            watcher = new FileSystemWatcher();
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite; 
            watcher.Created += Watcher_Created; 
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            scanfile();
        }

        public POST(string info)
        {
            this.info = info;
            Num = info.Split(',')[0];
            postPath = info.Split(",")[1];
            PostSize = info.Split(",")[2].Split('.')[0];
            W = info.Split(",")[3];
            port_pt = info.Split(",")[4];
            nons = info.Split(",")[5];
            threads = info.Split(",")[6];   
            InitializeWatcher();
            InitializeComponent();
        }

        public bool issrunning = false;
        public async void RunPost()
        {
            issrunning = true; watcher.Path = postPath;
            watcher.EnableRaisingEvents = true;

            copyfile();
            f3.DisplayOutputWithMaxLines("CopyFile..", 50);
            await Task.Delay(1000);
            scanfile();
            var p1 = int.Parse(port_pt) + 1;
            var p2 = int.Parse(port_pt) + 2;
            var p3 = int.Parse(port_pt) + 3;
            int numunits = 0;
            try
            {
                numunits = int.Parse(PostSize) / 64;
                if (numunits < 1)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {

                        this.pictureBox1.Image = Resources.ER;
                        this.pictureBox2.Image = Resources.ER;
                    });
                    f3.DisplayOutputWithMaxLines("filesize too sm..", 50);
                    return;
                }
            }
            catch (Exception ex)
            {

                this.BeginInvoke((MethodInvoker)delegate
                {

                    this.pictureBox1.Image = Resources.ER;
                    this.pictureBox2.Image = Resources.ER;
                });
                return;
            }


          
            JObject jsonObj = JObject.Parse(jsonText);
       
            jsonObj["api"]["grpc-public-listener"] = $"0.0.0.0:{p1}";
            jsonObj["api"]["grpc-private-listener"] = $"127.0.0.1:{p2}";
            jsonObj["api"]["grpc-json-listener"] = $"0.0.0.0:{p3}";
            jsonObj["smeshing"]["smeshing-opts"]["smeshing-opts-datadir"] = postPath;
            jsonObj["smeshing"]["smeshing-opts"]["smeshing-opts-numunits"] = numunits;
            jsonObj["smeshing"]["smeshing-coinbase"] = W;
            jsonObj["smeshing"]["smeshing-proving-opts"]["smeshing-opts-proving-nonces"] = nons;
            jsonObj["smeshing"]["smeshing-proving-opts"]["smeshing-opts-proving-threads"] = threads;

       
            string modifiedJson = jsonObj.ToString(Formatting.Indented);
            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}GO-SPACE_{SpaceHelper.Gover}\\{Num}\\{Num}PostConfig.json", modifiedJson);
            f3.DisplayOutputWithMaxLines("Created Config.Json..", 50);
            Task.Run(async () =>
            {

                await RunExAsync();
            });
            // RunEx(commamd);
        }
        string expfullpath = string.Empty;
        string command = string.Empty;
        string rundir = string.Empty;
        public void copyfile()
        {

            string sourceDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}GO-SPACE_{SpaceHelper.Gover}\\"; 
            string targetDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}GO-SPACE_{SpaceHelper.Gover}\\{Num}\\"; 

            rundir = targetDirectory;

            //MessageBox.Show("sourceDirectory:"+ sourceDirectory);
            //MessageBox.Show("targetDirectory:" + targetDirectory);

            try
            {
              
                string[] files = Directory.GetFiles(sourceDirectory);
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }
               
                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileName(filePath);
                    string targetFilePath = Path.Combine(targetDirectory, fileName);
                    File.Copy(filePath, targetFilePath, true);  

                }


            }
            catch (Exception ex)
            {
                f3.DisplayOutputWithMaxLines(ex.ToString(), 100);
            }
        }
        public async Task RunExAsync()
        {
            try
            {


                string targetDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}GO-SPACE_{SpaceHelper.Gover}\\{Num}"; ; // 目标目录路径
                rundir = targetDirectory;
                //Environment.CurrentDirectory = rundir;
                command = $" --listen /ip4/0.0.0.0/tcp/{port_pt} --config  {rundir}\\{Num}PostConfig.json  -d Spacemesh --filelock gosm2";
                //   string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
                var name = $"{rundir}\\go-spacemesh.exe";

                // MessageBox.Show(name);
                f3.DisplayOutputWithMaxLines(name + "  " + command, 100);
                ProcessStartInfo startInfo = new ProcessStartInfo
                {

                    FileName = name,
                    Arguments = command,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true, 
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = rundir
                };

                await Task.Run(() =>
                {
                    _process = new Process { StartInfo = startInfo };
                    _process.Start();
                    f3.DisplayOutputWithMaxLines("_process.Start", 100);
                    _cancellationTokenSource = new CancellationTokenSource();
                    Task outputTask = MonitorOutputAsync(_process, _cancellationTokenSource.Token);
                    Task errorTask = MonitorErrorAsync(_process, _cancellationTokenSource.Token);
                });
            }
            catch (Exception exx)
            {

                f3.DisplayOutputWithMaxLines(exx.ToString(), 100);
            }

        }

        F3 f3;
        private async Task MonitorOutputAsync(Process process, CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                string line = await process.StandardOutput.ReadLineAsync();
                if (line != null)
                {
                    // setstate("ok");
                    if (f3 != null && f3.Visible)
                    {
                        f3.Invoke(new DisplayOutputWithMaxLinesDelegate(f3.DisplayOutputWithMaxLines), line, 30);

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
                    if (f3 != null && f3.Visible)
                    {
                        f3.Invoke(new DisplayOutputWithMaxLinesDelegate(f3.DisplayOutputWithMaxLines), line, 30);

                    }

                }
            }
        }
        private void POST_Load(object sender, EventArgs e)
        {
            f3 = new F3($"{Num},{postPath},{W},{port_pt},{PostSize}GB");
            f3.Show();
            f3.Hide();
            this.label1.Text = Num;
            this.label3.Text = W;
            this.label7.Text = $"{PostSize}GB";
            this.label4.Text = port_pt;


            //Task.Run(() =>
            //            {
            //               RunPost();
            //            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f3.Show();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            KillProcess();
            await RunExAsync();
        }
    }
}
