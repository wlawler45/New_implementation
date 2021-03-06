using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.IO.Ports;
//

public class impulsecode : MonoBehaviour {
	//public CommSerial commSerial;
	//public Ardunity.CommSerial sp=new Ardunity.CommSerial();
	public int count = 0;
	public int average = 0;
	public int mass;
	public int ID;
	public Color onColor = new Color (1, (float)0.92, (float) 0.016, 1);
	public Color offColor = new Color (0,0,0,1);
	public Collisioncollector cc;

	public void Start () {
		cc = transform.parent.GetComponent<Collisioncollector> ();
	}
	// Use this for initialization
	void OnCollisionEnter (Collision col) {
		//only send the velocity and mass not the derivative of anything
		float momentum =((col.relativeVelocity.sqrMagnitude)*mass)/(mass+col.rigidbody.mass); //this may not be the best way to get momentum
		int sender = Mathf.RoundToInt(momentum*200); //momentum values were around 5, so multiply by 200 to maintain more precision
		if (sender > 4095) sender=4095;
		cc.addvalue(ID,sender);

		//Change color of touchpoint on collision
		this.gameObject.GetComponent<Renderer>().material.color = onColor;
	}
	void OnCollisionStay (Collision col){
		//send the derivative by storing the previous value, ((current velocity-previous velocity)/period)*mass of objects possibly divided by collision area if the area makes a difference to the pressure calculation.
		//float impact = col.impulse.sqrMagnitude;
		//Debug.Log ("Impulse:");

		float velocity = col.relativeVelocity.sqrMagnitude;
		int velocitint = Mathf.RoundToInt (velocity*100); //*100 is added for increased precision
		if (velocitint > 4094) 
			velocitint=4094; //4095 will be the terminaion int for serial
		cc.addvalue(ID,4000);
		//cc.addvalue (ID, velocitint); correct entry
		//if (velocity > 255)
		//	velocity = 255;
		

		//byte biter = (byte)velocity;

		//sp.WriteLine (biter.ToString ());

		//Debug.Log("Sending current");


	}
	void OnCollisionExit (Collision col){
		//sp.WriteLine ("0");
		cc.addvalue(ID,0);
		//Debug.Log ("Exited");

		//Change color of touchpoint back since the collision is done
		this.gameObject.GetComponent<Renderer>().material.color = offColor;
	}

	// Update is called once per frame

}