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

            try
            {
                using (MySqlConnection conn = secrets.GetConnectionString())
                {
                    conn.Open();
                    conn.Close();
                }
            }
            catch { MessageBox.Show("There is no db connection. Exiting application..."); Application.Exit(); }
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
        /// Send 2 TCP Messages (of bytes) to a specific IP
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

            Thread.Sleep(500);
        }

        private void BtnToggleAll_Click(object sender, EventArgs e)
        {
            ToggleAll();
        }

        public void ToggleAll()
        {
            SendTCPMessage(WALL_IP, Commands.SELECTALL);
            SendTCPMessage(WALL_IP, Commands.SELECTALL2);
            SendTCPMessage(STAGE_IP, Commands.SELECTALL);
            SendTCPMessage(STAGE_IP, Commands.SELECTALL2);
            TxtOutput.Text += "Sent Toggle All" + Environment.NewLine;
            libmiroppb.Log("Sent Toggle All");
        }

        public void PowerAllOn()
        {
            SendTCPMessage(WALL_IP, Commands.POWERON_INTENSITY);
            SendTCPMessage(STAGE_IP, Commands.POWERON_INTENSITY);
            TxtOutput.Text += "Sent power on" + Environment.NewLine;
            libmiroppb.Log("Sent power on");
        }

        public void PowerAllOff()
        {
            SendTCPMessage(WALL_IP, Commands.POWEROFF_INTENSITY);
            SendTCPMessage(STAGE_IP, Commands.POWEROFF_INTENSITY);
            TxtOutput.Text += "Sent power off" + Environment.NewLine;
            libmiroppb.Log("Sent power off");
        }

        public void SundayLights()
        {
            //select outside and power on
            SendTCPMessage(STAGE_IP, Commands.SELECTGROUP1);
            SendTCPMessage(STAGE_IP, Commands.POWERON_INTENSITY);
            SendTCPMessage(STAGE_IP, Commands.SELECTGROUP1);
            //select inside
            SendTCPMessage(STAGE_IP, Commands.SELECTGROUP2);
            SendTCPMessage(STAGE_IP, Commands.SELECTGROUP3);
            //send power on to both groups
            SendTCPMessage(STAGE_IP, Commands.POWERON_INTENSITY);
            //set white to full
            SendTCPMessage(STAGE_IP, Commands.LIGHTSWHITE);
            //unselect 3rd device
            SendTCPMessage(STAGE_IP, Commands.SELECTGROUP3);
            //set magenta to 220
            SendTCPMessage(STAGE_IP, Commands.LIGHTSBLUE);
            //unselect 2nd device
            SendTCPMessage(STAGE_IP, Commands.SELECTGROUP2);
            //select 3rd device
            SendTCPMessage(STAGE_IP, Commands.SELECTGROUP3);
            //set cyan color to 220
            SendTCPMessage(STAGE_IP, Commands.LIGHTSRED);
            //unselecting group 3
            SendTCPMessage(STAGE_IP, Commands.SELECTGROUP3);
            TxtOutput.Text += "Sent Sunday Colors" + Environment.NewLine;
            libmiroppb.Log("Sent Sunday Colors");
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
            }
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

    }
}