using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour
{
	public Transform PanelActive;
	public Transform PanelInActive;
	public Transform PanelMain;

	static private PanelManager panelManager;
	static public void Show (Transform panel)
	{
		if (panelManager == null)
		{
			GameObject gameObject = GameObject.Find ("Canvas");
			if (gameObject != null)
				panelManager = gameObject.GetComponent<PanelManager> ();
		}

		if (panelManager != null)
		{
			while (panelManager.PanelActive.childCount > 0)
				panelManager.PanelActive.GetChild (0).transform.SetParent (panelManager.PanelInActive);

			panel.SetParent (panelManager.PanelActive);
		}
	}

	// Use this for initialization
	void Start (){
		Show (PanelMain);
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
}
