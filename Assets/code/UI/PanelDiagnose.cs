using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelDiagnose : MonoBehaviour {

	private int questionsAsked;
	public Patient patient{ set; private get; }

	[SerializeField]
	private SymptomAnswer symptomAnswerPrefab;

	[SerializeField]
	private Transform answerLocation;

	public void AskForSymptoms(){

		if (questionsAsked < patient.symptoms.Count) {

			SymptomAnswer symptomAnswer = Instantiate (symptomAnswerPrefab,Vector3.zero, Quaternion.identity) as SymptomAnswer;
			symptomAnswer.transform.SetParent (answerLocation);

			symptomAnswer.transform.localScale = Vector3.one;
			symptomAnswer.transform.localPosition = Vector3.zero;

			symptomAnswer.Text = patient.symptoms [questionsAsked];

			symptomAnswer.Animate ();

			questionsAsked++;
		}
	}

	public void Back(){
		questionsAsked = 0;
		DestroyAllChildren (answerLocation);
		PanelManager.instance.ShowPanelPatient ();
	}

	private void DestroyAllChildren(Transform targetTransform){
		var children = new List<GameObject>();
		foreach (Transform child in targetTransform) {
			children.Add (child.gameObject);
		}
		children.ForEach(child => Destroy(child));

	}
}
