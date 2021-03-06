﻿using Topshelf;
using Topshelf.Runtime;

namespace Uchilka.WinService
{
    static class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(c =>
            {
                c.Service<WinService>(s =>
                {
                    s.ConstructUsing(x => ConstructService(x));
                    s.WhenStarted((service, control) => service.Start(control));
                    s.WhenStopped((service, control) => service.Stop(control));
                });

                c.StartAutomatically();
                c.RunAsLocalSystem();
                c.SetDescription("Uchilka Service");
                c.SetDisplayName("Uchilka Service");
                c.SetServiceName("UchilkaSvc");
            });
        }

        public static WinService ConstructService (HostSettings settings)
        {
            return new WinService();
        }
    }
}
