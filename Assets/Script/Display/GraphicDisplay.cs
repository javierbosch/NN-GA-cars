using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GraphicDisplay : MonoBehaviour
{
	public Text Test;

	private NeuralNetwork NN;
	private GeneticManager Genetic_Manager;
	private GameManager Game_Manager;
	private Car car;
	private float MaxHeight = 500;
	public GameObject[] Bars;

	public int MaxGenerations;
	public int OffSet;

	void Start ()
	{
		Genetic_Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GeneticManager>();
		Game_Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		NN = GetComponent<NeuralNetwork>();
		MaxGenerations = Bars.Length;
	}
	void Update ()
	{
		if (Game_Manager.FastestCarValues() [0] == 0) {
			return;
		}
		else{
			if (Genetic_Manager.GenerationNumber > MaxGenerations + OffSet) {
				OffSet += 1;
			}
			if (Genetic_Manager.GenerationNumber < MaxGenerations) {
				for (int i = 0; i < Genetic_Manager.GenerationNumber; i++) {
					RectTransform rt = Bars [i].GetComponent<RectTransform> ();
					float x = ToSingle (Game_Manager.GetFitnessOfGeneration (i + OffSet) * MaxHeight);
					rt.sizeDelta = new Vector2 (25, x);
				}
			} else {
				for (int i = 0; i < MaxGenerations; i++) {
					RectTransform rt = Bars [i].GetComponent<RectTransform> ();
					float x = ToSingle (Game_Manager.GetFitnessOfGeneration (i + OffSet) * MaxHeight);
					rt.sizeDelta = new Vector2 (25, x);
				}
			}
		}
	}
	private static float ToSingle(double value)
	{
		return (float)value;
	}
}
