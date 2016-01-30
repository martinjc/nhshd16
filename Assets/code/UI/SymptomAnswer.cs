using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SymptomAnswer : MonoBehaviour {

	[SerializeField]
	private Text text;

	public string Text {
		get {
			return text.text;
		}
		set {
			text.text = value;
		}
	}

	public void Animate(){
		GetComponent<Animator> ().SetTrigger ("Activate");
	}
}
