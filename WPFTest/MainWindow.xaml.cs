using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Diagnostics;

namespace WPFTest
{
    public partial class MainWindow : Window
    {

        static List<string>? prop;
        static List<string>? opp;
        static Random random = new();
        DispatcherTimer timer = new();
        Stopwatch stopwatch = new();
        string currentTime = string.Empty;
        System.Windows.Media.BrushConverter convert = new();
        System.Windows.Media.FontFamilyConverter fontConvert = new();
        bool timer_flag;
        bool protected_sound = false;
        bool scaleup;
        bool statement = false;
        string content;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SmallPopupHandler(object? text = null, RoutedEventArgs? e = null)
        {
            try
            {
                if (((Button)text).Tag.ToString() == "Close")
                {
                    verif_popup.IsOpen = false;
                }
                else
                {
                    VerifTextBlock.Text = text.ToString();
                    verif_popup.IsOpen = true;
                }
            }
            catch (Exception)
            {
                VerifTextBlock.Text = text.ToString();
                verif_popup.IsOpen = true;
            }
        }

        private void AllocateTeam(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Tag.ToString())
            {
                case "true":
                    prop = new List<string>();
                    opp = new List<string>();
                    List<string> DefaultList = new();
                    try
                    {
                        foreach (string s in File.ReadAllLines("Default.txt"))
                        {
                            DefaultList.Add(s);
                        }
                    }
                    catch (IOException)
                    {
                        OutputTeams.Text = "Could not locate default team save file!";
                    }

                    foreach (string s in DefaultList)
                    {
                        if (random.NextInt64(0, 2) == 0)
                        {
                            prop.Add(s);
                        }
                        else
                        {
                            opp.Add(s);
                        }
                    }
                    if (prop.Count != opp.Count)
                    {
                        if (prop.Count - opp.Count > 1)
                        {
                            opp.Add((string)prop[prop.Count - 1]);
                            prop.RemoveAt(prop.Count - 1);
                        }
                        else if (opp.Count - prop.Count > 0)
                        {
                            prop.Add((string)opp[opp.Count - 1]);
                            opp.RemoveAt(opp.Count - 1);
                        }
                    }
                    OutputHandler(prop, opp, Structure.SelectedIndex);

                    break;

                case "false":
                    prop = new List<string>();
                    opp = new List<string>();
                    string[] members = OutputTeams.Text.Split(
                        new string[] { Environment.NewLine },
                        StringSplitOptions.None);
                    foreach (string s in members)
                    {
                        if (random.NextInt64(0, 2) == 0)
                        {
                            prop.Add(s);
                        }
                        else
                        {
                            opp.Add(s);
                        }
                    }
                    if (prop.Count != opp.Count)
                    {
                        if (prop.Count - opp.Count > 1)
                        {
                            opp.Add((string)prop[prop.Count - 1]);
                            prop.RemoveAt(prop.Count - 1);
                        }
                        else if (opp.Count - prop.Count > 0)
                        {
                            prop.Add((string)opp[opp.Count - 1]);
                            opp.RemoveAt(opp.Count - 1);
                        }
                    }
                    OutputTeams.Text = "";
                    OutputHandler(prop, opp, Structure.SelectedIndex);
                    break;

                case "clear":
                    OutputTeams.Text = "";
                    break;
            }
        }

