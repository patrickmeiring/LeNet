using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdvancedOCR
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LeNetTrainer trainer = new LeNetTrainer();
            trainer.Initialise();
            trainer.Train();

            LeNetObservationForm observationForm = new LeNetObservationForm(trainer.Snapshot);
            Application.Run(observationForm);
        }

    }
}
