using UnityEngine;
using System.Collections;

public class KillParticleScript : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		StartCoroutine("KillMe");
	}
	IEnumerator KillMe()
	{
		yield return new WaitForSeconds(1f);
		Destroy(this.gameObject);
	}

}
