using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TimeShield
{
    internal class AppBlocking
    {
        static bool blocking = false;
        const string filePath = "C:\\blocked_apps.txt";

        public static void AddToBlockedApp(string app)
        {
            blocking = false;
            // Sleep for 1/2 second
            Thread.Sleep(500);

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                // Create a new file
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    writer.WriteLine(app);
                }
            }
            else
            {
                using (StreamWriter writer = File.AppendText(filePath))
                {
                    writer.WriteLine(app);
                }
            }
        }

        public static void ClearList()
        {
            if (!blocking)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        MessageBox.Show("Blocked Apps List Cleared");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to block app: {ex.Message}");
                }
            }
        }

        public static void StartBlocking()
        {
            blocking = true;
            Thread blocking_thread = new Thread(BlockApps);
            blocking_thread.Start();
        }

        public static void StopBlocking()
        {
            blocking = false;
        }

        public static void BlockApps()
        {
            while (blocking)
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Process[] processes = Process.GetProcessesByName(line);

                        if (processes.Length > 0)
                        {
                            // Close app if process found
                            foreach (Process process in processes)
                            {
                                try
                                {
                                    process.Kill(); // Terminate the process
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception($"Failed to block app: {ex.Message}");
                                }
                            }
                        }
                        // Sleep for 1 second
                        Thread.Sleep(1000);
                    }
                }
            } 
        }
    }
}
