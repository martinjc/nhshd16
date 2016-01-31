using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Handbook {
	
	public List<Symptom> symptoms;

	public string ToString() {
		return JsonUtility.ToJson (this, true);
	}
}
