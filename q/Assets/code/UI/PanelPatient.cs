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

	void Reset(){
		patientPreview = GetComponentInChildren<PatientPreview> ();
	}
}
