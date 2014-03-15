using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    class LeNetTrainer
    {
        public LeNetTrainer()
        {

        }

        public void Initialise()
        {
            Console.WriteLine("Loading Training Data Set...");
            TrainingDataSet = DataSetItem.GetTrainingSet().Randomise(0);
            Console.WriteLine("Loading Generalisation Data Set...");
            GeneralisationDataSet = DataSetItem.GetGeneralisationSet().Randomise(1);

            Console.WriteLine("Creating LeNet...");
            Network = new LeNetNetwork('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
            Snapshot = new LeNetSnapshot(Network);
        }

        public async Task Train()
        {
            await Task.Run(new Action(() =>
                {
                    Console.WriteLine();
                    for (int i = 0; i < 50; i++)
                    {
                        Console.WriteLine("Run Epoch {0}", i);

                        Network.IsPreTraining = true;
                        DoEpoch(TrainingDataSet.Take(500));
                        Network.IsPreTraining = false;
                        DoEpoch(TrainingDataSet);
                    }
                    Console.WriteLine("Complete.");
                }
            ));

            
        }
    


        protected void DoEpoch(IEnumerable<DataSetItem> trainItems)
        {
            int correct = 0;
            int total = 0;
            foreach (DataSetItem item in trainItems)
            {
                TrainingResults result = Network.Train(item);
                if (result.Correct) correct++;
                total++;

                if (Snapshot.UpdateRequested) Snapshot.UpdateSnapshot();
                if (total % 10 == 0) UpdateStatus(correct, total);
            }
            Console.WriteLine();
        }

        private void UpdateStatus(int itemsCorrect, int itemsProcessed)
        {
            double currentAccuracy = (itemsCorrect * 100.0) / ((double)itemsProcessed);
            Console.CursorLeft = 0;
            Console.Write(currentAccuracy.ToString("000.00") + "% on " + itemsProcessed.ToString() + " items...");

        }

        public LeNetSnapshot Snapshot { get; protected set; }
        LeNetNetwork Network;
        IList<DataSetItem> TrainingDataSet;
        IList<DataSetItem> GeneralisationDataSet;
    }
}
