using SharpNeat.Core;
using SharpNeat.Phenomes;
using System;
using System.Threading;

namespace BrainGene
{
    class GameEvaluator : IPhenomeEvaluator<IBlackBox>
    {
        private ulong _evalCount;
        private bool _stopConditionSatisfied;

        public ulong EvaluationCount
        {
            get { return _evalCount; }
        }

        public bool StopConditionSatisfied
        {
            get { return _stopConditionSatisfied; }
        }

        public FitnessInfo Evaluate(IBlackBox phenome)
        {
            double fitness = 0;
            NeatPlayer neatPlayer = new NeatPlayer(phenome);

            int threadId = Thread.CurrentThread.ManagedThreadId;
            Game game = new Game(neatPlayer, threadId);
            fitness = game.PlayGameToEnd();
            // Update the evaluation counter.
            _evalCount++;
            if (fitness >= 4000)
            {
                _stopConditionSatisfied = true;
            }
            return new FitnessInfo(fitness, fitness);
        }

        public void Reset()
        {
            
        }

        
    }
}
