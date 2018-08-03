using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    //Number Of Car Stay not Crashed in Single Generation Test
    public int CarStayAlive;

    //Check Points Position
    public Transform[] CheckPoints;

    //If The Geneartion Test Is End And All Car Is Crashed
	bool EndTheGenerationTest = true;

    //The Best Fitness Value The Cars Reached
    public double BestFitness = 0;
	public double BestFitnessThisGeneration = 0;
	public double BestFitnessLastGeneration = 0;

	public float MaxSpeed;

	public double[] fitnesses;
    //The Start Point For Instantiate Cars
    public Transform StartPosition;

    //References
    private GeneticManager Genetic_Manager;
    private InformationDisplay display;
    private Car car;

    public ArrayList mCars = new ArrayList();

    //To Store The Length Of All Track
    private double TrackLenght;

    private void Start()
    {

        Genetic_Manager = GetComponent<GeneticManager>();
        display = GameObject.FindGameObjectWithTag("Display").GetComponent<InformationDisplay>();

        //Calculate Track Length
        TrackLenght = CalculateTrackLength();
    }


    double CalculateTrackLength()
    {

        //Calculate The Distance Between Start Point And Each CheckPoint And Finish Line

        double tracklenght = Vector3.Distance(CheckPoints[0].position, StartPosition.position);

        for (int i = 0; i < CheckPoints.Length - 1; i++)
        {
            tracklenght += Vector3.Distance(CheckPoints[i].position, CheckPoints[i + 1].position);
        }

        return tracklenght;

    }



    //Start New Generation Test 
    public void StartNewTest(int NumOfPopulition)
    {
        //Assign Car Number 
        CarStayAlive = NumOfPopulition;

        //Make The Variable False To Begin Testing
        EndTheGenerationTest = false;
    }

	void Update () 
    {

        if(EndTheGenerationTest)
        {
            return;
        }

        //to Check If All Car Crashed to Stop The Test 
        if (CarStayAlive<=0  && EndTheGenerationTest == false)
        {
			BestFitnessLastGeneration = BestFitnessThisGeneration;
            EndTheGenerationTest = true;
            Genetic_Manager.RePopulition();
        }
		
	}

    public double CalculateFitness(Vector3 CarPosition,int IndexforLastChekPoint)
    {
       
        if(IndexforLastChekPoint==CheckPoints.Length-1)
        {
            return 1;
        }

        //Calclute The Fitness Value By The Progress To Finish Line


        double Fitness = Vector3.Distance(CheckPoints[IndexforLastChekPoint+1].position,CarPosition);

        for (int i=IndexforLastChekPoint+1;i<CheckPoints.Length-1;i++)
        {
            Fitness += Vector3.Distance(CheckPoints[i].position, CheckPoints[i + 1].position);
        }


        Fitness = (1 - (Fitness / TrackLenght));

		if (BestFitnessThisGeneration<Fitness)
		{
			BestFitnessThisGeneration = Fitness;
		} 

        if (BestFitness<Fitness)
        {
            BestFitness = Fitness;
        }    
		Array.Resize (ref fitnesses, Genetic_Manager.GenerationNumber);
		fitnesses [Genetic_Manager.GenerationNumber-1] = BestFitnessThisGeneration;
        return Fitness;

    }


    public void TestFinish()
    {
        display.ShowSuccessPanel();
        Time.timeScale = 0;
    }

    public void AddCar(Car _car)
    {
        mCars.Add(_car);
    }

    public void RemoveCar(Car _car)
    {
        mCars.Remove(_car);
    }

    public int NumCars()
    {
        return mCars.Count;
    }

    public Car GetCar(int _ndx)
    {
        if(_ndx >=0 && _ndx < NumCars())
            return (Car)mCars[_ndx];
        else
            return (Car)null;
    }

    public Car FastestCar()
    {
        double FastestValue = 0;
        Car FastestCar = null;

        for (int i = 0; i < NumCars(); i++)
        {   
            Car Car = GetCar(i);
            double CalclutedFitness = CalculateFitness(Car.GetPosition(),Car.LastIndexCheckPoint);
            if (Car != null)
            {
                if (FastestValue < CalclutedFitness)
                {
                    FastestValue = CalclutedFitness;
                    FastestCar = GetCar(i);
                }    
            }
            else
            {
                return null;
            }
        }
        return FastestCar;
    }
    
    public double[] FastestCarValues()
	{
		if (CarStayAlive < Genetic_Manager.PopulitionSize && CarStayAlive > 0)
			return FastestCar ().CalculateInputLayer ();
		else 
			return new double[] { 0.0 };
	}
	public int PopulationSize(){
		return Genetic_Manager.PopulitionSize;
	}

	public bool TestEnded(){
		return EndTheGenerationTest;
	}
	public double GetFitnessOfGeneration(int i){
		return fitnesses [i];
	}
}
