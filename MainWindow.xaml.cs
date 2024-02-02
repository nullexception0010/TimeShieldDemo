using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeShield
{
    public partial class MainWindow : Window
    {
        private string selectedAppToBlock; //store the selected app the user wants to block themselves from using
        private Button currentlySelectedButton; //track the currently selected button (item in the scrollview list)

        public MainWindow()
        {
            InitializeComponent();

            RunRefreshLogic();
        }

        private void AddApp(object sender, RoutedEventArgs e)
        {
            AppBlocking.AddToBlockedApp(selectedAppToBlock.ToString());
        }

        private void BlockApps(object sender, RoutedEventArgs e)
        {
            AppBlocking.StartBlocking();
        }

        private void UnblockApps(object sender, RoutedEventArgs e)
        {
            AppBlocking.StopBlocking();
        }

        private void ClearAppList(object sender, RoutedEventArgs e)
        {
            AppBlocking.ClearList();
        }


        private void BlockWebsites(object sender, RoutedEventArgs e)
        {
            WebsiteBlocking.BlockWebsite(Website.Text);
            WebsiteBlocking.FlushDNSCache();
        }

        private void UnblockWebsites(object sender, RoutedEventArgs e)
        {
            WebsiteBlocking.UnblockWebsite(Website.Text);
            WebsiteBlocking.FlushDNSCache();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            // Clear existing items
            stackPanel.Children.Clear();

            // Get the list of running processes
            Process[] processes = Process.GetProcesses();

            // Use a HashSet to store unique app names
            HashSet<string> uniqueApps = new HashSet<string>();

            foreach (Process process in processes)
            {
                try
                {
                    // Get the process name
                    string appName = process.ProcessName;

                    // Check if the app name is not already in the HashSet
                    if (uniqueApps.Add(appName))
                    {
                        // Create a Button for each app
                        Button appButton = new Button
                        {
                            Content = appName,
                            Tag = appName, // Store the app name in the Tag property for later reference
                            Background = System.Windows.Media.Brushes.White // Set the default background color to white

                        };

                        // Handle the Click event for the Button
                        appButton.Click += AppButton_Click;

                        // Add the Button to the stack panel
                        stackPanel.Children.Add(appButton);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions if needed
                    MessageBox.Show($"Error retrieving process information: {ex.Message}");
                }
            }
        }

        private void AppButton_Click(object sender, RoutedEventArgs e)
        {
            //retrieve the app name from the Tag property
            string clickedApp = (string)((Button)sender).Tag;

            //if the clicked app is the same as the previously selected one, unhighlight it
            if (currentlySelectedButton != null && clickedApp == selectedAppToBlock)
            {
                currentlySelectedButton.Background = System.Windows.Media.Brushes.White;
                selectedAppToBlock = null;
                currentlySelectedButton = null;
            }
            else
            {
                //unhighlight the previously selected button
                if (currentlySelectedButton != null)
                {
                    currentlySelectedButton.Background = System.Windows.Media.Brushes.White;
                }

                //set the background of the newly clicked button to light blue
                ((Button)sender).Background = System.Windows.Media.Brushes.LightBlue;

                //set the currently selected button and app name to the newly clicked button
                currentlySelectedButton = (Button)sender;
                selectedAppToBlock = clickedApp;
            }
        }

        //this one is here to run once at the beginning, it wasn't working when I tried running refresh_click at the start
        private void RunRefreshLogic()
        {
            // Clear existing items
            stackPanel.Children.Clear();

            // Get the list of running processes
            Process[] processes = Process.GetProcesses();

            // Use a HashSet to store unique app names
            HashSet<string> uniqueApps = new HashSet<string>();

            foreach (Process process in processes)
            {
                try
                {
                    // Get the process name
                    string appName = process.ProcessName;

                    // Check if the app name is not already in the HashSet
                    if (uniqueApps.Add(appName))
                    {
                        // Create a Button for each app
                        Button appButton = new Button
                        {
                            Content = appName,
                            Tag = appName, // Store the app name in the Tag property for later reference
                            Background = System.Windows.Media.Brushes.White // Set the default background color to white
                        };

                        // Handle the Click event for the Button
                        appButton.Click += AppButton_Click;

                        // Add the Button to the stack panel
                        stackPanel.Children.Add(appButton);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions if needed
                    MessageBox.Show($"Error retrieving process information: {ex.Message}");
                }
            }
        }

    }
}
