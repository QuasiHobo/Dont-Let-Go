using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotatorManager : MonoBehaviour {
		
	float touchSpeed = 12f;

	Rigidbody myRB;
	bool leftMovePressed = false;
	bool rightMovePressed = false;
	public bool doingHug = false;

	public Renderer char1Heart;
	public Renderer char2Heart;
	Color heartStartColor;
	public Color heartEndColor;

	public Image hugbar;
	public float hugDuration = 2f;
	public float hugRegeneration = 5f;

	//For seperation mechanic
	public GameObject char1_object;
	public GameObject char2_object;
	public GameObject char1_startPos;
	public GameObject char2_startPos;
	public GameObject char1_endPos;
	public GameObject char2_endPos;
	public GameObject char1_startSepPos;
	public GameObject char2_startSepPos;

	public bool seperated;
	//

	int tapAmount = 0;

	public delegate void OnHugEvent();
	public static event OnHugEvent OnHug;

	public delegate void OnStopHugEvent();
	public static event OnStopHugEvent OnStopHug;


	void Start () 
	{
		//Events
		ObstacleDetector.OnSeperate += StartSeperation;
		ScoreDetector.OnSeperateStop += StopSeperation;

		tapAmount = 0;
		heartStartColor = char1Heart.material.color;
		myRB = this.GetComponent<Rigidbody> ();
		hugbar.fillAmount = 1;
	}

	void OnDisable()
	{
		ObstacleDetector.OnSeperate -= StartSeperation;
		ScoreDetector.OnSeperateStop -= StopSeperation;
	}

	void Update()
	{
			if (leftMovePressed) {
				myRB.AddRelativeTorque (0, -1 * touchSpeed, 0, ForceMode.Acceleration);
				char1Heart.material.color = Color.Lerp (char1Heart.material.color, heartEndColor, Time.deltaTime * 4f);
			} else
				char1Heart.material.color = Color.Lerp (char1Heart.material.color, heartStartColor, Time.deltaTime * 2f);
			if (rightMovePressed) {
				myRB.AddRelativeTorque (0, -(-1) * touchSpeed, 0, ForceMode.Acceleration);
				char2Heart.material.color = Color.Lerp (char2Heart.material.color, heartEndColor, Time.deltaTime * 4f);
			} else
				char2Heart.material.color = Color.Lerp (char2Heart.material.color, heartStartColor, Time.deltaTime * 2f);

			if (!doingHug && hugbar.fillAmount <= 1) {
				hugbar.fillAmount += Time.deltaTime / hugRegeneration;
			}
	}
	void StartSeperation()
	{
		StartCoroutine ("Seperate");
	}
	IEnumerator Seperate()
	{
		float t = 0;
		while (t <= 1) {
			t += Time.deltaTime * 1.5f;
			char1_object.transform.position = Vector3.MoveTowards (char1_startPos.transform.position, char1_endPos.transform.position, t);
			char2_object.transform.position = Vector3.MoveTowards (char2_startPos.transform.position, char2_endPos.transform.position, t);
			yield return null;
			}
	}
	IEnumerator SeperateStop()
	{
		float t = 0;
		while (t <= 1) {
			t += Time.deltaTime * 1.5f;
			char1_object.transform.position = Vector3.MoveTowards (char1_object.transform.position, char1_startPos.transform.position, t);
			char2_object.transform.position = Vector3.MoveTowards (char2_object.transform.position, char2_startPos.transform.position, t);
			yield return null;
		}
	}
		
	void StopSeperation()
	{
		StartCoroutine ("SeperateStop");
	}

	public void LeftButtonPressedUp()
	{
		leftMovePressed = false;
		doingHug = false;
		OnStopHug ();
	}
	public void RightButtonPressedUp()
	{
		rightMovePressed = false;
		doingHug = false;
		OnStopHug ();
	}
	public void LeftButtonPressedDown()
	{
		leftMovePressed = true;
		if (leftMovePressed && rightMovePressed) 
		{
			if (hugbar.fillAmount > 0) 
			{
				StartCoroutine ("DoublePressController");
			}
		}
	}
	public void RightButtonPressedDown()
	{
		rightMovePressed = true;
		if (leftMovePressed && rightMovePressed) 
		{
			if (hugbar.fillAmount > 0) 
			{
				StartCoroutine ("DoublePressController");
			}
		}
	}
	public IEnumerator DoublePressController()
	{
			doingHug = true;
			OnHug ();
			yield return new WaitForSeconds (0.1f);
			while (doingHug) {
				if (hugbar.fillAmount > 0) {
					hugbar.fillAmount -= Time.deltaTime / hugDuration;
				}
				if (hugbar.fillAmount <= 0) {
					doingHug = false;
				}
				yield return null;
			}
			OnStopHug ();
		}
}
