using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChartItem : MonoBehaviour {

	[SerializeField]
	private Image image;

	private Color filledColor = new Color(0.7f, 0.7f, 0.7f);

	private Color emptyColor = new Color(0.3f, 0.3f, 0.3f);

//	void Start(){
//		emptyColor = image.color;
//	}

	public void SetFull(){
		image.color = filledColor;
	}

	public void SetEmpty(){
		image.color = emptyColor;
	}
}
