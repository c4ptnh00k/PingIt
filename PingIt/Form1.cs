using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            
            List<IPAddress> ips = new List<IPAddress>();
            foreach(IPAddress ip in ipList.Items)
            {
                
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;

                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                int failCount = 0;
                for (int i = 0; i < 3; i++)
                {
                    
                    PingReply reply = pingSender.Send(ip, timeout, buffer, options);
                    if (reply.Status != IPStatus.Success)
                    {
                        failCount++;
                    }
                
                    if (failCount > 1)
                    {
                        Email(reply, ip);
                        
                    }
                    if (i == 2 && failCount <= 2)
                    {
                        ips.Add(ip);
                    }
                }
                
                
            }
                if (ips.Count > 0)
                {
                    string message = "The following pings were successfull " + "\r\n" ;
                    foreach (IPAddress ip in ips)
                    {
                        message += ip.ToString() + "\r\n";
                    }
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
            if (System.Net.IPAddress.TryParse(ipstring, out address))
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
        
        private void Email(PingReply pr, IPAddress addr)
        {
            var email = "PingIt@NOREPLY.COM";
            var message = new MailMessage();
            string from = email;
            const string to = "Colt.Stumpf@angeltrax.com";

            message.Body = "There was an unseccessful ping to: " + addr.ToString();

            message.From = new MailAddress(from);
            message.Subject = "Ping Test";
            message.To.Add(to);
            var mailer = new SmtpClient();
            mailer.Send(message);
        }
    }
}
