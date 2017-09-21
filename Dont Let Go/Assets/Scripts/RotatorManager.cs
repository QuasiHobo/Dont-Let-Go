using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotatorManager : MonoBehaviour {
		
	float touchSpeed = 16f; //16-18 for mobile builds

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

	int tapAmount = 0;

	public delegate void OnHugEvent();
	public static event OnHugEvent OnHug;

	public delegate void OnStopHugEvent();
	public static event OnStopHugEvent OnStopHug;


	void Start () 
	{
		//Events
		CollectDetector.OnCollectBoost += BoostStarted;

		tapAmount = 0;
		heartStartColor = char1Heart.material.color;
		myRB = this.GetComponent<Rigidbody> ();
		hugbar.fillAmount = 1;
	}

	void OnDisable()
	{
		CollectDetector.OnCollectBoost -= BoostStarted;
	}
	void BoostStarted()
	{
		
	}
	void Update()
	{
			if (leftMovePressed) {
				myRB.AddRelativeTorque (0, -1 * touchSpeed, 0, ForceMode.Acceleration);
				char1Heart.material.color = Color.Lerp (char1Heart.material.color, heartEndColor*3f, Time.deltaTime * 4f);
			} else
				char1Heart.material.color = Color.Lerp (char1Heart.material.color, heartStartColor, Time.deltaTime * 2f);
			if (rightMovePressed) {
				myRB.AddRelativeTorque (0, -(-1) * touchSpeed, 0, ForceMode.Acceleration);
				char2Heart.material.color = Color.Lerp (char2Heart.material.color, heartEndColor*3f, Time.deltaTime * 4f);
			} else
				char2Heart.material.color = Color.Lerp (char2Heart.material.color, heartStartColor, Time.deltaTime * 2f);

			if (!doingHug && hugbar.fillAmount <= 1) {
				hugbar.fillAmount += Time.deltaTime / hugRegeneration;
			}

		//For testing on PC
		if(Input.GetKeyDown(KeyCode.LeftArrow))
			LeftButtonPressedDown();
		if(Input.GetKeyDown(KeyCode.RightArrow))
			RightButtonPressedDown();
		if(Input.GetKeyUp(KeyCode.LeftArrow))
			LeftButtonPressedUp();
		if(Input.GetKeyUp(KeyCode.RightArrow))
			RightButtonPressedUp();
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
		if(GameManager.Instance.boostOngoing == false)
		{
			doingHug = true;
			OnHug ();
			yield return new WaitForSeconds (0.1f);
			while (doingHug && GameManager.Instance.boostOngoing == false) {
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
}
