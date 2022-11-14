using Dapper;
using miroppb;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FSControl
{
    public partial class FrmMain : Form
    {
        const string STAGE_IP = "192.168.3.16";
        const string WALL_IP = "192.168.3.14";
        const int PORT_NUM = 3332;

        private List<ClSchedule> Actions = new List<ClSchedule>();

        public FrmMain()
        {
            InitializeComponent();

            libmiroppb.Log("Welcome to FSConrol :)");

            try
            {
                using (MySqlConnection conn = secrets.GetConnectionString())
                {
                    conn.Open();
                    conn.Close();
                }
            }
            catch { MessageBox.Show("There is no db connection. Exiting application..."); Application.Exit(); }

            using (MySqlConnection conn = secrets.GetConnectionString())
            {
                List<ClSchedule> _schedule = conn.Query<ClSchedule>("SELECT * FROM schedule").ToList();
                libmiroppb.Log("Using following schedule:");
                foreach (ClSchedule schedule in _schedule)
                {
                    Actions.Add(schedule);
                    libmiroppb.Log($"[{schedule.day}, {schedule.time.ToShortTimeString()}, {schedule.action}]");
                }
            }
        }

        private void BtnPowerOn_Click(object sender, EventArgs e)
        {
            PowerAllOn();
        }

        private void BtnPowerOff_Click(object sender, EventArgs e)
        {
            PowerAllOff();
        }

        private void BtnSundayStage_Click(object sender, EventArgs e)
        {
            SundayLights();
        }

        /// <summary>
        /// Send a TCP Message to a specific IP. Waits after command
        /// </summary>
        /// <param name="ip">IP to send the message to</param>
        /// <param name="message">Message to send</param>
        private void SendTCPMessage(string ip, string message)
        {
            using (TcpClient client = new TcpClient())
            {
                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), PORT_NUM);

                client.Connect(serverEndPoint);

                using (NetworkStream clientStream = client.GetStream())
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    clientStream.Write(buffer, 0, buffer.Length);
                    clientStream.Flush();
                }
            }
        }

        /// <summary>
        /// Send consecutive TCP Messages to a specific IP.
        /// </summary>
        /// <param name="ip">IP to send the message to</param>
        /// <param name="messages">Messages to send</param>
        private void SendTCPMessage(string ip, string[] messages)
        {
            using (TcpClient client = new TcpClient())
            {
                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), PORT_NUM);

                client.Connect(serverEndPoint);

                using (NetworkStream clientStream = client.GetStream())
                {
                    foreach (string message in messages)
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(message);
                        clientStream.Write(buffer, 0, buffer.Length);
                        clientStream.Flush();
                    }
                }
            }
            Thread.Sleep(200);
        }

        /// <summary>
        /// Send a TCP Message to a specific IP, and receive a response
        /// </summary>
        /// <param name="ip">IP to send the message to</param>
        /// <param name="message">Messages to send</param>
        /// <returns></returns>
        private string? SendAndReadTCPMessage(string ip, string message)
        {
            using (TcpClient client = new TcpClient())
            {
                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), PORT_NUM);

                client.Connect(serverEndPoint);

                using (NetworkStream clientStream = client.GetStream())
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    clientStream.Write(buffer, 0, buffer.Length);
                    byte[] data = new byte[1024];
                    int numBytesRead = clientStream.Read(data, 0, data.Length);
                    clientStream.Flush();
                    Thread.Sleep(200);
                    if (numBytesRead > 0)
                    {
                        return Encoding.ASCII.GetString(data, 0, numBytesRead);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// Test the connection to a specific IP on PORT_NUM
        /// </summary>
        /// <param name="ip">IP to test</param>
        /// <returns>Boolean of whether or not the connection was successful</returns>
        public bool TestConnection(string ip)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), PORT_NUM);
                    if (!client.ConnectAsync(serverEndPoint).Wait(1000))
                    {
                       return false;
                    }
                }
                return true;
            }
            catch { return false; }
        }

        private void BtnToggleAll_Click(object sender, EventArgs e)
        {
            ToggleAll();
        }

        public void ToggleAll()
        {
            SendTCPMessage(WALL_IP, new[] { Commands.SELECTALL, Commands.SELECTALL2 });
            SendTCPMessage(STAGE_IP, new[] { Commands.SELECTALL, Commands.SELECTALL2 });
            TxtOutput.Text += "Sent Toggle All" + Environment.NewLine;
            libmiroppb.Log("Sent Toggle All");
        }

        public void PowerAllOn()
        {
            //First check if all devices are selected. If there are any unselected, select all
            List<string> devices1 = SendAndReadTCPMessage(WALL_IP, Commands.GROUPSTATUS)!.Split(',').Skip(1).ToList();
            if (devices1.Any(x => x == "0"))
                SendTCPMessage(WALL_IP, new string[] { Commands.SELECTALL, Commands.SELECTALL2 });
            List<string> devices2 = SendAndReadTCPMessage(STAGE_IP, Commands.GROUPSTATUS)!.Split(',').Skip(1).ToList();
            if (devices1.Any(x => x == "0"))
                SendTCPMessage(STAGE_IP, new string[] { Commands.SELECTALL, Commands.SELECTALL2 });

            SendTCPMessage(WALL_IP, Commands.POWERON_INTENSITY);
            SendTCPMessage(STAGE_IP, Commands.POWERON_INTENSITY);
            TxtOutput.Text += "Sent power on" + Environment.NewLine;
            libmiroppb.Log("Sent power on");
        }

        public void PowerAllOff()
        {
            //First check if all devices are selected. If there are any unselected, select all
            List<string> devices1 = SendAndReadTCPMessage(WALL_IP, Commands.GROUPSTATUS)!.Split(',').Skip(1).ToList();
            if (devices1.Any(x => x == "0"))
                SendTCPMessage(WALL_IP, new string[] { Commands.SELECTALL, Commands.SELECTALL2 });

            List<string> devices2 = SendAndReadTCPMessage(STAGE_IP, Commands.GROUPSTATUS)!.Split(',').Skip(1).ToList();
            if (devices2.Any(x => x == "0"))
                SendTCPMessage(STAGE_IP, new string[] { Commands.SELECTALL, Commands.SELECTALL2 });

            SendTCPMessage(WALL_IP, Commands.POWEROFF_INTENSITY);
            SendTCPMessage(STAGE_IP, Commands.POWEROFF_INTENSITY);
            TxtOutput.Text += "Sent power off" + Environment.NewLine;
            libmiroppb.Log("Sent power off");
        }

        public void SundayLights()
        {
            SendTCPMessage(STAGE_IP, new[]
            {
                Commands.SELECTGROUP1, Commands.POWERON_INTENSITY, Commands.SELECTGROUP1, //select outside and power on
                Commands.SELECTGROUP2, Commands.SELECTGROUP3, //select inside
                Commands.POWERON_INTENSITY, //send power on to both groups
                Commands.LIGHTSWHITE, //set white to full
                Commands.SELECTGROUP3, //unselect 3rd device
                Commands.LIGHTSBLUE, //set cyan to 220
                Commands.SELECTGROUP2, //unselect 2nd device
                Commands.SELECTGROUP3, //select 3rd device
                Commands.LIGHTSRED, //set magenta color to 220
                Commands.SELECTGROUP3}); //unselecting group 3
            TxtOutput.Text += "Sent Sunday Colors" + Environment.NewLine;
            libmiroppb.Log("Sent Sunday Colors");
        }

        public void StageWhite()
        {
            SendTCPMessage(STAGE_IP, new[]
            {
                Commands.SELECTGROUP1, Commands.POWERON_INTENSITY, Commands.SELECTGROUP1, //select outside and power on
                Commands.SELECTGROUP2, Commands.SELECTGROUP3, //select inside
                Commands.POWERON_INTENSITY, //send power on to both groups
                Commands.LIGHTSWHITE, //set white to full
                Commands.SELECTGROUP2, Commands.SELECTGROUP3 //unselect all devices
            });
            TxtOutput.Text += "Sent Stage White" + Environment.NewLine;
            libmiroppb.Log("Sent Stage White");
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void toggleAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleAll();
        }

        private void powerOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PowerAllOn();
        }

        private void powerOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PowerAllOff();
        }

        private void sundayStageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SundayLights();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void scheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSchedule frm = new();
            frm.ValidActions.Clear();
            frm.ValidActions.AddRange(Enum.GetNames(typeof(ScheduleActions)));
            frm.ShowDialog(this);

            //re-read the schedules into dictionary
            Actions.Clear();

            using (MySqlConnection conn = secrets.GetConnectionString())
            {
                List<ClSchedule> _schedule = conn.Query<ClSchedule>("SELECT day, time, action FROM schedule").ToList();
                libmiroppb.Log("Using following schedule:");
                foreach (ClSchedule schedule in _schedule)
                {
                    Actions.Add(schedule);
                    libmiroppb.Log($"[{schedule.day}, {schedule.time.ToShortTimeString()}, {schedule.action}]");
                }
            }
        }

        private void timerSchedule_Tick(object sender, EventArgs e)
        {
            foreach (ClSchedule a in Actions)
            {
                if (DateTime.Now.DayOfWeek == a.day && DateTime.Now.ToShortTimeString() == a.time.ToShortTimeString() && a.action == ScheduleActions.TurnAllOn)
                {
                    PowerAllOn();
                    libmiroppb.Log("Turning All On");
                }
                else if (DateTime.Now.DayOfWeek == a.day && DateTime.Now.ToShortTimeString() == a.time.ToShortTimeString() && a.action == ScheduleActions.TurnAllOff)
                {
                    PowerAllOff();
                    libmiroppb.Log("Turning All Off");
                }
                else if (DateTime.Now.DayOfWeek == a.day && DateTime.Now.ToShortTimeString() == a.time.ToShortTimeString() && a.action == ScheduleActions.SundayLights)
                {
                    SundayLights();
                    libmiroppb.Log("Running Sunday Lights");
                }
                else if (DateTime.Now.DayOfWeek == a.day && DateTime.Now.ToShortTimeString() == a.time.ToShortTimeString() && a.action == ScheduleActions.ToggleAll)
                {
                    ToggleAll();
                    libmiroppb.Log("Running Toggle All");
                }
            }
        }

        private void BtnChangeLights_Click(object sender, EventArgs e)
        {
            if (CmbVariations.SelectedIndex > -1)
            {
                string val = CmbVariations.SelectedItem.ToString()!;
                object? cmd = typeof(Commands).GetField("LIGHTS_" + val)?.GetValue(this);
                ChangeCombo((string[])cmd!);
            }
        }

        public void ChangeCombo(string[] name)
        {
            foreach (string a in name)
            {
                SendTCPMessage(WALL_IP, a);
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            List<string> combo = new List<string>();
            foreach (var prop in typeof(Commands).GetFields())
            {
                if (prop.FieldType.BaseType!.FullName == "System.Array")
                    combo.Add(prop.Name.Replace("LIGHTS_", ""));
            }
            CmbVariations.Items.Clear();
            CmbVariations.Items.AddRange(combo.ToArray());
        }

        private void FrmMain_Deactivate(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }

    public static class Commands
    {
        static public readonly string SELECTALL = "FSOC000255";
        static public readonly string SELECTALL2 = "FSOC000000";
        static public readonly string POWERON_INTENSITY = "FSOC138255";
        static public readonly string POWEROFF_INTENSITY = "FSOC138000";
        static public readonly string SELECTGROUP1 = "FSOC034255";
        static public readonly string SELECTGROUP2 = "FSOC035255";
        static public readonly string SELECTGROUP3 = "FSOC036255";
        static public readonly string LIGHTSWHITE = "FSOC583255";
        static public readonly string LIGHTSBLUE = "FSOC132220";
        static public readonly string LIGHTSRED = "FSOC130220";

        static public readonly string GROUPSTATUS = "FSBC023000";

        static public readonly string[] LIGHTS_BLUE_PURPLE = { "FSOC130165", "FSOC131135", "FSOC132255" };
        static public readonly string[] LIGHTS_BLUE_COMBO = { "FSOC130013", "FSOC131034", "FSOC132255" };
        static public readonly string[] LIGHTS_LIGHT_BLUE = { "FSOC130021", "FSOC131243", "FSOC132255" };
        static public readonly string[] LIGHTS_HARVEST_YELLOW = { "FSOC130254", "FSOC131243", "FSOC132021" };
    }
}