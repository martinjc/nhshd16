using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Patient{

	public string ID;

	public string Name;
	public int Age;
	public string Gender;
	public string PhotoFilename;
	public string AilmentName;
	public List<string> Symptoms;

	private Sprite icon;
	public Sprite Icon{
		get{
			if(icon == null){
				if(PhotoFilename != null){
					icon = Resources.Load("faces/" + PhotoFilename, typeof(Sprite)) as Sprite;
				}
			}
			return icon;
		}
	}

}
