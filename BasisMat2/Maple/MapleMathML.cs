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

        public async Task<string> Export(string MapleInput) {
            var ExportedValue = await Evaluate($"Export(\"{MapleInput}\");");
            StringBuilder Builder = new StringBuilder();

            string[] lines = ExportedValue.Replace("\r\n", "\n").Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                var Append = "";

                if (i == 0) {
                    Append = lines[i].Substring(1, lines[i].Length - 2);
                }
                else if (i == lines.Length - 1) { Append = lines[i].Substring(0, lines[i].Length - 2); }
                else
                {
                    Append = lines[i].Trim().Substring(0, lines[i].Length - 1);
                }


                Builder.Append(Append);
            }

            return Builder.ToString();
            //return ExportedValue.Replace("\\\r\n", "");
        } 

        private string Prettify(string MathML) {
            return MathML
                .Replace("RightArrow(", string.Empty)
                .Replace("]],", "]")
                .Replace("]])", "]")

                .Replace("]]", "]")
                .Replace("[[", " [")
                .Replace("#xe6", "æ")
                .Replace("#xe5", "å")
                .Replace("på", "på ");
        }
    }

    // RightArrow(
}
