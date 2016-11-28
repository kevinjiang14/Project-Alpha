using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour {

	private List<GameObject> buffs;
	public GameObject buffSlot;

	// Use this for initialization
	void Start () {
		buffs = new List<GameObject> ();
	}

	public void AddBuff(GameObject buff){
		buffs.Add (buff);
		GameObject slot = Instantiate (buffSlot);
		slot.GetComponent<BuffSlotManager> ().SetBuff (buff);
		slot.transform.SetParent (transform.FindChild("Layout"));
	}

	public void RemoveBuff(GameObject buff){
		int index = buffs.IndexOf (buff);
		buffs.Remove (buff);
		Destroy(transform.FindChild("Layout").GetChild (index).gameObject);
	}
}
