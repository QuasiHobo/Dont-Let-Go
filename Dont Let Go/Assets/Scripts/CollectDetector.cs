using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectDetector : MonoBehaviour {

	public delegate void OnCollectCollisionEvent();
	public static event OnCollectCollisionEvent OnCollect;

	public delegate void OnCollectBoostCollisionEvent();
	public static event OnCollectBoostCollisionEvent OnCollectBoost;

	public delegate void OnCollectBigCollisionEvent();
	public static event OnCollectBigCollisionEvent OnCollectBig;

	GameObject collectEffect_big;
	GameObject collectEffect_boost;

	Camera mainCam;
	Color startColor = Color.white;
	Color collectColor = new Color32(233,248,255,1);
	bool colorChanging = false;

	SkinnedMeshRenderer charRenderer;
	float fresnelSpreadInit = 2.85f;
	public float fresnelTemp;

	AudioSource myAudio;

	// Use this for initialization
	void Start () 
	{
		collectEffect_big = Resources.Load("Prefabs/Effects/Effect_Collectable_1") as GameObject;
		collectEffect_boost = Resources.Load("Prefabs/Effects/Effect_Collectable_2") as GameObject;
		mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

		if(this.gameObject.tag == "legRot_char1")
		{
			charRenderer = GameObject.FindGameObjectWithTag("charMesh_1").GetComponent<SkinnedMeshRenderer>();
		}
		if(this.gameObject.tag == "legRot_char2")
		{
			charRenderer = GameObject.FindGameObjectWithTag("charMesh_2").GetComponent<SkinnedMeshRenderer>();
		}
		fresnelTemp = charRenderer.material.GetFloat("_FresnelSpread");

		myAudio = this.gameObject.GetComponent<AudioSource>();
	}
	
	void OnTriggerEnter (Collider collider)
	{
		if(GameManager.Instance.gameOver == false)
		{
			if (collider.tag == "Collectable") 
			{
//			GameObject tempEffect = Instantiate(collectEffect_big, this.gameObject.transform.position, collectEffect_boost.transform.rotation) as GameObject;
//			tempEffect.transform.parent = this.gameObject.transform;
				collider.gameObject.GetComponent<CollectableController>().WasCollected();
				//Destroy (collider.gameObject);
				OnCollect ();
				StartCoroutine("CamCollectColor");
				StartCoroutine("CollectEffect");
				myAudio.Play();
			}
			if(collider.tag == "Collectable_Boost")
			{
				Destroy (collider.gameObject);
//			GameObject tempEffect = Instantiate(collectEffect_boost, this.gameObject.transform.position, collectEffect_boost.transform.rotation) as GameObject;
//			tempEffect.transform.parent = this.gameObject.transform;

				if(GameManager.Instance.boostOngoing == false)
				OnCollectBoost();
				myAudio.Play();
			
			}
			if(collider.tag == "Collectable_Big")
			{
				Destroy (collider.gameObject);
//			GameObject tempEffect = Instantiate(collectEffect_big, this.gameObject.transform.position, collectEffect_big.transform.rotation) as GameObject;
//			tempEffect.transform.parent = this.gameObject.transform;
				OnCollectBig ();
				StartCoroutine("CollectEffect");
				myAudio.Play();
			}
		}
	}

	IEnumerator CamCollectColor()
	{
		
		if(!colorChanging)
		{
			colorChanging = true;
			float t = 0;
			startColor = mainCam.backgroundColor;
			while(t < 1)
			{
				t += Time.deltaTime * 8f;
				mainCam.backgroundColor = Color.Lerp(startColor, collectColor, t);
				yield return null;
			}
		t = 0;
			while(t < 1)
			{
				t += Time.deltaTime * 8f;
				mainCam.backgroundColor = Color.Lerp(collectColor, startColor, t);
				yield return null;
			}
			colorChanging = false;
		}
		yield return null;
	}

	void Update()
	{
		if(fresnelTemp < fresnelSpreadInit)
		{
			fresnelTemp += 3.5f*Time.deltaTime;
		}

		charRenderer.material.SetFloat("_FresnelSpread", fresnelTemp);
	}

	IEnumerator CollectEffect()
	{
		if(fresnelTemp > 1.25)
		{
			fresnelTemp -= 1f;
		}
		yield return null;
	}
}
