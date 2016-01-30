using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Patient{

	public string id;

	public string name;
	public int age;
	public string gender;
	public string photo_fpath;
	public string ailment_name;
	public List<string> symptoms;
	public int arrival_time;
	public int ailment_deadline;

	private Sprite icon;
	public Sprite Icon{
		get{
			if(icon == null){
				if(photo_fpath != null){
					icon = Resources.Load("faces/" + photo_fpath, typeof(Sprite)) as Sprite;
				}
			}
			return icon;
		}
	}

}
