using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SymptomCauseListItem : MonoBehaviour {

	[SerializeField]
	private Text nameText;

	public string Name{
		get{
			return nameText.text;
		}
		set{
			nameText.text = value;
		}
	}
}
