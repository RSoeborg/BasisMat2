using BasisMat2.Maple;
using BasisMat2.Sniffing;
using BasisMat2.Sniffing.Outputs.PcapNg;
using BasisMat2.Win;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BasisMat2.JavaWin
{
    class JavaMapletInteractor
    {
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public async static Task<string[]> GaussianEliminationTutorResult(MapleLinearAlgebra engine, MapleMatrix matrix)
        {
            string[] @out = new string[] { };

            await semaphoreSlim.WaitAsync();
            try
            {
                IWindow window = await engine.GaussianEliminationTutor(matrix);
                if (window is MSWindow) // Microsoft Windows
                {
                    // begin sniffing here ..
                    var nics = NetworkInterfaceInfo.GetInterfaces();
                    var nic = nics.FirstOrDefault(c => c.Name.Contains("Loopback Pseudo"));

                    // ensure loopback pseudo interface is found...
                    if (nic != default(NetworkInterfaceInfo))
                    {
                        #region Start Sniffing
                        var appOptions = new AppOptions();
                        appOptions.Parse(new string[] { "" });
                        var filters = appOptions.BuildFilters();
                        var output = new PcapNgFileOutput(nic, appOptions.Filename);
                        var sniffer = new SocketSniffer(nic, filters, output);
                        sniffer.Start();
                        #endregion

                        await Task.Delay(1000 * 900);



                        #region MSWIN
                        var mswin = (MSWindow)window;
                        mswin.WindowPos(0, 0, 400, 800);

                        for (int i = 0; i < 4; i++)
                        {
                            mswin.SendKeyStroke(System.Windows.Forms.Keys.Tab);
                            await Task.Delay(60);
                        }

                        mswin.SendKeyStroke(System.Windows.Forms.Keys.Enter);
                        mswin.Hide();
                        await Task.Delay(1500);
                        mswin.Close();
                        #endregion

                        #region Interpret Sniffed Data
                        sniffer.Stop();
                        output.Dispose();


                        List<string> operations = new List<string>();
                        using (var reader = new StreamReader("snifter.pcapng"))
                        {
                            var content = reader.ReadToEnd();
                            var regex = new Regex(@"\<application_communications.*?\<content\>Applied operation\:\ (.*?)\<\/content\>", RegexOptions.Singleline);
                            var match = regex.Match(content);
                            
                            while (match.Success)
                            {
                                var operation = match.Groups[1].Value.Trim();//initial space
                                operations.Add(operation);
                                match = match.NextMatch();
                            }
                        }

                        @out = operations.ToArray();
                        #endregion
                    }

                }
                
            } finally
            {
                semaphoreSlim.Release();
            }
            
            return @out;
        }
        
    }
}
