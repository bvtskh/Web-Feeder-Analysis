using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace FeederAnalysis.Business
{
    public class NetworkHelper
    {
        public static bool CheckConnect(string ip)
        {
            Ping ping = new Ping();
            //change the following ip variable into the ip adress you are looking for
            IPAddress address = IPAddress.Parse(ip);
            PingReply pong = ping.Send(address);
            if (pong.Status == IPStatus.Success)
            {
                return true;
            }
            return false;
        }

        private static void Try_catch(Action action)
        {
            try
            {
                action();
            }
            catch
            {
            }
        }
    }
}
