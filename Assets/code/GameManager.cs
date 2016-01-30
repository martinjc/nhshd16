using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private PanelPatient panelPatient;

	[SerializeField]
	private PanelDiagnose panelDiagnose;

	[SerializeField]
	private GameState currentGameState;

	private Patient currentPatient;

	private const string DEBUG_HOST = "127.0.0.1:5000";

	public void StartGame(){
		//Run any timer logic or shiiiit here
		string aspect = "game";
		string method = "start";
		string url = DEBUG_HOST + "/" + aspect + "/" + method;
		new WWW(url);
		GetNewPatient();
	}

	public void EndGame() {
		Debug.Log ("We're Done. FUCK JON");
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
		WWWForm wwwForm = new WWWForm ();
		wwwForm.AddField ("test", "true");
		new WWW(url, wwwForm);
	}

	IEnumerator CallStateEndpoint() {
		string url = DEBUG_HOST + "/game/state";
		WWW www = new WWW (url);
		yield return www;
		string jsonString = www.text;
		currentGameState = JsonUtility.FromJson<GameState> (jsonString);
		if(currentGameState.state == "ended"){
			EndGame ();
		}
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