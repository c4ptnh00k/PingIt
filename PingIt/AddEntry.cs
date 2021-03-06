﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace PingIt
{
    public partial class AddEntry : Form
    {
        public AddEntry()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var db = new PingItEntities();

            var newCraddlePoint = new CraddlePoint
            {
                IpAddress = txtIp.Text,
                Organization = txtOrganization.Text,
                VehicleId = txtVehicle.Text,
                Provider = txtProvider.Text
            };
            db.CraddlePoints.Add(newCraddlePoint);
            db.SaveChanges();
            
        }

        
    }
}
