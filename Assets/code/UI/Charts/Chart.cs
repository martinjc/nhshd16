using UnityEngine;
using System.Collections;

public class Chart : MonoBehaviour {

	[SerializeField]
	private ChartItem chartItemPrefab;

	public void Set(int totalItems, int filledItems){
		//Debug.Log ("Updating charts =  " + totalItems + "," + filledItems);
		//int changeInTotal = (transform.childCount - totalItems);



		//Debug.Log ("Change = " + changeInTotal);

		int currentCount = transform.childCount;

		Debug.Log ("Current count = " + currentCount);
		Debug.Log ("Total items = " + totalItems);


		if (currentCount < totalItems) {
			while(currentCount < totalItems){
				AddItem ();
				currentCount++;
			}
		} else if(currentCount > totalItems){
			while(currentCount > totalItems){
				RemoveItem ();
				currentCount--;
			}
		}

		Coloring (filledItems);
	}

	private void Coloring(int colouredItems){
		foreach (Transform child in transform) {
			child.GetComponent<ChartItem> ().SetEmpty ();
		}

		int index = 0;
		foreach (Transform child in transform) {
			if (index >= colouredItems) {
				break;
			}
			child.GetComponent<ChartItem> ().SetFull ();
			index++;
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
