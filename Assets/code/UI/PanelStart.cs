using UnityEngine;
using System.Collections;

public class PanelStart : MonoBehaviour {

	[SerializeField]
	private GameManager gameManager;

	private PanelManager panelManager;

	public void StartGame(){
		PanelManager.instance.ShowPanelPatient ();
		gameManager.StartGame ();
	}
}
