using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour
{
	public Transform PanelActive;
	public Transform PanelInActive;

	public Transform PanelPatient;
	public Transform PanelStart;

	public static PanelManager _instance;

	public static PanelManager instance{
		get{
			if(_instance == null){
				_instance = GameObject.FindObjectOfType<PanelManager>();

				if(_instance != null){

				}else{
					GameObject DataManager = new GameObject("PanelManager");
					_instance = DataManager.AddComponent<PanelManager>();

				}
				DontDestroyOnLoad(_instance.gameObject);
			}

			return _instance;
		}
	}

	void Awake(){
		if(_instance == null){
			_instance = this;
			DontDestroyOnLoad(this);
		}else{
			if(this != _instance)
				Destroy(this.gameObject);
		}
	}

	//static private PanelManager panelmanager;
	public void Show (Transform panel){
		if (_instance != null)
		{
			while (_instance.PanelActive.childCount > 0)
				_instance.PanelActive.GetChild (0).transform.SetParent (_instance.PanelInActive);

			panel.SetParent (_instance.PanelActive);
		}
	}

	// Use this for initialization
	void Start (){
		Show (PanelStart);
	}

	public void ShowPanelPatient(){
		Show (PanelPatient);
	}
}
