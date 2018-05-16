using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {

	#if UNITY_IOS
	[SerializeField] string gameID = "1771708";
	#elif UNITY_ANDROID
	[SerializeField] string gameID = "1771707";
	#endif

	//events
	public delegate void OnAdEndedEvent();
	public static event OnAdEndedEvent OnAdEnded;

	// Use this for initialization
	void Start () 
	{
		Advertisement.Initialize ("gameId", true);
		GameManager.OnShowSimpleAd += SimpleAd;
	}

	void OnDisable()
	{
		GameManager.OnShowSimpleAd -= SimpleAd;
	}

	void SimpleAd()
	{
//		StartCoroutine("ShowAdWhenReady");
	}

	IEnumerator ShowAdWhenReady()
	{
		while(!Advertisement.IsReady())
			yield return null;
		
		int randomNr = Random.Range(1, 4);
		Debug.Log("random nr: "+randomNr);
		if(randomNr == 1)
		Advertisement.Show();

	}

}
