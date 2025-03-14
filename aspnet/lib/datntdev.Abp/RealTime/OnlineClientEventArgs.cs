﻿using System;

namespace datntdev.Abp.RealTime
{
    public class OnlineClientEventArgs : EventArgs
    {
        public IOnlineClient Client { get; }

        public OnlineClientEventArgs(IOnlineClient client)
        {
            Client = client;
        }
    }
}