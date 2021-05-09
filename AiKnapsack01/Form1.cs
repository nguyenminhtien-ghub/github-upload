using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AiKnapsack01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static int capacity = new int();
        public static int numberOfItems = new int();
        public static int populationSize = new int();
        public static int numberOfGenerations = new int();
        public static int crossoverProbability = new int();
        public static int mutationProbability = new int();
        public static int currentGeneration = new int();
        public static int currentChromosome = new int();
        public static int bestFitnessIndex = new int();
        public static int bestFitnessValue = new int();
        public static int bestFitnessQuantity = new int();
        public static int bestFitnessWeight = new int();
        public static int bestSolutionIndex = new int();
        public static int bestSolutionValue = new int();
        public static int bestSolutionQuantity = new int();
        public static int bestSolutionWeight = new int();
        public static int parentIndex1 = new int();
        public static int parentIndex2 = new int();
        public static int[] weightsTemplate = new int[20];
        public static int[] valuesTemplate = new int[20];
        //public static int[] capacityTemplate = new int[4];
        public static int[] weights;
        public static int[] values;
        public static int[] fitness;
        public static int[] sumOfWeights;
        public static int[] quantityOfItems;
        public static int[] selectionProbability;
        public static int[,] probabilityDiapason;
        public static bool[,] population;
        public static bool[,] newPopulation;

        void calculateFitness()
        {
            for (int i = 0; i < populationSize; i++)
            {
                fitness[i] = 0;
                sumOfWeights[i] = 0;
                quantityOfItems[i] = 0;
            }
            for (int i = 0; i < populationSize; i++)
            {
                for (int j = 0; j < numberOfItems; j++)
                {
                    if (population[i, j])
                    {
                        fitness[i] += values[j];
                        sumOfWeights[i] += weights[j];
                        quantityOfItems[i]++;
                    }
                }
            }
            bestFitnessValue = fitness.Max();
            bestFitnessIndex = fitness.ToList().IndexOf(bestFitnessValue);
            bestFitnessWeight = sumOfWeights[bestSolutionIndex];
            bestSolutionQuantity = quantityOfItems[bestSolutionIndex];
            bestSolutionValue = 0;
            for (int i = 0; i < populationSize; i++)
            {
                if (fitness[i] > bestSolutionValue && sumOfWeights[i] <= capacity)
                {
                    bestSolutionValue = fitness[i];
                    bestSolutionIndex = i;
                    bestSolutionWeight = sumOfWeights[i];
                    bestSolutionQuantity = quantityOfItems[i];
                }
            }
        }
        void probabilityCalculation()
        {
            for (int i = 0; i < populationSize; i++)
            {
                selectionProbability[i] = 0;
                for (int j = 0; j < 2; j++)
                {
                    probabilityDiapason[i, j] = 0;
                }
            }
            int sumFitness = 0;
            for (int i = 0; i < populationSize; i++)
            {
                if (sumOfWeights[i] <= capacity)
                {
                    sumFitness += fitness[i];
                }
            }
            for (int i = 0; i < populationSize; i++)
            {
                if (sumOfWeights[i] <= capacity)
                {
                    selectionProbability[i] = 100 * fitness[i] / sumFitness;
                }
                else
                {
                    selectionProbability[i] = 0;
                }
            }
            probabilityDiapason[0, 0] = 0;
            probabilityDiapason[0, 1] = selectionProbability[0];

            for (int i = 1; i < populationSize; i++)
            {
                probabilityDiapason[i, 0] = probabilityDiapason[i - 1, 1] + 1;
                probabilityDiapason[i, 1] = probabilityDiapason[i, 0] + selectionProbability[i];
            }
        }

        void parentsSeletion()
        {
            parentIndex1 = 0;
            parentIndex2 = 0;
            Random rng = new Random();
            int roulette = rng.Next(0, 100);
            for (int i = 1; i < populationSize; i++)
            {
                if (roulette >= probabilityDiapason[i, 0] && roulette < probabilityDiapason[i, 1])
                {
                    parentIndex1 = i;
                    break;
                }
            }
            roulette = 0;
            bool rouletteValid = false;
            while (!rouletteValid)
            {
                roulette = rng.Next(0, 100);
                if (roulette < probabilityDiapason[parentIndex1, 0] || roulette >= probabilityDiapason[parentIndex1, 1])
                {
                    rouletteValid = true;
                }
            }
            for (int i = 1; i < populationSize; i++)
            {
                if (roulette >= probabilityDiapason[i, 0] && roulette <probabilityDiapason[i, 1])
                {
                    parentIndex2 = i;
                    break;
                }
            }
        }

        void crossover()
        {
            Random rng = new Random();
            int division = rng.Next(1, numberOfItems - 2);
            bool firstParent = new bool();
            if (rng.NextDouble() > 0.5)
            {
                firstParent = true;
            }
            else
            {
                firstParent = false;
            }
            for (int i = 0; i < division; i++)
            {
                if (firstParent)
                {
                    newPopulation[currentChromosome, i] = population[parentIndex1, i];
                }
                else
                {
                    newPopulation[currentChromosome, i] = population[parentIndex2, i]; 
                }
            }
            for (int i = division; i < numberOfItems; i++)
            {
                if (!firstParent)
                {
                    newPopulation[currentChromosome, i] = population[parentIndex1, i];
                }
                else
                {
                    newPopulation[currentChromosome, i] = population[parentIndex2, i]; 
                }
            }
            currentChromosome++;
        }

        void mutation()
        {
            Random rng = new Random();
            int mutate = new int();
            for (int i = 0; i < populationSize; i++)
            {
                for (int j = 0; j < numberOfItems; j++)
                {
                    mutate = rng.Next(0, 100);
                    if (mutate <= mutationProbability)
                    {
                        newPopulation[i, j] = !newPopulation[i, j];
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView3.Items.Clear();
            numberOfGenerations = Convert.ToInt32(numericUpDown6.Value);
            crossoverProbability = Convert.ToInt32(textBox1.Text);
            mutationProbability = Convert.ToInt32(textBox2.Text);
            currentGeneration = 0;

            calculateFitness();

            while (currentChromosome != numberOfGenerations + 1)
            {
                for (int i = 0; i < populationSize; i++)
                {
                    for (int j = 0; j < numberOfItems; j++)
                    {
                        newPopulation[i, j] = false;
                    }
                }
                currentChromosome = 0;

                //Elitism
                for (int i = 0; i < numberOfItems; i++)
                {
                    newPopulation[currentChromosome, i] = population[bestFitnessIndex, i];
                }
                currentChromosome++;

                probabilityCalculation();

                Random rng = new Random();
                while (currentChromosome < populationSize)
                {
                    parentsSeletion();
                    int breed = rng.Next(0, 100);
                    if (breed <= crossoverProbability)
                    {
                        crossover();
                    }
                    else
                    {
                        for (int i = 0; i < numberOfItems; i++)
                        {
                            newPopulation[currentChromosome, i] = population[parentIndex1, i];
                        }
                        currentChromosome++;
                        if (currentChromosome < populationSize)
                        {
                            for (int i = 0; i < numberOfItems; i++)
                            {
                                newPopulation[currentChromosome, i] = population[parentIndex2, i];
                            }
                            currentChromosome++;
                        }
                    }
                }

                mutation();

                for (int i = 0; i < populationSize; i++)
                {
                    for (int j = 0; j < numberOfItems; j++)
                    {
                        population[i, j] = newPopulation[i, j];
                    }
                }

                calculateFitness();
                currentChromosome++;
            }

            string gene ="";
            for (int i = 0; i < numberOfItems; i++)
            {
                if (population[bestSolutionIndex, i])
                {
                    gene += "1";
                }
                else
                {
                    gene += "0";
                }
            }

            string[] rowBest =
            {
                (bestSolutionIndex + 1).ToString(),  gene, bestSolutionValue.ToString(), bestSolutionWeight.ToString(), fitness[bestSolutionIndex].ToString()
            };
            var itemBest = new ListViewItem(rowBest);
            listView3.Items.Add(itemBest);

            int tWeights;
            int tValues;
            
            for (int i = 0; i < populationSize; i++)
            {
                tWeights = 0;
                tValues = 0;
                gene = "";

                for (int j = 0; j < numberOfItems; j++)
                {
                    if (population[i, j])
                    {
                        gene += "1";
                        tWeights += weights[j];
                        tValues += values[j];
                    }
                    else
                    {
                        gene += "0";
                    }
                }

                string[] row =
                  {
                        (i + 1).ToString(), gene, tValues.ToString(), tWeights.ToString(), fitness[i].ToString()
                    };
                var item = new ListViewItem(row);
                listView3.Items.Add(item);
            }

            //this.listView3.Sorting = SortOrder;
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            numberOfItems = Convert.ToInt32(numericUpDown1.Value);
            int mValue = Convert.ToInt32(numericUpDown3.Value);
            int mWeight = Convert.ToInt32(numericUpDown2.Value);

            weights = new int[numberOfItems];
            values = new int[numberOfItems];
            
            for (int i = 0; i < numberOfItems; i++)
            {
                weights[i] = weightsTemplate[i];
                values[i] = valuesTemplate[i];
            }

            Random rng = new Random();
            for (int i = 0; i < numberOfItems; i++)
            {
                int rWeight = rng.Next(1, mWeight);
                weights[i] = rWeight;
                int rValues = rng.Next(1, mValue);
                values[i] = rValues;

                string[] row =
                {
                    (i + 1).ToString(), values[i].ToString(), weights[i].ToString()
                };
                var item = new ListViewItem(row);
                listView1.Items.Add(item);
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            capacity = Convert.ToInt32(numericUpDown5.Value);
            populationSize = Convert.ToInt32(numericUpDown4.Value);

            population = new bool[populationSize, numberOfItems];
            fitness = new int[populationSize];
            sumOfWeights = new int[populationSize];
            quantityOfItems = new int[populationSize];
            selectionProbability = new int[populationSize];
            newPopulation = new bool[populationSize, numberOfItems];
            probabilityDiapason = new int[populationSize, 2];

            int tWeights;
            int tValues;
            Random rng = new Random();
            for (int i = 0; i < populationSize; i++)
            {
                for (int j = 0; j < numberOfItems; j++)
                {
                    if (rng.NextDouble() > 0.5)
                    {
                        population[i, j] = true;
                    }
                    else
                    {
                        population[i, j] = false;
                    }
                }
                tWeights = 0;
                tValues = 0;
                for (int j = 0; j < numberOfItems; j++)
                {
                    if (population[i, j])
                    {
                        tWeights += weights[j];
                        tValues += values[j];
                    }                    
                }

                string gene = "";
                for (int j = 0; j < numberOfItems; j++)
                {
                    if (population[i, j])
                    {
                        gene += "1";
                    }
                    else
                    {
                        gene += "0";
                    }
                }

                if (tWeights > capacity)
                {
                    i--;
                }

                string[] row =
                {
                    (i + 1).ToString(), gene, tValues.ToString(), tWeights.ToString() 
                };
                var item = new ListViewItem(row);
                listView2.Items.Add(item);
            }

            
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
