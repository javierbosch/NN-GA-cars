using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticManager : MonoBehaviour
{
    public GameObject CarPrefab;

    public Agent[] Populition;

    public int PopulitionSize = 100;

    //The number of the best agents, will be kept to the next generation 
    public int Elitism = 5; 

    //The number of the best agents, will be used for crossover
    public int NumberOfAgentToCrossOver = 70;

    //NN shape 
    public int[] NeuralNetworkShape;

    //% of agents and cromosomes mutated
    public double MutationRate = 0.05;

    //% of the first parent each agent
    public double CrossoverRate = 0.3;
    
    public  int GenerationNumber = 1;

    private int WeightNum;
    private int BiasesNum;

    //References
    private GameManager Game_Manager;

    private void Start()
    {
        Game_Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        //Calculate the number of weights in the NN
        for (int i = 1; i < NeuralNetworkShape.Length; i++)
        {
            WeightNum += NeuralNetworkShape[i - 1] * NeuralNetworkShape[i];
        }

        //Calculate the number of biases in the NN
        BiasesNum = NeuralNetworkShape.Length - 1;
   
        //Initiate a populition 
        Populition = new Agent[PopulitionSize];

        for (int i = 0; i < PopulitionSize; i++)
        {
            //Initiate DNA for agents 
            Populition[i] = new Agent();

            Populition[i].Fitness = 0;
            //Initiate new agent weights
            Populition[i].Weights = new double[WeightNum];
            //Initiate new agent biases
            Populition[i].Biases = new double[BiasesNum];

            //Generate random weights value
            for (int j = 0; j < WeightNum; j++)
            {
                Populition[i].Weights[j] = Random.Range(-1.0f, 1.0f);
            }

            //Generate random biases value
            for (int j = 0; j < BiasesNum; j++)
            {
                Populition[i].Biases[j] = Random.Range(-1.0f, 1.0f);
            }
        }
        StartTheGenetationTest();
    }

    public void StartTheGenetationTest()
    {
        for (int i = 0; i < PopulitionSize; i++)
        {
            //Initiate new car prefab
            Populition[i].Prefab = Instantiate(CarPrefab, new Vector3(-4, 0.035f, 4.116f), new Quaternion(0, 0, 0, 0));
            //Initiate new car Neural Network
            Populition[i].Prefab.GetComponent<Car>().InstNeuralNetwork(NeuralNetworkShape, Populition[i].Weights,Populition[i].Biases);
            //Assign DNA ID by index of populition array
            Populition[i].Prefab.GetComponent<Car>().Id = i;
        }
        //Start the test
        Game_Manager.StartNewTest(PopulitionSize);
    }
		
    public void RePopulition()
    {
        Debug.Log("Generation: " + GenerationNumber + "|" + "Best fitness this generation: " + Game_Manager.BestFitnessThisGeneration);
		Game_Manager.BestFitnessThisGeneration = 0;

        //Sort populition by fitness
        for (int i = 0; i < PopulitionSize; i++)
        {
            for (int j = i; j < PopulitionSize; j++)
            {
                if (Populition[i].Fitness < Populition[j].Fitness)
                {
                    Agent Temp = Populition[i];
                    Populition[i] = Populition[j];
                    Populition[j] = Temp;
                }
            }
        }

        int IndexforNewPopulition = 0;

        Agent[] NewPopulition = new Agent[PopulitionSize];

 
        for (int i = 0; i < Elitism; i++)
        {
            NewPopulition[IndexforNewPopulition] = Populition[i];
            NewPopulition[IndexforNewPopulition].Fitness = 0;
            IndexforNewPopulition++;
        }
			    
        //Crossover :
        for (int i = 0; i < NumberOfAgentToCrossOver; i+=2)
        {
            Agent Child1 = new Agent();
            Agent Child2 = new Agent();

            Child1.Weights = new double[WeightNum];
            Child2.Weights = new double[WeightNum];

            Child1.Biases = new double[BiasesNum];
            Child2.Biases = new double[BiasesNum];

            Child1.Fitness = 0;
            Child2.Fitness = 0;

            for (int j=0;j<WeightNum;j++)
            {
                if(Random.Range(0.0f,1.0f)<CrossoverRate)
                {
                    Child1.Weights[j] = Populition[i +1].Weights[j];
                    Child2.Weights[j] = Populition[i].Weights[j];
                }
                else
                {
                    Child1.Weights[j] = Populition[i ].Weights[j];
                    Child2.Weights[j] = Populition[i +1 ].Weights[j];
                }
            }

            for (int j = 0; j < BiasesNum; j++)
            {
                if (Random.Range(0.0f, 1.0f) < CrossoverRate)
                {
                    Child1.Biases[j] = Populition[i].Biases[j];
                    Child2.Biases[j] = Populition[i + 1].Biases[j];
                }
                else
                {
                    Child1.Biases[j] = Populition[i + 1].Biases[j];
                    Child2.Biases[j] = Populition[i].Biases[j];
                }
            }
            NewPopulition[IndexforNewPopulition] = Child1;
            IndexforNewPopulition++;
            NewPopulition[IndexforNewPopulition] = Child2;
            IndexforNewPopulition++;
        }

        //Make mutations     
		for (int i=0;i<IndexforNewPopulition;i++)
        {
            if (Random.Range(0.0f, 1.0f) < MutationRate)
            {
                for(int j=0;j<WeightNum;j++)
                {
                    if(Random.Range(0.0f, 1.0f) < MutationRate)
                    {
                        NewPopulition[i].Weights[j] = Random.Range(-1.0f, 1.0f);
                    }
                }
                for (int j = 0; j < BiasesNum; j++)
                {
                    if (Random.Range(0.0f, 1.0f) < MutationRate )
                    {
                        NewPopulition[i].Biases[j] = Random.Range(-1.0f, 1.0f);
                    }
                }
            }
        }

        //Migrations
        while (IndexforNewPopulition<PopulitionSize)
        {
            NewPopulition[IndexforNewPopulition] = new Agent();

            NewPopulition[IndexforNewPopulition].Fitness = 0;
            //Initiate new agent weights
            NewPopulition[IndexforNewPopulition].Weights = new double[WeightNum];
            //Initiate new agent Biases
            NewPopulition[IndexforNewPopulition].Biases = new double[BiasesNum];
            //Generate random weights value
            for (int j = 0; j < WeightNum; j++)
            {
                NewPopulition[IndexforNewPopulition].Weights[j] = Random.Range(-1.0f, 1.0f);
            }
            //Generate random biases value
            for (int j = 0; j < BiasesNum; j++)
            {
                NewPopulition[IndexforNewPopulition].Biases[j] = Random.Range(-1.0f, 1.0f);
            }
            IndexforNewPopulition++;
        }

        //Assign new populition 
        Populition = NewPopulition;

        //Start new generation test 
        StartTheGenetationTest();

        //Increase generation number
        GenerationNumber++;
    }
}