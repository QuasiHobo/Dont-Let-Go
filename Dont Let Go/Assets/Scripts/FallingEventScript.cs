using UnityEngine;
using System.Collections;

public class FallingEventScript : MonoBehaviour {

	public GameObject fallHitObj;

	public delegate void OnFallHitEvent(string charNumb);
	public static event OnFallHitEvent OnFallHit;

	string thisCharNumb;

	void Start()
	{
		if (this.gameObject.tag == "Char1_fys")
			thisCharNumb = "1";
		if (this.gameObject.tag == "Char2_fys")
			thisCharNumb = "2";
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "ColorFadeObj") 
		{
			OnFallHit (thisCharNumb);
		}
	}
}
