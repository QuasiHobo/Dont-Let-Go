using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotatorManager : MonoBehaviour {
		
	Rigidbody myRB;
	public bool leftMovePressed = false;
	public bool rightMovePressed = false;
	public bool doingHug = false;

	public Renderer char1Heart;
	public Renderer char2Heart;
	Color heartStartColor;
	public Color heartEndColor;

	public Image hugbar;
	public float hugDuration = 2f;
	public float hugRegeneration = 5f;

	public delegate void OnHugEvent();
	public static event OnHugEvent OnHug;

	public delegate void OnStopHugEvent();
	public static event OnStopHugEvent OnStopHug;


	void Start () 
	{
		heartStartColor = char1Heart.material.color;
		myRB = this.GetComponent<Rigidbody> ();
//		myRB.AddRelativeTorque (0, 1*500f, 0, ForceMode.Force);
		hugbar.fillAmount = 1;
	}

	void OnDisable()
	{

	}

	void Update()
	{
		if (leftMovePressed) {
			myRB.AddRelativeTorque (0, -1*12f, 0, ForceMode.Acceleration);

			char1Heart.material.color = Color.Lerp (char1Heart.material.color, heartEndColor, Time.deltaTime*4f);
		}
		else
			char1Heart.material.color = Color.Lerp (char1Heart.material.color, heartStartColor, Time.deltaTime*2f);
		if (rightMovePressed) {
			myRB.AddRelativeTorque (0, -(-1)*12f, 0, ForceMode.Acceleration);
			char2Heart.material.color = Color.Lerp (char2Heart.material.color, heartEndColor, Time.deltaTime*4f);
		}
		else
			char2Heart.material.color = Color.Lerp (char2Heart.material.color, heartStartColor, Time.deltaTime*2f);

		if (!doingHug && hugbar.fillAmount <= 1) {
			hugbar.fillAmount += Time.deltaTime / hugRegeneration;
		}
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
			if (hugbar.fillAmount > 0) {
				doingHug = true;
				OnHug ();
				StartCoroutine ("HugBarController");
			}
		}
	}
	public void RightButtonPressedDown()
	{
		rightMovePressed = true;
		if (leftMovePressed && rightMovePressed) 
		{
			if (hugbar.fillAmount > 0) {
				doingHug = true;
				OnHug ();
				StartCoroutine ("HugBarController");
			}
		}
	}

	public IEnumerator HugBarController()
	{
		while (doingHug) 
		{
			if (hugbar.fillAmount > 0) 
			{
				hugbar.fillAmount -= Time.deltaTime / hugDuration;
			}
			if (hugbar.fillAmount <= 0) 
			{
				doingHug = false;
			}
			yield return null;
		}

		OnStopHug ();
	}
		

}
