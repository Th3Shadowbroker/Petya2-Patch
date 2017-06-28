using Petya_2_Patcher.Classes;
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

namespace Petya_2_Patcher
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Patch CurrentPatch = null;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += OnLoad;
        }

        /// <summary>
        /// Triggered when loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnLoad( object sender, EventArgs e )
        {

            //Request
            MessageBoxResult Req = MessageBox.Show( "You use this tool at your own risk!", "Important!", MessageBoxButton.OKCancel, MessageBoxImage.Warning );

            //Abort if user disagreed
            if ( Req == MessageBoxResult.Cancel )
            {
                Application.Current.Shutdown();
            }

            //Load patch
            try
            {
                CurrentPatch = new Patch();
            }
            catch ( Exception ex )
            {
                MessageBox.Show( "Something went wrong during initialization!", "Important!", MessageBoxButton.OK, MessageBoxImage.Error );
                Application.Current.Shutdown();
            }

            //Update status label
            this.updateStatus();

        }

        /// <summary>
        /// Update the status label
        /// </summary>
        private void updateStatus()
        {
            //Patched
            if ( CurrentPatch.isPatched() )
            {
                StatusLabel.Content = "Current State: Patched";
                RunPatch.Content = "Remove patch from registry";
            }

            //Not patched
            else
            {
                StatusLabel.Content = "Current State: Not patched";
                RunPatch.Content = "Patch registry";
            }
        }

        /// <summary>
        /// When the patch button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunPatch_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                //Do unpatch
                if ( CurrentPatch.isPatched() )
                {
                    CurrentPatch.unpatch();

                    ProcessStartInfo CmdRunInfo = new ProcessStartInfo();
                    CmdRunInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\cmd.exe";
                    CmdRunInfo.Arguments = "/C net start winmgmt /Q";
                    Process.Start( CmdRunInfo );

                    this.updateStatus();
                }

                //Do patch
                else
                {
                    CurrentPatch.patch();

                    ProcessStartInfo CmdRunInfo = new ProcessStartInfo();
                    CmdRunInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\cmd.exe";
                    CmdRunInfo.Arguments = "/C net stop winmgmt /Q";
                    Process.Start(CmdRunInfo);

                    this.updateStatus();
                }

            }
            catch( Exception ex )
            {
                MessageBox.Show( "Something went wrong!", "Error", MessageBoxButton.OK, MessageBoxImage.Error );
            }

        }
    }
}
