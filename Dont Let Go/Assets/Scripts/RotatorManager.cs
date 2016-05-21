using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotatorManager : MonoBehaviour {
		
	Rigidbody myRB;
	public bool leftMovePressed = false;
	public bool rightMovePressed = false;

	public Renderer char1Heart;
	public Renderer char2Heart;
	Color heartStartColor;
	public Color heartEndColor;

	public delegate void OnHugEvent();
	public static event OnHugEvent OnHug;

	public delegate void OnStopHugEvent();
	public static event OnStopHugEvent OnStopHug;

	void Start () 
	{
		heartStartColor = char1Heart.material.color;
		SwipeManager.OnSteeringSwipe += SwipeDetected;
		myRB = this.GetComponent<Rigidbody> ();
//		myRB.AddRelativeTorque (0, 1*500f, 0, ForceMode.Force);
	}

	void OnDisable()
	{
		SwipeManager.OnSteeringSwipe -= SwipeDetected;
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
	}

	public void LeftButtonPressedUp()
	{
		leftMovePressed = false;
		OnStopHug ();
	}
	public void RightButtonPressedUp()
	{
		rightMovePressed = false;
		OnStopHug ();
	}
	public void LeftButtonPressedDown()
	{
		leftMovePressed = true;
		if (leftMovePressed && rightMovePressed)
			OnHug ();
	}
	public void RightButtonPressedDown()
	{
		rightMovePressed = true;
		if (leftMovePressed && rightMovePressed)
			OnHug ();
	}

	void SwipeDetected(float steeringInput, string swipeDirection, float swipeSpeed)
	{
//		if (swipeSpeed < -1.5f)
//			swipeSpeed = -1.5f;
//		if (swipeSpeed > 1.5f)
//			swipeSpeed = 1.5f;
//		if (swipeSpeed > -0.5f && swipeSpeed <= 0)
//			swipeSpeed = -0.5f;
//		if (swipeSpeed < 0.5f && swipeSpeed > 0)
//			swipeSpeed = 0.5f;

//		myRB.AddRelativeTorque (0, -swipeSpeed*95f, 0, ForceMode.Acceleration);

//		myRB.AddRelativeTorque (0, -swipeSpeed*650f, 0, ForceMode.Force);
//		Debug.Log ("Modtager: " + swipeSpeed);
	}

}
