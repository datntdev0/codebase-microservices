﻿namespace datntdev.Abp.Auditing
{
    public interface IClientInfoProvider
    {
        string BrowserInfo { get; }

        string ClientIpAddress { get; }

        string ComputerName { get; }
    }
}