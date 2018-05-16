using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharShakeScript : MonoBehaviour {

	public Transform charTransform;
	public Transform charBoostPos;

	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
	float tempShakeAmount;
	Vector3 originalPos;

	bool startshake = false;

	void OnEnable()
	{
		CollectDetector.OnCollectBoost += Boost;
		originalPos = charTransform.localPosition;
	}
	void OnDisable()
	{
		CollectDetector.OnCollectBoost -= Boost;
	}

	// Use this for initialization
	void Awake () 
	{

		tempShakeAmount = shakeAmount;

		if (charTransform == null)
		{
			charTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void Boost()
	{
		StartCoroutine("BoostShake");
	}
	IEnumerator BoostShake()
	{
		shakeAmount = 0.5f;

		yield return new WaitForSeconds(1.7f);
		while(GameManager.Instance.boostOngoing)
		{
			if (shakeDuration > 0)
			{
				charTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

				shakeDuration -= Time.deltaTime * decreaseFactor;
			}
			else
			{
				shakeDuration = 0f;
				charTransform.localPosition = originalPos;
			}

			yield return null;
		}

	}

	// Update is called once per frame
	void Update () 
	{

		if(GameManager.Instance.boostOngoing == false && GameManager.Instance.gameStarted == true)
		{
			if (shakeDuration > 0)
			{
				charTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

				shakeDuration -= Time.deltaTime * decreaseFactor;
			}
			else
			{
				shakeDuration = 0f;
				charTransform.localPosition = originalPos;
			}

			if(shakeAmount > tempShakeAmount && shakeAmount < 0.15f)
			{
				shakeAmount -= 0.1f*Time.deltaTime;
			}
			else if(shakeAmount > tempShakeAmount && shakeAmount >= 0.15f)
			{
				shakeAmount -= 1.5f*Time.deltaTime;
			}

		}

		
	}
}
