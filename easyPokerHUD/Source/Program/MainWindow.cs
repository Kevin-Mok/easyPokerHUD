﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace easyPokerHUD
{
    public partial class MainWindow : Form
    {
        Timer fileWatcherStatusUpdateTimer = new Timer();

        public MainWindow()
        {
            InitializeComponent();

            //Set the version number label to the current version
            this.versionNumberLabel.Text = "v " + ApplicationUpdater.getCurrentProductVersion();

            //Start the filewatchers
            MainMethods.activateFileWatchers();

            //Start the timer that updates the console
            fileWatcherStatusUpdateTimer.Tick += updateHUDStatus;
            fileWatcherStatusUpdateTimer.Interval = 1500;
            fileWatcherStatusUpdateTimer.Start();

            errorMessage.Click += MainMethods.openQuickStartGuide;
        }

        //Gathers the current status of every filewatcher and displays it as a label 
        public void updateHUDStatus(Object obj, EventArgs e)
        {
            string[] statusStrings = MainMethods.getHUDStatusStrings();

            if (statusStrings[0].Equals(""))
            {
                successMessage.Text = "";
            }
            else
            {
                successMessage.Text = "HUD is up and running for " + statusStrings[0] + ".";
            }

            if (statusStrings[1].Equals(""))
            {
                errorMessage.Text = "";
                errorMessage.Hide();
                fileWatcherStatusUpdateTimer.Stop();
            }
            else
            {
                errorMessage.Text = "Could not find hand histories for " + statusStrings[1] + ". Click here to see how to fix this.";
            }
        }

        // Close every thread and write cached players to database
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //buyMessage.Text = "Writing players to database";
            PokerStarsMain.updatePlayersInDatabaseFromCache();
            EightPokerMain.updatePlayersInDatabaseFromCache();
           

            try
            {
                Environment.Exit(Environment.ExitCode);

            } catch (Exception p)
            {
                Console.WriteLine("Block 1");
                Console.WriteLine(p.Message);
            }
            
        }

        //Opens donation page
        private void donateButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://easypokerhud.com/join-hof/");
        }
    }
}
