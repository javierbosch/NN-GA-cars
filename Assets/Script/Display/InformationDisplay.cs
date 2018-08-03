using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InformationDisplay : MonoBehaviour
{

    public GameObject SuccessPanel;

    public Text GenerationNumberText;
    public Text BestFitnessText;
	public Text BestFitneesTGText;
    public Text CarStayAliveText;
    public Text Values;
	public Text Speed;

	private GeneticManager Genetic_Manager;
    private GameManager Game_Manager;
    private Car car;

    

	void Start ()
    {
        Genetic_Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GeneticManager>();
        Game_Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


    string GetFastestCarValuesAsString()
    {
        double [] values = Game_Manager.FastestCarValues();
        string Values = "";
		if (values [0] == 0)
			Values = "Sensor 1-> NaN \nSensor 2-> NaN \nSensor 3-> NaN\nSensor 4-> NaN \nSensor 5-> NaN \nSensor 6-> NaN\nSensor 7-> NaN";
		else {
			for (int i = 0; i < values.Length; ++i) {
				int SensorNumber = i + 1;
				if (i == 0)
					Values = "Sensor " + SensorNumber +"-> "+ values [i].ToString ();
				else
						Values = Values +  "\nSensor " + SensorNumber  +"-> "+ values [i].ToString ();
			}
		}
        return Values;
    }

	string GetFastestCarSpeedAsString(){
		string SpeedText;
		if (Game_Manager.FastestCarValues ()[0] == 0) 
			SpeedText = "Speed-> NaN";
		else
			SpeedText = "Speed-> " + Game_Manager.FastestCar ().GetSpeed ().ToString ();
		return SpeedText;

	}

	void Update ()
    {
        GenerationNumberText.text = "Generation: " + Genetic_Manager.GenerationNumber.ToString();
        BestFitnessText.text = "Best fitness: " + Game_Manager.BestFitness.ToString();
        CarStayAliveText.text ="Cars alive: " + Game_Manager.CarStayAlive.ToString();
        Values.text = GetFastestCarValuesAsString();
		Speed.text = GetFastestCarSpeedAsString();
		BestFitneesTGText.text = "Best fitness this generation: " + Game_Manager.BestFitnessThisGeneration.ToString();
    }

    public void ShowSuccessPanel()
    {
        SuccessPanel.SetActive(true);

        SuccessPanel.GetComponent<SuccessPanel>().SetInformation(Genetic_Manager.GenerationNumber, Genetic_Manager.MutationRate, Genetic_Manager.CrossoverRate);
    }



   




}
