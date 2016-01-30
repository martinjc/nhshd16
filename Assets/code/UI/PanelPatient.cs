using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelPatient : MonoBehaviour {

	[SerializeField]
	PatientPreview patientPreview;

	public void Populate(Patient patient){
		patientPreview.Name = patient.name;
		patientPreview.Age = patient.age;
		patientPreview.Gender = patient.sex;
	}

	//Have put this here so we can hook in any special UI stuff
	public void Diagnose(){
		PanelManager.instance.ShowPanelDiagnose ();
	}

	void Reset(){
		patientPreview = GetComponentInChildren<PatientPreview> ();
	}
}
