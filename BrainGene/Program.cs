using log4net.Config;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BrainGene
{
    class Program
    {

        const bool LEARN = false;
        static NeatEvolutionAlgorithm<NeatGenome> _ea;
        const string CHAMPION_FILE = "game_champion_gen_{0}.xml";

        static void Main(string[] args)
        {
            //Initialise log4net (log to console).
            XmlConfigurator.Configure(new FileInfo("log4net.properties"));

            // Experiment classes encapsulate much of the nuts and bolts of setting up a NEAT search.
            GameExperiment experiment = new GameExperiment();

            // Load config XML.
            XmlDocument xmlConfig = new XmlDocument();
            xmlConfig.Load("game.config.xml");
            experiment.Initialize("BrainGene", xmlConfig.DocumentElement);

            if (LEARN)
            {
                // Create evolution algorithm and attach update event.
                _ea = experiment.CreateEvolutionAlgorithm();
                _ea.UpdateEvent += new EventHandler(ea_UpdateEvent);

                //Start algorithm (it will run on a background thread).
                _ea.StartContinue();

                //Hit return to quit.
                Console.ReadLine();
            }
            else
            {
                // Have the user choose the genome XML file.

                NeatGenome genome = null;

                // Try to load the genome from the XML document.
                using (XmlReader xr = XmlReader.Create("C:\\Users\\Cantino\\Documents\\School\\Capstone\\NeatTicTacToe\\BrainGene\\bin\\Debug\\game_champion_gen_0.xml"))
                    genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, (NeatGenomeFactory)experiment.CreateGenomeFactory())[0];

                // Get a genome decoder that can convert genomes to phenomes.
                var genomeDecoder = experiment.CreateGenomeDecoder();

                // Decode the genome into a phenome (neural network).
                var phenome = genomeDecoder.Decode(genome);

                // Set the NEAT player's brain to the newly loaded neural network.
                NeatPlayer player = new NeatPlayer(null);
                player.Brain = phenome;
                Game game = new Game(player, 0);
                Console.WriteLine(game.PlayGameToEnd());
                // Show the option to select the NEAT player
                Console.ReadLine();
            }
        }

        static void ea_UpdateEvent(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format("gen={0:N0} bestFitness={1:N6}", _ea.CurrentGeneration, _ea.Statistics._maxFitness));
            // Save the best genome to file
            var doc = NeatGenomeXmlIO.SaveComplete(new List<NeatGenome>() { _ea.CurrentChampGenome }, false);
            doc.Save(string.Format(CHAMPION_FILE, _ea.CurrentGeneration));
        }
    }
}
