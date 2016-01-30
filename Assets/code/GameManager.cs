using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private PanelPatient panelPatient;

	[SerializeField]
	private PanelDiagnose panelDiagnose;

	private Patient currentPatient;

	private const string DEBUG_HOST = "127.0.0.1:5000";//"10.184.5.117:5000";

	public void StartGame(){
		//Run any timer logic or shiiiit here
		string aspect = "game";
		string method = "start";
		string url = DEBUG_HOST + "/" + aspect + "/" + method;

		Debug.Log (url);

		new WWW(url);
		GetNewPatient();
	}

	public void GetNewPatient(){
		//Call web service ID
		string aspect = "patients";
		string method = "next";
		string url = DEBUG_HOST + "/" + aspect + "/" + method;

		StartCoroutine(CallPatientCreationEndpoint (url));
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
		string url = DEBUG_HOST + "/" + aspect + "/" + currentPatient.id + "/" + action;
		new WWW(url);
	}

	IEnumerator CallPatientCreationEndpoint(string url) {
		//		currentPatient = DebugGenerateTestPatient ();
		//		panelPatient.Populate (currentPatient);
		//		panelDiagnose.patient = currentPatient;

		Debug.Log (url);

		WWW www = new WWW(url);
		yield return www;

		//if (!string.IsNullOrEmpty (www.error)) {
		//	Debug.Log (www.error);
		//} else {


		string jsonString = www.text;

		Debug.Log (jsonString);

		currentPatient = JsonUtility.FromJson<Patient> (jsonString);
		panelPatient.Populate (currentPatient);
		panelDiagnose.patient = currentPatient;
		//}
	}

	void Reset(){
		panelPatient = GameObject.FindObjectOfType<PanelPatient> ();
	}

	int debugID;
	private Patient DebugGenerateTestPatient(){
		Patient patient = new Patient ();

		patient.age = Random.Range (0, 100);
		patient.name = "Martin 'The cunt' Chorley";
		patient.id = debugID.ToString ();
		patient.sex = "Male";
		patient.photo_fpath = "debugPhoto.jpg";
		patient.symptoms = new List<string> ();

		for (int i = 0; i < 10; ++i) {
			patient.symptoms.Add (i.ToString ());
		}

		debugID++;

		return patient;
	}
}