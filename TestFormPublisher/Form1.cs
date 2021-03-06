﻿using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessageRouting.Shared.Service;
using MessageRouting.Shared.Model;


namespace TestFormPublisher
{
    public partial class Form1 : Form
    {
       
        private ISignalrPublisher Publiser { get; set; }

        public Form1()
        {
            InitializeComponent();
                        
            // http://localhost:50107/
            // MessageRoutingBus  

            //Publiser = new SignalrPublisher(@"http://localhost:50107/", "MessageRoutingBus");
            //Publiser = new SignalrPublisher(@"http://SESDEN822.vcn.ds.volvo.net:8122/", "MessageRoutingBus");
            //Publiser = new SignalrPublisher(@"http://localhost:50107/", "MessageRoutingBus");
            //Publiser = new SignalrPublisher(@"http://192.168.1.61/signalr/", "MessageRoutingBus");

            //Publiser.StatusChanged += Publiser_StatusChanged;
        }

        

        void Publiser_StatusChanged(object sender, string e)
        {
            if (lblStatus.InvokeRequired)
            {
                Invoke(new Action(() => Publiser_StatusChanged(sender, e)));
            }

            lblStatus.Text = e;
        }
        
        private void bntPublish_Click(object sender, EventArgs e)
        {
            var msg = new MessageRouting.Shared.Model.Message 
            {
                 Topic = tbxTopic.Text,
                 Text = tbxMessage.Text,
                 Level = tbxLevel.Text
            };

            Publiser.Publish(msg);
        }

        private void btnStartPublisher_Click(object sender, EventArgs e)
        {
            try
            {
                Publiser = new SignalrPublisher(tbxHostName.Text, tbxHub.Text);

                Publiser.StatusChanged += Publiser_StatusChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStartEmulation_Click(object sender, EventArgs e)
        {
            try
            {
                int cnt = Convert.ToInt32(tbxSendCount.Text);

                for (int i = 0; i < cnt; i++)
                {
                    var msg = new MessageRouting.Shared.Model.Message
                    {
                        Topic = tbxTopicEmulation.Text.Replace("[0]", i.ToString()),
                        Text = tbxMessageEmulation.Text.Replace("[0]", i.ToString()),
                        Level = tbxLevelEmulation.Text.Replace("[0]", i.ToString())
                    };

                    Publiser.Publish(msg);

                    lblEmulationStats.Text = string.Format("Stats: \r\n # {0}", i);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
