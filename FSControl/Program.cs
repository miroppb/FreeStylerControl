using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

namespace FSControl
{
    internal static class Program
    {
        public static FrmMain? frm { get; private set; } = null;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().RunAsync();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            frm = new FrmMain();
            Application.Run(frm);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
             WebHost.CreateDefaultBuilder(args).UseUrls("http://*:1114")
                 .UseStartup<Startup>();
    }
}