        private void HandleDefault(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Tag.ToString())
            {
                case "save":
                    File.WriteAllLines("Default.txt", DefaultInput.Text.Split('\n'));
                    SmallPopupHandler("Saved Successfully!");
                    break;

                case "recall":
                    try
                    {
                        foreach (string s in File.ReadAllLines("Default.txt"))
                        {
                            DefaultInput.AppendText(s + '\n');
                        }
                    }
                    catch (Exception)
                    {
                        SmallPopupHandler("Could not read file!\nHave you made a save file before?");
                    }
                    break;
            }

        }

        private void OutputHandler(List<string> prop, List<string> opp, int struc)
        {
            switch (struc)
            {
                case 0:
                    OutputTeams.Text = "Proposition:\n<-------->\n";
                    for (int z = 0; z < prop.Count; z++)
                    {
                        OutputTeams.AppendText(prop[z] + '\n');
                    }
                    OutputTeams.AppendText("\nOpposition:\n<-------->\n");
                    for (int z = 0; z < opp.Count; z++)
                    {
                        OutputTeams.AppendText(opp[z] + '\n');
                    }
                    break;

                case 1:
                    if (prop.Count < 4 || opp.Count < 4)
                    {
                        SmallPopupHandler("Not enough members for BP \nstructure!");
                        break;
                    }
                    OutputTeams.Text = "Proposition:\n<-------->\n";
                    OutputTeams.AppendText("Prime Minister: " + prop[0] + "\nDeputy: " + prop[1]);
                    int basis = (prop.Count-2) % 2;
                    int sections = (prop.Count-2) / 2;
                    for (int i = 0; i < 2; i++)
                    {
                        if (i == 0)
                        {
                            OutputTeams.AppendText("\nMember of Government: \n");
                            for (int j = 0; j < sections; j++)
                            {
                                try { OutputTeams.AppendText(prop[2 + j]+'\n');}catch (Exception ) { SmallPopupHandler("List size: "+ prop.Count.ToString()+"Called index "+(j+2).ToString()); }
                                
                            }
                        }
                        else
                        {
                            OutputTeams.AppendText("\nGovernment Whip: \n");
                            for (int j = 0; j < sections; j++)
                            {
                                try { OutputTeams.AppendText(prop[sections + 2 + j] + '\n'); } catch (Exception) { SmallPopupHandler("List size: " + prop.Count.ToString() + "Called index " + (sections+j + 2).ToString()); }
                            }
                            if (basis == 1)
                            {
                                OutputTeams.AppendText(prop[^1] + '\n');
                            }
                        }
                    }

                    OutputTeams.AppendText("\nOpposition:\n<-------->\n");
                    OutputTeams.AppendText("Leader of Opposition: " + opp[0] + "\nDeupty of Opposition: " + opp[1]);
                    basis = (opp.Count - 2) % 2;
                    sections = (opp.Count - 2) / 2;
                    for (int i = 0; i < 2; i++)
                    {
                        if (i == 0)
                        {
                            OutputTeams.AppendText("\nMember of Opposition: \n");
                            for (int j = 0; j < sections; j++)
                            {
                                OutputTeams.AppendText(opp[2 + j] + '\n');
                            }
                        }
                        else
                        {
                            OutputTeams.AppendText("\nOpposition Whip: \n");
                            for (int j = 0; j < sections; j++)
                            {
                                OutputTeams.AppendText(opp[sections + 2 + j] + '\n');
                            }
                            if (basis == 1)
                            {
                                OutputTeams.AppendText(opp[^1] + '\n');
                            }
                        }
                    }
                    break;
            }
        }

        private void TimerHandler(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                scaleup = true;
                Timer_label.FontSize = 160;
            }
            else
            {
                scaleup = false;
                Timer_label.FontSize = 48;
            }
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            switch (((Button)sender).Tag)
            {
                case "Start":
                    timer_flag = true;
                    stopwatch.Start();
                    timer.Start();
                    
                    break;

                case "Stop":
                    Timer_label.Foreground = (System.Windows.Media.Brush)convert.ConvertFromString("#000000");
                    if (timer_flag)
                    {
                        if (stopwatch.IsRunning)
                        {
                            timer.Stop();
                            stopwatch.Stop();
                        }
                        timer_flag= false;
                    }
                    else
                    {
                        stopwatch.Reset();
                        Timer_label.FontFamily = (System.Windows.Media.FontFamily)fontConvert.ConvertFromString("Digital-7 Mono");
                        if (!scaleup)
                        {
                            Timer_label.FontSize = 48;
                        }
                        Timer_label.Content = "--:--";
                        protected_sound = false;
                    }
                    break;

            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (stopwatch.IsRunning)
            {
                TimeSpan ts = stopwatch.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}",ts.Minutes, ts.Seconds);
                Timer_label.Content = currentTime;

                if ((bool)ProtectedTime.IsChecked)
                {
                    if(stopwatch.Elapsed.TotalSeconds < 60 || stopwatch.Elapsed.TotalSeconds > 360)
                    {
                        if (!protected_sound && (stopwatch.Elapsed.TotalSeconds < 60 || stopwatch.Elapsed.TotalSeconds > 360))
                        {
                            Timer_label.Foreground = (System.Windows.Media.Brush)convert.ConvertFromString("#eb4034");
                            System.Media.SystemSounds.Hand.Play();
                            protected_sound = true;
                        }
                        else if (stopwatch.Elapsed.TotalSeconds > 420)
                        {
                            System.Media.SystemSounds.Beep.Play();
                            timer.Stop();
                            stopwatch.Stop();
                            Timer_label.Foreground = (System.Windows.Media.Brush)convert.ConvertFromString("#4287f5");
                            if (!scaleup)
                            {
                                Timer_label.FontSize = 30;
                            }
                            else
                            {
                                Timer_label.FontSize = 110;
                            }
                            Timer_label.FontFamily = (System.Windows.Media.FontFamily)fontConvert.ConvertFromString("Bahnschrift Light");
                            Timer_label.Content = "Debate Section Elapsed";
                            timer_flag = false;
                            protected_sound = false;
                        }
                    }
                    else
                    {
                        if (protected_sound)
                        {
                            Timer_label.Foreground = (System.Windows.Media.Brush)convert.ConvertFromString("#000000");
                            System.Media.SystemSounds.Hand.Play();
                            protected_sound= false;
                        }
                    }
                }
            }
        }

        private void StatementHandler(object sender, RoutedEventArgs e)
        {
            if ((bool)ShowStatement.IsChecked && App.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                if (!statement)
                {
                    Statement_popup.IsOpen = true;
                }
                Statement_label.Visibility = 0;
                statement = true;
            }
            else if ((bool)!ShowStatement.IsChecked)
            {
                Statement_label.Visibility = (System.Windows.Visibility)1;
            }
            
        }

        private void CloseStatementHandler(object sender, RoutedEventArgs e)
        {
            content = StatementInput.Text;
            Statement_label.Text = content;
            Statement_popup.IsOpen=false;
        }

        private void ResetStatement(object sender, RoutedEventArgs e)
        {
            statement = false;
            StatementHandler(sender, e);
        }
    }
}
