using SharpNeat.Phenomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainGene
{
    class NeatPlayer
    {
        public IBlackBox Brain { get; set; }

        public NeatPlayer(IBlackBox brain)
        {
            Brain = brain;
        }

        public byte[] GetMove(byte[] screen)
        {
            Brain.ResetState();
            for (int i=0; i<screen.Length; i++) {
                Brain.InputSignalArray[i] = screen[i];
            }
            Brain.Activate();
            byte[] res = new byte[3];
            for (short i = 0; i < res.Length; i++)
            {
                res[i] = (byte)(Brain.OutputSignalArray[i]>0.5?1:0);
            }
            return res;
        }
    }
}
