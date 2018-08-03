using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Controller : MonoBehaviour {
	public Camera[] cameras;
	public GameObject GD;
	public GameObject SD;
	private int CameraIndex;
	private bool e;
	private bool s;

	void Start () {
		e = false;
		s = false;
		CameraIndex = 0;
		cameras [CameraIndex].enabled = true;
		for (int i=1; i<cameras.Length; i++) 
		{
			cameras[i].enabled =  false;
		}

	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.C))
		{
			for (int i = 0; i < cameras.Length; i++) {
				cameras [i].enabled = false;
			}
			CameraIndex++;
			if (CameraIndex >= cameras.Length)
				CameraIndex = 0;
			cameras[CameraIndex].enabled = true;
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			if (e == true) {
				GD.SetActive (false);
				e = false;
			} else {
				GD.SetActive (true);
				e = true;
			
			}
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			if (s == true) {
				SD.SetActive (false);
				s = false;
			}
			else{
				SD.SetActive (true);
				s = true;
			}
		}
	}
}