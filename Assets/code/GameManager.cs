﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private PanelPatient panelPatient;

	[SerializeField]
	private PanelDiagnose panelDiagnose;

	[SerializeField]
	private GameState currentGameState;

	[SerializeField]
	HandbookManager handbookManager;

	[SerializeField]
	private PanelEndGame panelEndGame;

	private Patient currentPatient;

	[SerializeField]
	private Chart patientChart;

	[SerializeField]
	private Chart bedChart;

	public bool isUpdating;

	private const string DEBUG_HOST = "127.0.0.1:5000";//"10.184.5.117:5000";

	public void StartGame(){
		isUpdating = false;

		//Run any timer logic or shiiiit here
		string aspect = "game";
		string method = "start";
		string url = DEBUG_HOST + "/" + aspect + "/" + method;
		new WWW(url);
		GetNewPatient();

		//DEBUG
		//		Handbook handbook = new Handbook();
		//		handbook.symptoms = new List<Symptom> ();
		//		for (int i = 0; i < Random.Range(3, 9); ++i) {
		//			Symptom symptom = new Symptom ();
		//			symptom.name = "Symptom " + i.ToString();
		//
		//			symptom.causes = new List<string> ();
		//			for (int j = 0; j < Random.Range(2, 7); ++j) {
		//				symptom.causes.Add ("Cause " + j.ToString ());
		//			}
		//
		//			handbook.symptoms.Add (symptom);
		//		}

		Handbook handbook = new Handbook();

		PopulateHandbook (handbook);

		//Start update cycle
		InvokeRepeating ("UpdateStateCycle", 0f, 1f);
	}

	public void UpdateStateCycle(){
		if (!isUpdating) {
			stillPlaying ();
		}
	}

	public void EndGame() {
		panelEndGame.deathNum.text = currentGameState.dead.ToString();
		panelEndGame.score.text = currentGameState.score.ToString();
		PanelManager.instance.ShowEndgame ();
		Debug.Log ("We're Done. Fuck Martin.");
	}

	public void PopulateHandbook(Handbook handbook){
		string url = DEBUG_HOST + "/handbook/complete";
		StartCoroutine (CallHandbookCreationEndpoint (url, handbook));
		//		handbookManager.Populate (handbook);
	}


	public void GetNewPatient(){
		if (stillPlaying ()) {
			string aspect = "patients";
			string method = "next";
			string url = DEBUG_HOST + "/" + aspect + "/" + method;
			StartCoroutine (CallPatientCreationEndpoint (url));
		}
	}

	public bool stillPlaying() {
		StartCoroutine(CallStateEndpoint ());
		if (currentGameState.state == "ended") {
			return false;
		} else {
			return true;
		}
	}

	public void DeferPatient(){
		//Tell webservice to defer
		ProcessPatient("defer");
		GetNewPatient ();
	}

	public void AcceptPatient(){
		//Tell web service to accept the patient

		if (currentGameState.total_beds - currentGameState.used_beds > 0) {
			ProcessPatient ("admit");
			GetNewPatient ();
		}
	}

	public void DismissPatient(){
		//Tell web service to dismiss the patient
		ProcessPatient("dismiss");
		GetNewPatient ();
	}

	private void ProcessPatient(string action) {
		string aspect = "patients";
		string url = DEBUG_HOST + "/" + aspect + "/" + currentPatient.id + "/" + action;
		WWWForm wwwForm = new WWWForm ();
		wwwForm.AddField ("test", "true");
		new WWW(url, wwwForm);
	}

	IEnumerator CallStateEndpoint() {
		isUpdating = true;
		string url = DEBUG_HOST + "/game/state";
		WWW www = new WWW (url);
		yield return www;
		string jsonString = www.text;
		currentGameState = JsonUtility.FromJson<GameState> (jsonString);
		if(currentGameState.state == "ended"){
			EndGame ();
		}



		//Update charts
		patientChart.Set(currentGameState.in_queue, currentGameState.in_queue);
		bedChart.Set(currentGameState.total_beds, currentGameState.used_beds);

		isUpdating = false;
	}

	IEnumerator CallHandbookCreationEndpoint(string url, Handbook hb) {
		WWW www = new WWW(DEBUG_HOST + "/handbook/complete");
		yield return www;
		string responseJson = www.text;
		Debug.Log (responseJson);
		//issues with JsonUtility prevents deserializing top level arrays
		hb = JsonUtility.FromJson<Handbook>(responseJson);
		Debug.Log (hb.symptoms.Count);
		handbookManager.Populate(hb);
	}

	IEnumerator CallPatientCreationEndpoint(string url) {
		WWW www = new WWW(url);
		yield return www;
		string jsonString = www.text;
		Debug.Log (jsonString);
		currentPatient = JsonUtility.FromJson<Patient> (jsonString);

		panelPatient.Populate (currentPatient);
		panelDiagnose.patient = currentPatient;
	}

	void Reset(){
		panelPatient = GameObject.FindObjectOfType<PanelPatient> ();
	}
}
