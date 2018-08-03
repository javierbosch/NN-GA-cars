using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera1 : MonoBehaviour {

	private GameManager Game_Manager;
	private Car target;
	private Vector3 Position;
	private Transform start;

	void Start ()
	{
		Game_Manager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ();
		start = Game_Manager.StartPosition;
	}
		
	void Update ()
	{
		int PopulationSize = Game_Manager.PopulationSize();
			
		if (Game_Manager.CarStayAlive < PopulationSize && Game_Manager.CarStayAlive > 0) {
			target = Game_Manager.FastestCar();
			Position = target.transform.position - target.transform.right * 2 + target.transform.up * 2;
			transform.position = Vector3.MoveTowards (transform.position, Position, 1f);
			transform.LookAt (Game_Manager.FastestCar().transform);
		}
		else{
			Position = start.transform.position - start.transform.right * 2 + start.transform.up * 2;
			transform.position = Vector3.MoveTowards(transform.position, Position, 1f);
		}
	}
}
