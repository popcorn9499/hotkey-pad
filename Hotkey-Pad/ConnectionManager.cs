﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Hotkey_Pad
{
    public class ConnectionManager
    {
        private string ip;
        private int port;
        private lvServerListItem lvItem;
        private const string CONNECTION_ATTEMPING = "Connecting";
        private const string CONNECTED = "Connected!";
        private bool closeConnection = false;

        public ConnectionManager(String ip, int port, lvServerListItem lvItem)
        {
            this.ip = ip;
            this.port = port;
            this.lvItem = lvItem;
            this.createConnection();
        }

        public void close()
        {
            this.closeConnection = true;
        }

        private async void createConnection()
        {
            //string host = "127.0.0.1";
            int timeout = 5000;

            while (true) //continuously trys to reconnect
            {
                try //catches any errors that may occur
                {
                    lvItem.Connection_Status = CONNECTION_ATTEMPING;
                    TcpClient client = new TcpClient();

                    NetworkStream netstream;
                    StreamReader reader;
                    StreamWriter writer;

                    await client.ConnectAsync(this.ip, this.port); //connects to the host on port specified
                    netstream = client.GetStream();

                    //gets the stream reader and writer
                    reader = new StreamReader(netstream);
                    writer = new StreamWriter(netstream);


                    //sets details for the connection
                    writer.AutoFlush = true;

                    netstream.ReadTimeout = timeout;

                    String serverName = this.ip + ":" + this.port.ToString(); //create the name of the server for the serverList
                    //Program.AddToList(serverName, writer);

                    //create this as a async task so it can be canceled if the connection has been ended or use another method to remove it
                    this.lvItem.Connection_Status = CONNECTED;
                    while (true) //start reading for messages
                    {
                        String response = await reader.ReadLineAsync();
                        if (this.closeConnection)
                        {
                            break;
                        }
               
                    }


                }
                catch (Exception e) {}
            }
        }
    }
}
