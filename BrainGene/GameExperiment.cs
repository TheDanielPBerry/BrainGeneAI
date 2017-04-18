using SharpNeat.Core;
using SharpNeat.Phenomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeEvolution;

namespace BrainGene
{
    class GameExperiment : SimpleNeatExperiment
    {
        public override IPhenomeEvaluator<IBlackBox> PhenomeEvaluator {
            get { return new GameEvaluator(); }
        }

        public override int InputCount {
            get { return 300; }
        }

        public override int OutputCount {
            get { return 3; }
        }

        public override bool EvaluateParents {
            get { return true; }
        }
    }
}
