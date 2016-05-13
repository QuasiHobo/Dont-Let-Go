using UnityEngine;
using System.Collections;

public class RotatorManager : MonoBehaviour {
		
	Rigidbody myRB;

	void Start () 
	{
		SwipeManager.OnSteeringSwipe += SwipeDetected;
		myRB = this.GetComponent<Rigidbody> ();
//		myRB.AddRelativeTorque (0, 1*500f, 0, ForceMode.Force);
	}

	void OnDisable()
	{
		SwipeManager.OnSteeringSwipe -= SwipeDetected;
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

		myRB.AddRelativeTorque (0, -swipeSpeed*95f, 0, ForceMode.Acceleration);

//		myRB.AddRelativeTorque (0, -swipeSpeed*650f, 0, ForceMode.Force);
		Debug.Log ("Modtager: " + swipeSpeed);
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
