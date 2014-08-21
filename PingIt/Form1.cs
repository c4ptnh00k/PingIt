using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Mail;

namespace PingIt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void btnStart_Click(object sender, EventArgs e)
        {
            var ips = new List<IPAddress>();
            foreach(IPAddress ip in ipList.Items)
            {
                var pingSender = new Ping();
                var options = new PingOptions {DontFragment = true};
                const string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                var buffer = Encoding.ASCII.GetBytes(data);
                const int timeout = 120;
                var failCount = 0;
                for (int i = 0; i < 3; i++)
                {
                    var reply = pingSender.Send(ip, timeout, buffer, options);
                    if (reply != null && reply.Status != IPStatus.Success)
                    {
                        failCount++;
                    }
                    if (failCount > 1)
                    {
                        Email(ip);
                    }
                    if (i == 2 && failCount <= 2)
                    {
                        ips.Add(ip);
                    }
                }
            }
                if (ips.Count > 0)
                {
                    var message = ips.Aggregate("The following pings were successfull " + "\r\n", (current, ip) => current + (ip.ToString() + "\r\n"));
                    const string caption = "Form Closing";
                    var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        txtIp.Clear();
                    }
                }
            }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            const string message =
                "Please enter a valid IP address";
            const string caption =
                "Form Closing";
            var ipstring = txtIp.Text;
            IPAddress address;
            if (IPAddress.TryParse(ipstring, out address))
            {
                ipList.Items.Add(address);
            }
            else
            {
                var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    txtIp.Clear();
                }
            }
        }
        
        private static void Email(IPAddress addr)
        {
            const string email = "PingIt@NOREPLY.COM";
            var message = new MailMessage();
            const string @from = email;
            const string to = "Colt.Stumpf@angeltrax.com";

            message.Body = "There was an unseccessful ping to: " + addr;

            message.From = new MailAddress(from);
            message.Subject = "Ping Test";
            message.To.Add(to);
            var mailer = new SmtpClient();
            mailer.Send(message);
        }
    }
}
