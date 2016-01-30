using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HandbookSymptomListItem : MonoBehaviour {

	private const int DEFAULT_VERTICAL_BASE_SCALE = 100;
	private const int EXPANDED_VERTICAL_BASE_SCALE = 75;
	private const int EXPANDED_VERTICAL_ITEM_SCALE = 40;

	[SerializeField]
	private Text nameText;
	[SerializeField]
	private GameObject CauseItemPrefab;

	public HandbookManager HandbookManager;
	private List<GameObject> causes;
	private Symptom symptom;

	private LayoutElement layoutElement;

	private bool isExpanded;

	void Start(){
		layoutElement = GetComponent<LayoutElement> ();
	}

	public Symptom Symptom{
		get{
			return symptom;
		}
		set{
			symptom = value;
			nameText.text = symptom.Name;
		}
	}

	public void Expand(){
		
		if (isExpanded) {
			HandbookManager.ContractAll ();
		} else {
			HandbookManager.ContractAll ();

			if (causes == null) {
				causes = new List<GameObject> ();
			}
			int scaleCounter = EXPANDED_VERTICAL_BASE_SCALE;
			foreach (string cause in symptom.causes) {
				causes.Add (GenerateCauseItem (cause));
				scaleCounter += EXPANDED_VERTICAL_ITEM_SCALE;
			}

			layoutElement.minHeight = scaleCounter;

			isExpanded = true;
		}
	}

	public void Contract(){
		if (causes != null) {
			if (causes.Count > 0) {
				foreach (GameObject gameobject in causes) {
					Destroy (gameobject);
				}
			}
		}

		layoutElement.minHeight = DEFAULT_VERTICAL_BASE_SCALE;

		isExpanded = false;
	}

	private GameObject GenerateCauseItem(string name){
		GameObject causeListItem = (GameObject)Instantiate (CauseItemPrefab);
		SymptomCauseListItem symptomCause = causeListItem.GetComponent<SymptomCauseListItem> ();
		symptomCause.Name = name;

		causeListItem.transform.SetParent (transform);
		causeListItem.transform.localPosition = new Vector3(0, 0, 0);
		causeListItem.transform.localScale = new Vector3 (1f, 1f, 1f);
		causeListItem.transform.localRotation = new Quaternion();

		return causeListItem;
	}
}
