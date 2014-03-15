using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedOCR
{
    class LeNetNetwork
    {
        public InputStep inputLayer { get; protected set; }

        const int FirstConvolutionCount = 6;
        const int FirstConvolutionSize = 5;
        public ConvolutionStep[] FirstConvolutions { get; protected set; }
        public SubsamplingStep[] FirstSubsampling { get; protected set; }

        const int SecondConvolutionCount = 16;
        const int SecondConvolutionSize = 5;

        readonly bool[,] SecondConvolutionConnections = new bool[16,6] {
                {true ,true ,true ,false,false,false}, 
                {false,true ,true ,true ,false,false},
                {false,false,true ,true ,true ,false},
                {false,false,false,true ,true ,true },
                {true ,false,false,false,true ,true },
                {true ,true ,false,false,false,true },
                
                {true ,true ,true ,true ,false,false}, 
                {false,true ,true ,true ,true ,false},
                {false,false,true ,true ,true ,true },
                {true ,false,false,true ,true ,true },
                {true ,true ,false,false,true ,true },
                {true ,true ,true ,false,false,true },

                {true ,true ,false,true ,true ,false},
                {false,true ,true ,false,true ,true },
                {true ,false,true ,true ,false,true },

                {true ,true ,true ,true ,true ,true }
            };

        public ConvolutionStep[] SecondConvolutions { get; protected set; }
        public SubsamplingStep[] SecondSubsampling { get; protected set; }

        const int ConsolidationNeurons = 120;
        public const int OutputFeedForwardNeurons = 84;


        public FeedForwardStep Consolidation { get; protected set; }
        public FeedForwardStep Output { get; protected set; }
        public MarkingStep Marking { get; protected set; }

        protected Step[] forwardSteps;
        protected Step[] reverseSteps;

        protected LeNetConfiguration configuration;

        public LeNetNetwork(params char[] characters)
        {
            Weights.GlobalLearningRate = 0.00005;
            Weights.GlobalMu = 0.02;
            configuration = LeNetConfiguration.FromCharacters(characters);
            CreateNetwork();
        }

        private void CreateNetwork()
        {
            InstanciateSteps();
            CreateStepLists();
        }

        private void InstanciateSteps()
        {
            inputLayer = new InputStep(32, 32);
            FirstConvolutions = new ConvolutionStep[FirstConvolutionCount];
            FirstSubsampling = new SubsamplingStep[FirstConvolutionCount];
            for (int i = 0; i < FirstConvolutionCount; i++)
            {
                ConvolutionStep convolutionStep = new ConvolutionStep(inputLayer, FirstConvolutionSize);
                FirstConvolutions[i] = convolutionStep;
                FirstSubsampling[i] = new SubsamplingStep(convolutionStep, 2);
            }

            SecondConvolutions = new ConvolutionStep[SecondConvolutionCount];
            SecondSubsampling = new SubsamplingStep[SecondConvolutionCount];
            for (int i = 0; i < SecondConvolutionCount; i++)
            {
                RectangularStep[] inputs = FirstSubsampling.Where((item, upstreamIndex) => SecondConvolutionConnections[i, upstreamIndex]).ToArray();
                ConvolutionStep convolutionStep = new ConvolutionStep(inputs, SecondConvolutionSize);
                SecondConvolutions[i] = convolutionStep;
                SecondSubsampling[i] = new SubsamplingStep(convolutionStep, 2);
            }

            Consolidation = new FeedForwardStep(120, SecondSubsampling);

            Output = new FeedForwardStep(OutputFeedForwardNeurons, Consolidation);
            Marking = new MarkingStep(Output, configuration);
        }

        private void CreateStepLists()
        {
            List<Step> steps = new List<Step>();
            
            steps.AddRange(FirstConvolutions);
            steps.AddRange(FirstSubsampling);
            steps.AddRange(SecondConvolutions);
            steps.AddRange(SecondSubsampling);
            steps.Add(Consolidation);
            steps.Add(Output);
            steps.Add(Marking);

            this.forwardSteps = steps.ToArray();
            steps.Reverse();
            this.reverseSteps = steps.ToArray();
        }

        private bool preTraining;
        public bool IsPreTraining
        {
            get { return preTraining; }
            set
            {
                if (preTraining != value)
                {
                    preTraining = value;
                    Array.ForEach(forwardSteps, step => step.IsPreTraining = preTraining);
                }
            }
        }

        public void PropogateForward(DataSetItem inputs)
        {
            inputLayer.SetInputs(inputs.Inputs);
            Array.ForEach(forwardSteps, step => step.PropogateForward());
        }

        static int iterations = 0;
        public TrainingResults Train(DataSetItem inputs)
        {
            PropogateForward(inputs);    
            Marking.CorrectClass = Array.IndexOf(configuration.Characters, inputs.Character);
            Array.ForEach(reverseSteps, step => step.PropogateBackwards());

            int correctOutputIndex = Array.IndexOf(configuration.Characters, inputs.Character);
            
            return new TrainingResults(Marking.Output, correctOutputIndex);
            
        }

    }

    class TrainingResults
    {
        public TrainingResults(double[] results, int correctClass)
        {
            Error = results[correctClass];
            double minimumNonCorrectError = results.Where((result, index) => index != correctClass).Min();
            Correct = minimumNonCorrectError > Error;
        }
        public readonly double Error;
        public readonly bool Correct;
    }

}
