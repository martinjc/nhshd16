using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandbookManager : MonoBehaviour {

	[SerializeField]
	private GameObject HandbookSymptomListItemPrefab;

	private List<HandbookSymptomListItem> symptomItems;

	public void Populate(Handbook handbook){
		symptomItems = new List<HandbookSymptomListItem> ();

		foreach (Symptom symptom in handbook.symptoms) {
			symptomItems.Add(AddButton (symptom));
		}
	}


	private HandbookSymptomListItem AddButton(Symptom symptom){
		GameObject handbookSymptomListItem = (GameObject)Instantiate (HandbookSymptomListItemPrefab);
		HandbookSymptomListItem handbookSymptom = handbookSymptomListItem.GetComponent<HandbookSymptomListItem> ();
		handbookSymptom.Symptom = symptom;
		handbookSymptom.HandbookManager = this;

		handbookSymptomListItem.transform.SetParent (transform);
		handbookSymptomListItem.transform.localPosition = new Vector3(0, 0, 0);
		handbookSymptomListItem.transform.localScale = new Vector3 (1f, 1f, 1f);
		handbookSymptomListItem.transform.localRotation = new Quaternion();

		return handbookSymptom;
	}

	public void ContractAll(){
		foreach (HandbookSymptomListItem item in symptomItems) {
			item.Contract ();
		}
	}
}
