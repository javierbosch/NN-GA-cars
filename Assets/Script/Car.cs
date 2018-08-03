using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Rigidbody rb;

    //Reference For The Current Agent Index in Populition Array
    public int Id;

    public NeuralNetwork NN;
    //Reference For Sensors Position In The Car
    public Transform[] SensorsPosition;
 
    private bool CarCrashed= false;

    //To know What The Last Check Point the Car Reach it
    public int LastIndexCheckPoint = -1;

    //Reference
    private GameManager Game_Manager;
    private GeneticManager Genetic_Manager;

    Vector3 LastPosition;
	private float Speed;

    private void Start()
    {
        //Assign The References
        Game_Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Genetic_Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GeneticManager>();
        rb = GetComponent<Rigidbody>();
        Game_Manager.AddCar(this);
    }

    public void InstNeuralNetwork(int[] Layers, double[] Weights,double[] Biases)
    {
        NN = GetComponent<NeuralNetwork>();
        NN.inst(Layers, Weights, Biases);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
	public float GetYRotation()
	{
		return transform.eulerAngles.y;
	}

	public void Update ()
    { 
        //Check If Car Crashed To Stop All Movements
        if (CarCrashed)
        {
            return;
        }

        //to Check if The Neural Network Instantiate And The Test Begin
        if (NN==null)
        {
            return;
        }

        //Calculate Input Layer From Car Sensors 
        double[] InputLayer = CalculateInputLayer();

        //Run The Neural Network And Get Output Layer
        double[] Output = NN.Run(InputLayer);

        //The First Value In output layer is the Engine Power
        double EngineValue = NN.Sigmoid(Output[1]);

        //The Second Value In output layer is Turn Value of the car
        double TurnValue = Output[0];

        //Apply Moving
		LastPosition = transform.position;
		transform.Translate(new Vector3((float)EngineValue * Game_Manager.MaxSpeed * Time.deltaTime, 0, 0));

		Speed = (transform.position - LastPosition).magnitude / Time.deltaTime;

        //transform.position += transform.forward * Time.deltaTime * ToSingle(EngineValue);
        //Apply Rotation
		transform.Rotate(new Vector3(0, (float)TurnValue * 45f* Game_Manager.MaxSpeed* Time.deltaTime, 0));
    }
		
    public static float ToSingle(double value)
    {
        return (float)value;
    }

    public double[] CalculateInputLayer()
    {
		int NumberOfSensor = SensorsPosition.Length;
        //Instantiate Distance array
        double[] Values = new double[NumberOfSensor];
        //Calculate Distances from Sensors
		for (int i = 0; i < NumberOfSensor; i++)
        {
            // Ray Cast From Sensor To Calclute Distance 
            Ray ray = new Ray(SensorsPosition[i].position, SensorsPosition[i].forward);

            RaycastHit hit;

			Physics.Raycast(ray, out hit,10f);

            if(hit.collider!=null)
            {
                //Distances Between Sensor Position and Wall
                Values[i] = Vector3.Distance(SensorsPosition[i].position, hit.point);
            }
            else
            {
                Values[i] = 0;
            }   
        }
        return Values;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Wall"  && CarCrashed==false)
        {
            Crashed();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag=="CheckPoint")
        {   
            LastIndexCheckPoint++;
        }

		if (other.tag == "FinishLine")
		{
			Game_Manager.TestFinish();
		}
    }

	public float GetSpeed(){
		return Speed;
	}

    void Crashed()
    {
        Game_Manager.RemoveCar(this);
        CarCrashed = true;
        //Decrease Number Of Car They Stay Alive
        Game_Manager.CarStayAlive--;
        //Calculate Fitness Value
        double fitness = Game_Manager.CalculateFitness(transform.position, LastIndexCheckPoint);
        //Assign Fitness value
        Genetic_Manager.Populition[Id].Fitness = fitness;

        Destroy(gameObject, 1f);
    }
}
