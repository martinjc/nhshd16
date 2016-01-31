using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private PanelPatient panelPatient;

	private Patient currentPatient;

	public void StartGame(){
		//Run any timer logic or shiiiit here
		GetNewPatient();
	}

	public void GetNewPatient(){
		//Call web service ID

		currentPatient = DebugGenerateTestPatient ();

		panelPatient.Populate (currentPatient);
	}

	public void DeferPatient(){
		//Tell webservice to defer

		GetNewPatient ();
	}

	public void AcceptPatient(){
		//Tell web service to accept the patient

		GetNewPatient ();
	}

	public void DismissPatient(){
		//Tell web service to dismiss the patient

		GetNewPatient ();
	}

	private void CallEndpoint(string methodName, string patientID){

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
