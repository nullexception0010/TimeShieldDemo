using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddApp(object sender, RoutedEventArgs e)
        {
            AppBlocking.AddToBlockedApp(App.Text.ToString());
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

    }
}
