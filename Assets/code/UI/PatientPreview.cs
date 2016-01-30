using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PatientPreview : MonoBehaviour {

	[SerializeField]
	private Text name;
	[SerializeField]
	private Image image;
	[SerializeField]
	private Text age;
	[SerializeField]
	private Text gender;

	public Image Photo{
		get{
			return image;
		}
		set{
			//Set image
		}
	}

	public string Name{
		get{
			return name.text;
		}
		set{
			name.text = value;
		}
	}

	public int Age{
		get{
			return System.Int32.Parse(age.text);
		}
		set{
			age.text = value.ToString ();
		}
	}

	public string Gender{
		get{
			return gender.text;
		}
		set{
			gender.text = value;
		}
	}

}
