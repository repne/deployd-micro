﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceProcess;
using SimpleServices;
using deployd.watchman.AppStart;
using ServiceInstaller = System.ServiceProcess.ServiceInstaller;

namespace deployd.watchman
{
    [RunInstaller(true)]
    public class Program : ServiceInstaller
    {
        private static void Main(string[] args)
        {
            new Service(args,
                        new List<IWindowsService>{ new NancyUi() }.ToArray,
                        installationSettings: (serviceInstaller, serviceProcessInstaller) =>
                            {
                                serviceInstaller.ServiceName = "deployd.watchman";
                                serviceInstaller.StartType = ServiceStartMode.Automatic;
                                serviceProcessInstaller.Account = ServiceAccount.NetworkService;
                            },
                        configureContext: x => { x.Log = Console.WriteLine; })
                .Host();
        }
    }
}
