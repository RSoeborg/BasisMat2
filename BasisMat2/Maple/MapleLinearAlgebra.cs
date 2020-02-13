using BasisMat2.Win;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasisMat2.Maple
{
    public sealed class MapleLinearAlgebra : MapleEngine
    {
        public MapleLinearAlgebra(string MaplePath) : base(MaplePath) { }
        
        public new void Open()
        {
            base.Open();
            IncludePackage("LinearAlgebra");
            IncludePackage("Student[LinearAlgebra]");
        }
        
        public async Task<MapleMatrix> ReducedRowEchelonForm(MapleMatrix Matrix) {
            var @output = await LPrint($"ReducedRowEchelonForm({Matrix})"); // reduce matrix to echelon form and transform it with lprint
            return new MapleMatrix(output); // create maple matrix from lprint and return
        }

        public async Task<IWindow> GaussianEliminationTutor(MapleMatrix matrix)
        {
            var WinList = new MSWinList();
            await Evaluate($"GaussJordanEliminationTutor({matrix});", false);
            IWindow window = default(IWindow);
            while (window == default(IWindow))
            {
                await Task.Delay(5);
                window = WinList.Windows.FirstOrDefault(c => c.Title.EndsWith("Gauss-Jordan Elimination"));
            }
            await Task.Delay(200);//wait for it to be loaded properly.
            return window;
        }
    }
}
