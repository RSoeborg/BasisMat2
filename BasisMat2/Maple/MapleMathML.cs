using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasisMat2.Maple
{
    class MapleMathML : MapleEngine
    {
        public MapleMathML(string MaplePath) : base(MaplePath) { }

        public new void Open()
        {
            base.Open();
            IncludePackage("MathML");
        }

        public async Task<string> Import(string MathML)
        {
            var ML = await Evaluate($"Import(\"{MathML}\");");
            return Prettify(ML);
        }

        private string Prettify(string MathML) {
            return MathML.Replace("RightArrow(", string.Empty);
        }
    }

    // RightArrow(
}
