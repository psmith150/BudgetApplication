using BudgetApplication.IoC;
using BudgetApplication.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BudgetApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string COMPANY_NAME = "DMC";
        private const string APPLICATION_NAME = "BudgetApplication";
        private readonly string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), COMPANY_NAME, APPLICATION_NAME);

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //EventLog.Initialize(this.logPath);

            IoCContainer.Build();

            // Set the starting window
            this.MainWindow = IoCContainer.ResolveWindow(typeof(MainWindow));
            this.MainWindow.Show();
        }

        // Log any unhandled exceptions to the log directory in a CrashDump file
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string message;

            if (e.ExceptionObject is Exception)
            {
                try
                {
                    string directory;

                    try
                    {
                        directory = this.logPath ?? ".";
                    }
                    catch
                    {
                        directory = ".";
                    }

                    File.WriteAllText
                    (
                        Path.Combine(directory, "CrashDump - " + DateTime.Now.ToString("yyyyMMdd - HHmmss") + ".txt"),
                        ((Exception)e.ExceptionObject).ToString()
                    );
                }
                finally
                {
                    message = ((Exception)e.ExceptionObject).ToString();
                }
            }
            else
            {
                message = "An unknown error occurred.";
            }

            Debug.Assert(false, message);
            Environment.Exit(-1);
        }
    }
}
