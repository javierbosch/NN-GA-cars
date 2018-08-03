using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2 : MonoBehaviour {

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
		int PopulationSize = Game_Manager.PopulationSize ();
		target = Game_Manager.FastestCar ();
		if (Game_Manager.CarStayAlive < PopulationSize && Game_Manager.CarStayAlive > 0)
			Position = new Vector3 (target.GetPosition ().x, transform.position.y, target.GetPosition ().z - 4);
		else
			Position = new Vector3 (start.transform.position.x , transform.position.y, start.transform.position.z - 4);
		transform.position = Vector3.MoveTowards(transform.position, Position, 1f);
    }
}