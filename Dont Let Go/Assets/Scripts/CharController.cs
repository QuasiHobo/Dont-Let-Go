using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine.UI;

public class CharController : MonoBehaviour {

	public string charNumb;
	FullBodyBipedIK FBBIK;

	GameObject legController;

	GameObject legPosDown;
	GameObject legPosUp;

	public float startBodyWeight = 0.2f;
	public float hugBodyWeight = 0.0f;
	public float hugDuration = 3.0f;

	public Button hugButton;

	// Use this for initialization
	void Start () 
	{
		hugButton.enabled = true;
		FBBIK = this.gameObject.GetComponent<FullBodyBipedIK> ();

		legController = this.transform.Find ("legRot").gameObject;
		legPosDown = this.transform.Find ("LegPosDown").gameObject;
		legPosUp = this.transform.Find ("LegPosUp").gameObject;

		FBBIK.solver.bodyEffector.positionWeight = startBodyWeight;
		legController.transform.position = legPosDown.transform.position;
	}

	public void HugButtonPressed()
	{
		StartCoroutine ("Hug");
	}

	public IEnumerator Hug()
	{
		hugButton.enabled = false;
		float duration = 0.5f;
		float t = 0f;
		while (t < 1) 
		{
			t += Time.deltaTime / duration;
			legController.transform.position = Vector3.Lerp (legPosDown.transform.position, legPosUp.transform.position, t);
			FBBIK.solver.bodyEffector.positionWeight = Mathf.Lerp (startBodyWeight, hugBodyWeight, t);
			yield return null;
		}

		yield return new WaitForSeconds (hugDuration);
		t = 0f;
		while (t < 1) 
		{
			t += Time.deltaTime / duration;
			legController.transform.position = Vector3.Lerp (legPosUp.transform.position, legPosDown.transform.position, t);
			FBBIK.solver.bodyEffector.positionWeight = Mathf.Lerp (hugBodyWeight, startBodyWeight, t);
			yield return null;
		}
		hugButton.enabled = true;
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
