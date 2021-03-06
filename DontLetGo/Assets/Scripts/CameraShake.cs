﻿using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	public Transform camBoostPos;

	// How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
	float tempShakeAmount;
	Vector3 originalPos;

	bool startshake = false;

	void Awake()
	{
		CharCollison.OnCollision += StopShake;
		CollectDetector.OnCollectBoost += Boost;
		ObstacleManager.OnSpecialStart += SpecialShake;
		ObstacleManager.OnSpecialStop += StopSpecial;
		ScoreDetector.OnCollided += PassShake;
		ObstacleManager.OnSpecialStart += SpecialLevelStarted;
//		ObstacleManager.OnSpecialStop += SpecialLevelStopped;

		tempShakeAmount = shakeAmount;

		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}
	void OnDisable()
	{
		CharCollison.OnCollision -= StopShake;
		CollectDetector.OnCollectBoost -= Boost;
		ObstacleManager.OnSpecialStart -= SpecialShake;
		ObstacleManager.OnSpecialStop -= StopSpecial;
		ScoreDetector.OnCollided -= PassShake;
		ObstacleManager.OnSpecialStart -= SpecialLevelStarted;
//		ObstacleManager.OnSpecialStop -= SpecialLevelStopped;
	}
	void SpecialLevelStarted()
	{
		if(GameManager.Instance.boostOngoing == false && shakeAmount <= 0.25f)
		{
			shakeAmount *= 12f;
		}
	}
	void PassShake()
	{
		if(GameManager.Instance.boostOngoing == false && shakeAmount <= 0.075f)
		{
			shakeAmount *= 6f;
		}
	}
	void SpecialShake()
	{
		shakeAmount *= 1.6f;
	}
	void StopSpecial()
	{
		shakeAmount = tempShakeAmount;
	}
	void StopShake(string charNumb)
	{
		if (charNumb != "lol") {
			shakeAmount = 0.25f; 
			decreaseFactor = 2.2f;
		}
	}
	void Boost()
	{
		StartCoroutine("WaitShake");
	}
	IEnumerator WaitShake()
	{
		yield return new WaitForSeconds(1f);
		startshake = true;
		yield return new WaitForSeconds(GameManager.Instance.boostTime);
		startshake = false;
	}
	void Update()
	{
		if(GameManager.Instance.boostOngoing == false && GameManager.Instance.gameStarted == true)
		{
			if (shakeDuration > 0)
			{
				camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

				shakeDuration -= Time.deltaTime * decreaseFactor;
			}
			else
			{
				shakeDuration = 0f;
				camTransform.localPosition = originalPos;
			}
		}
		if(shakeAmount > tempShakeAmount && shakeAmount < 0.15f)
		{
			shakeAmount -= 0.1f*Time.deltaTime;
		}
		else if(shakeAmount > tempShakeAmount && shakeAmount >= 0.15f)
		{
			shakeAmount -= 1.5f*Time.deltaTime;
		}
//		if(GameManager.Instance.boostOngoing == true && startshake)
//		{
//			if (shakeDuration > 0)
//			{
//				camTransform.localPosition = camBoostPos.localPosition + Random.insideUnitSphere * (shakeAmount*1.85f);
//
//				shakeDuration -= Time.deltaTime * decreaseFactor;
//			}
//		}
	}
}
