﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private PanelPatient panelPatient;

	private Patient currentPatient;

	private string DEBUG_HOST = 10.184.5.117:5000;

	public void StartGame(){
		//Run any timer logic or shiiiit here
		string aspect = "game";
		string method = "start";
		string url = DEBUG_HOST + "/" + aspect + "/" + method;
		CallEndpoint (url);
		GetNewPatient();
	}

	public void GetNewPatient(){
		//Call web service ID
		string aspect = "patients";
		string method = "next";
		string url = DEBUG_HOST + "/" + aspect + "/" + method;
		string jsonString = CallEndpoint (url);

		//currentPatient = JsonUtility.FromJson<Patient> (jsonString);
		currentPatient = DebugGenerateTestPatient ();

		panelPatient.Populate (currentPatient);
	}

	public void DeferPatient(){
		//Tell webservice to defer
		ProcessPatient("defer");
		GetNewPatient ();
	}

	public void AcceptPatient(){
		//Tell web service to accept the patient
		ProcessPatient("admit");
		GetNewPatient ();
	}

	public void DismissPatient(){
		//Tell web service to dismiss the patient
		ProcessPatient("dismiss");
		GetNewPatient ();
	}

	private void ProcessPatient(string action) {
		string aspect = "patients";
		string url = DEBUG_HOST + "/" + aspect + "/" + currentPatient.ID + "/" + action;
		CallEndpoint (url);
	}

	private void CallEndpoint(string url) {
		IEnumerator Start() {
			WWW www = new WWW(url);
			yield return www;
			string jsonString = www.text;
			return jsonString;
	}

	void Reset(){
		panelPatient = GameObject.FindObjectOfType<PanelPatient> ();
	}

	int debugID;
	private Patient DebugGenerateTestPatient(){
		Patient patient = new Patient ();

		patient.Age = Random.Range (0, 100);
		patient.Name = "Martin 'The cunt' Chorley";
		patient.ID = debugID.ToString ();
		patient.Gender = "Male";
		patient.PhotoFilename = "debugPhoto.jpg";
		patient.Symptoms = new List<string> ();

		for (int i = 0; i < 10; ++i) {
			patient.Symptoms.Add (i.ToString ());
		}

		debugID++;

		return patient;
	}
}
