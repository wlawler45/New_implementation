using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisioncollector : MonoBehaviour {
	public GameObject ArduinoController;//=GetComponent<Compileandsend>();
	//    Use this for initialization
	public Compileandsend outputter;
	private int IDoffset;  //IDoffset allows this code to determine which finger it is on and adapt the ID number of the actuator
	void Start(){ 
		ArduinoController = GameObject.FindWithTag ("SerialSender");
		//LATER ADD EXCEPTION CATCHER FOR IF THIS RETURNS NULL TO HELP PEOPLE WITH INITIALIZATION
		outputter=ArduinoController.GetComponent<Compileandsend>();
		IDoffset = 0;
		if (gameObject.name.Contains ("Index")) {  
			IDoffset=5;
		}
		if (gameObject.name.Contains ("Middle")) {
			IDoffset=10;
		}
		if (gameObject.name.Contains ("Ring")) {
			IDoffset=15;
		}
		if (gameObject.name.Contains ("Pinky")) {
			IDoffset=18;
		}
	}


	public void addvalue(int ID, int value){ //ID is number of actuator in array (between 1 and 5 for thumb, pointer and middle fingers and three for ring and pink since it is per finger) and value is collision information recorded
		
		//Debug.Log ("addvalue");
		int x=ID+IDoffset; //Full actuator ID determination


		outputter.editOut (x, value);
		//Debug.Log (value);
	}
}
