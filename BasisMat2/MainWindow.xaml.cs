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

            btnTest.Click += (s, e) => {
                var engine = new MapleLinearAlgebra(Settings.Default.Path);

                var matrix = new MapleMatrix(new string[][]
                    {
                        new string[] {   "a",      "2",     "3"    },
                        new string[] {   "1",     "-4",   "-19"    },
                        new string[] {  "-1",      "3",    "14"    }
                    });

                Task.Run(async () => {
                    engine.Open();
                    await JavaWin.JavaMapletInteractor.GaussianEliminationTutorResult(engine, matrix);
                    engine.Close();

                });
                

                /*
                Task.Run(async () => {
                    engine.Open();
                    var operations = await JavaWin.JavaMapletInteractor.GaussianEliminationTutorResult(engine, matrix);
                    //engine.Close();

                    foreach (var operation in operations)
                    {
                        
                    }


                    rtOutput.Dispatcher.Invoke(new Action(() => {
                        rtOutput.AppendText(string.Join("\n", operations));
                    }));
                });
                */

            };


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
    }
}
