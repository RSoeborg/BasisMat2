using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasisMat2.Sniffing.Outputs.PcapNg
{
    public interface IBlock
    {
        byte[] GetBytes();
    }
}
