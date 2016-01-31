using UnityEngine;
using System.Collections;

public class Chart : MonoBehaviour {

	[SerializeField]
	private ChartItem chartItemPrefab;

	public void Set(int totalItems, int filledItems){
		Debug.Log ("Updating charts.");
		int changeInTotal = -(transform.childCount - totalItems);

		Debug.Log ("Change = " + changeInTotal);

		if (changeInTotal < 0) {
			for (int i = 0; i < Mathf.Abs(changeInTotal); ++i) {
				RemoveItem ();
			}
		} else {
			for (int i = 0; i < changeInTotal; ++i) {
				AddItem ();
			}
		}
	}

	private void RemoveItem(){
		Debug.Log ("Trying to remove items.");
		Destroy (transform.GetChild (transform.childCount - 1).gameObject);
	}

	private void AddItem(){
		ChartItem chartItem = Instantiate (chartItemPrefab) as ChartItem;
		//ChartItem symptomCause = causeListItem.GetComponent<ChartItem> ();

		chartItem.transform.SetParent (transform);
		chartItem.transform.localPosition = new Vector3(0, 0, 0);
		chartItem.transform.localScale = new Vector3 (1f, 1f, 1f);
		chartItem.transform.localRotation = new Quaternion();
	}
}
