using BasisMat2.Win;
using BasisMat2.Maple;
using BasisMat2.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Diagnostics;
using System.Windows.Threading;

namespace BasisMat2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateMaplePath();

            #region Test (Kan blive slettet NP)
            btnTest.Click += (s, e) => {
                var engine = new MapleLinearAlgebra(Settings.Default.Path);

                var matrix = new MapleMatrix(new string[][]
                    {
                        new string[] {   "a",      "2",     "3"    },
                        new string[] {   "1",     "-4",   "-19"    },
                        new string[] {  "-1",      "3",    "14"    }
                    });

                btnTest.IsEnabled = false;
                btnTest.Content = "Udregner...";

                Task.Run(async () => {
                    engine.Open();
                    var operations = await JavaWin.JavaMapletInteractor.GaussianEliminationTutorResult(engine, matrix);
                    rtOutput.Dispatcher.Invoke(new Action(() => {
                        rtOutput.Document.Blocks.Clear();
                        rtOutput.AppendText(string.Join("\n", operations));

                        btnTest.Content = "Udregn Matrix";
                        btnTest.IsEnabled = true;
                    }));

                    engine.Close();
                });    
            };
            #endregion



        }

        private void BtnMapleSelect_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            var result = ofd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                if (ofd.FileName.Contains("maplew.exe") || ofd.FileName.Contains("cmaple.exe"))
                {
                    var file = new FileInfo(ofd.FileName);
                    var cmaple = file.Name.Equals("cmaple.exe") ? file : file.Directory.GetFiles().FirstOrDefault(f => f.Name.Equals("cmaple.exe"));

                    if (cmaple != default(FileInfo))
                    {
                        Settings.Default.Path = cmaple.FullName;
                        Settings.Default.Save();
                        Settings.Default.Reload();
                        UpdateMaplePath();
                        return;
                    }
                }
                MessageBox.Show("Maple Command Line kunne ikke findes.");
            }
        }
        private void UpdateMaplePath()
        {
            if (!string.IsNullOrWhiteSpace(Settings.Default.Path))
            {
                FileInfo cmaple = new FileInfo(Settings.Default.Path);
                lblMaplePath.Content = $"Path: {cmaple.Directory.Parent.FullName}";
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void toolbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
