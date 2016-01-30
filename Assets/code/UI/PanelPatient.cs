using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelPatient : MonoBehaviour {

	[SerializeField]
	PatientPreview patientPreview;

	public void Populate(Patient patient){
		patientPreview.Name = patient.Name;
		patientPreview.Age = patient.Age;
		patientPreview.Gender = patient.Gender;
	}

	//Have put this here so we can hook in any special UI stuff
	public void Diagnose(){
		PanelManager.instance.ShowPanelDiagnose ();
	}

	void Reset(){
		patientPreview = GetComponentInChildren<PatientPreview> ();
	}
}
