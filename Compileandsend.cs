﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.IO.Ports;
using System.Threading;

public class Compileandsend : MonoBehaviour { //This class collects all collision data and stores in array of values.
	/// T
	/// </summary>


	public int[] outvals;
	SerialPort stream;
	// Use this for initialization
	void Start () {
		string portName = "COM8"; //Change the port name to match the one you specifically are using (a function that handles this could be convenient...)
		int rate = 115200;
		outvals = new int[21];
		stream = new SerialPort(portName, rate); 
		stream.ReadTimeout = 50;
		try{
			stream.Open();
		}
		catch(System.IO.IOException){
			//silently catch the error
		}

		StartCoroutine (aftercollection ());
	}

	// Update is called once per frame
	//void final(){
	//	yield return new WaitForFixedUpdate();
	//	Debug.Log ("Waitforfixedupdate finished and continued");


	//}
	IEnumerator aftercollection(){
		//Idea for future: most of the time collisions are not happening, instead of always sending an array only send when new data
		while(true){
			yield return new WaitForFixedUpdate(); //We assume this line makes outvals stable until this line is reached again
			//Convert int array to byte array
			int Asize = outvals.Length*2+2; //extra 2-byte space is for termination byte
			if(Asize>2){
				//the garabage collector deals with with deleting bytearray. It becomes out of scope is when the loop restarts
				var bytearray= new byte[Asize];
				//Attach start int (40,95) to the array
				bytearray[0] = 19;//(byte) (4095 >> 8);
				bytearray[1] = 136;//unchecked((byte) (4095)); //unchecked allows for extra bits to be truncated instead of overflowed

				for (int i = 2; i<Asize; i+=2){
					//convert each int in outvals to 2 bytes in bytearray (The integer will always be <4096 so we only need the latter 2 bytes)
					bytearray [i] = (byte) (outvals[(i-2)/2] >> 8); //
					bytearray [i + 1] = (byte) (outvals[(i-2)/2]);
					print ((int)((bytearray[i]<<8)+bytearray[i+1]));
				}

				//Send byte array via serial
				try{
					stream.Write(bytearray,0,bytearray.Length-1);

				}
				catch(System.Exception){
					//print ("That's exceptional!");
				}
				for(int i = 0; i<outvals.Length; i++){
					outvals[i]=0;
				}
				//End sending byte array

				//save to byte array textfile (may have to change to deal with encodings)
				//Use this website to compare file with Microcontroller's save afterwards: https://www.diffnow.com/
				//WriteArrayToFile(bytearray,0,bytearray.Length-1);

				//print byte array to console. Printing is slow so give a small random chance to print
				/*
				if(Random.Range(0,100) == 1){
					for(int i =0; i<bytearray.Length;i++){
						print (bytearray [i]);
					}
				}
				*/
			}
		}
	}
	//takes a char array and writes the bytes to a text file
	static void WriteArrayToFile(byte[] buffer, int index, int count){
		string path = "Assets/Resources/SerialTest.txt"; //the path could be a parameter
		StreamWriter writer = new StreamWriter(path, true);
		writer.BaseStream.Write(buffer,index,count);
		writer.Close();
	}
	public void editOut(int index, int val){
		outvals [index] = val; //hashes the value to its respective index
	}
}
