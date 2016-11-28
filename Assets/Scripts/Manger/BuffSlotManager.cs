using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuffSlotManager : MonoBehaviour {

	private GameObject buff;

	public void SetBuff(GameObject buff){
		this.buff = buff;
		gameObject.GetComponent<Image> ().sprite = buff.GetComponent<SpriteRenderer> ().sprite;
	}
}
