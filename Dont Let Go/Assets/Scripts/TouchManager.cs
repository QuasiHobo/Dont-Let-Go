using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {
//	
//	// Singleton
//	private static TouchManager instance;
//	// Construct
//	private TouchManager ()
//	{
//	}
//	// Instance
//	public static TouchManager Instance {
//		get {
//			if (instance == null)
//				instance = GameObject.FindObjectOfType (typeof(TouchManager)) as TouchManager;
//			
//			return instance;
//		}
//	}
//	
//	//public variables
//	float comfortZoneForHorizontalSwipe = 4f;
//	float comfortZoneForVerticalSwipe = 3f;
//	float relativeComfortZone = 2f;
//	float minSwipeDistHorizontal = Screen.width/11f;
//	float minSwipeDistVertical = Screen.height/13f;
//	
//	float minSpeedSwipe = 0.8f;
//	float maxSpeedWipe = 25f;
//	
//	float minSpeedBoostSwipe = 1.2f;
//	float maxSpeedBoostSwipe = 25f;
//	
//	public Vector2 distanceInScreenWidths;
//	public float speedInScreenWidthsX;
//	public float speedInScreenWidthsY;
//	
//	//is used for swipeDetector
//    private float startTime;
//    private Vector2 startPos;
//	
//	//Is used for fingertracking
//	private float relativeTouchPos;
//	private float valueToPass;
//	private float DetailedValue;
//	
//	//Enum Inputstates
//	public enum InputState {
//		Swipe,
//		Fingertracking,
//		Rat,
//	}
//
//	public InputState _inputState = TouchManager.InputState.Rat;
//
//	public delegate void OnBoostButtonPressEvent();
//	public static event OnBoostButtonPressEvent OnBoostButtonPress;	
//	
//	public delegate void OnSteeringSwipeEvent(float steeringInput, string swipeDirection);
//	public static event OnSteeringSwipeEvent OnSteeringSwipe;	
//	
////	public delegate void OnSteeringRelativeEvent(float steeringInput);
////	public static event OnSteeringRelativeEvent OnSteeringRelative; 
//	
//	public delegate void OnSteeringRatEvent(float steeringInput, float detailedSteeringInput);
//	public static event OnSteeringRatEvent OnSteeringRat;
//	
////	public delegate void OnSteeringNoTouchEvent();
////	public static event OnSteeringNoTouchEvent OnSteeringNoTouch;
//	
//	public delegate void OnTakeControlEvent();
//	public static event OnTakeControlEvent OnTakeControl;
//
//	public delegate void OnCarChangeEvent();
//	public static event OnCarChangeEvent OnCarChange;
//	
//    void  Update()
//    {
//		
//		if(Input.GetKeyDown(KeyCode.Escape))
//		{
//			Application.Quit();
//		}
//		
//		switch(GameManager._gameState)
//		{
//		case GameManager.GameState.Driving:
//			StartCoroutine("DrivingState");
//			break;
//		case GameManager.GameState.InCamp:
//			StartCoroutine("InCampState");
//			break;
//		}
//	}
//	IEnumerator InCampState()
//	{
//		int fingerCount = 0;
//		
//		foreach (Touch touch in Input.touches) 
//		{
//			if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
//			{
//				fingerCount++;
//			}
//		}
//		
//		if(Input.touchCount > 0)
//		{
//			Touch touch = Input.touches[0];	
//			SwipeState(touch, fingerCount);
//		}
//		yield return null;
//	}
//	IEnumerator DrivingState() 
//	{
//		int fingerCount = 0;
//		
//        foreach (Touch touch in Input.touches) 
//		{
//            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
//			{
//            	fingerCount++;
//			}
//		}
//		
//		if(Input.touchCount > 0)
//		{
//			Touch touch = Input.touches[0];	
//			
//			RatState(touch, fingerCount);
//			SwipeState(touch, fingerCount);
//		}
//		yield return null;
//    }
//	
//	void RatState(Touch touch, int fingerCount)
//	{
//		if(fingerCount < 2)
//		{
//			if(touch.phase == TouchPhase.Began)
//			{
//			OnTakeControl();
//			}
//			
//			relativeTouchPos = touch.position.x-(Screen.width/2);
//			
//			DetailedValue = relativeTouchPos*relativeComfortZone/(Screen.width/2);
//			valueToPass = relativeTouchPos*relativeComfortZone/(Screen.width/2);
//
//			if(valueToPass > 0.85)
//			valueToPass = 1;
//			if(valueToPass < -0.85)
//			valueToPass = -1;
//			
//			if(DetailedValue < 0 && DetailedValue > -0.25f)
//			DetailedValue = 0;
//			if(DetailedValue > 0 && DetailedValue < 0.25f)
//			DetailedValue = 0;
//			
//			OnSteeringRat(valueToPass, DetailedValue);
//		}
//	}
//	void SwipeState(Touch touch, int fingerCount)
//	{
//		distanceInScreenWidths = Input.GetTouch(0).deltaPosition / Screen.width;
//		
//		if(distanceInScreenWidths.x > 0 || distanceInScreenWidths.x < 0)
//		{
//			speedInScreenWidthsX = distanceInScreenWidths.x / Input.GetTouch(0).deltaTime;
//		}
//		if(distanceInScreenWidths.y > 0 || distanceInScreenWidths.y < 0)
//		{
//			speedInScreenWidthsY = distanceInScreenWidths.y / Input.GetTouch(0).deltaTime;
//		}
//
//			if(touch.phase == TouchPhase.Began)
//			{
//				startTime = Time.time;
//				startPos = touch.position;
//			}
//			if(touch.phase == TouchPhase.Moved)
//			{
//				if(speedInScreenWidthsX < comfortZoneForVerticalSwipe && speedInScreenWidthsX > -comfortZoneForVerticalSwipe)
//				{
//					float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
//					
//					if(swipeDistVertical > minSwipeDistVertical)
//					{
//						if(speedInScreenWidthsY < maxSpeedBoostSwipe && speedInScreenWidthsY > minSpeedBoostSwipe)
//						{
//							if(CarClassManager.carInUse != "Tractor")
//							{
//								if(GameManager._gameState == GameManager.GameState.Driving)
//								OnBoostButtonPress();
//							}
//						}
//					}
//				}
//			}
//		
//			if(touch.phase == TouchPhase.Ended)
//			{
//				if(speedInScreenWidthsY < comfortZoneForHorizontalSwipe)
//				{
//					float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
//					
//				
//					if(swipeDistHorizontal > minSwipeDistHorizontal)
//					{
//						if(speedInScreenWidthsX > minSpeedSwipe && speedInScreenWidthsX < maxSpeedWipe)
//						{
//							string swipeDirection = "Right";
//						if(GameManager._gameState == GameManager.GameState.Driving)
//							OnSteeringSwipe(1f, swipeDirection);
//						if(GameManager._gameState == GameManager.GameState.InCamp)
//							OnCarChange();
//						}
//						if(speedInScreenWidthsX < -minSpeedSwipe && speedInScreenWidthsX < maxSpeedWipe)
//						{
//							string swipeDirection = "Left";
//						if(GameManager._gameState == GameManager.GameState.Driving)
//							OnSteeringSwipe(-1f, swipeDirection);
//						if(GameManager._gameState == GameManager.GameState.InCamp)
//							OnCarChange();
//						}
//					}
//				}
//			}
//		
//	}
	
//	void FingerTrackState(Touch touch, int fingerCount)
//	{	
//			switch(touch.phase)
//			{	
//			case TouchPhase.Began:
//				break;
//			case TouchPhase.Moved:
//				relativeTouchPos = touch.position.x-(Screen.width/2);
//				valueToPass = relativeTouchPos/(Screen.width/2);
//				OnSteeringRelative(valueToPass);
//				break;
//			case TouchPhase.Ended:
//				break;	
//			}	
//	}

}
	
	



