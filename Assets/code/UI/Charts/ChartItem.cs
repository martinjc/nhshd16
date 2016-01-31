using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChartItem : MonoBehaviour {

	[SerializeField]
	private Image image;

	public void SetColor(Color color){
		image.color = color;
	}
}
