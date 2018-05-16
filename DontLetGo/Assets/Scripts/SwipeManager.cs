using UnityEngine;
using System.Collections;

public class SwipeManager : MonoBehaviour {

//	// Singleton
//	private static SwipeManager instance;
//	// Construct
//	private SwipeManager ()
//	{
//	}
//	// Instance
//	public static SwipeManager Instance {
//		get {
//			if (instance == null)
//				instance = GameObject.FindObjectOfType (typeof(SwipeManager)) as SwipeManager;
//
//			return instance;
//		}
//	}

	//public variables
//	float comfortZoneForHorizontalSwipe = 3f; //4
//	float comfortZoneForVerticalSwipe = 3f;
//	float relativeComfortZone = 2f; //2
//	float minSwipeDistHorizontal = Screen.width/16f;
//	float minSwipeDistVertical = Screen.height/16f;
//
//	float minSpeedSwipe = 0.01f; //.6
//	float maxSpeedWipe = 1000000f;
//
//	float minSpeedBoostSwipe = 1.2f;
//	float maxSpeedBoostSwipe = 25f;
//
//	public Vector2 distanceInScreenWidths;
//	public float speedInScreenWidthsX;
//	public float speedInScreenWidthsY;
//
//	//is used for swipeDetector
//	private float startTime;
//	private Vector2 startPos;
//
//	//Is used for fingertracking
//	private float relativeTouchPos;
//	private float valueToPass;
//	private float DetailedValue;
//
//	//Events
//	public delegate void OnSteeringSwipeEvent(float steeringInput, string swipeDirection, float swipeSpeed);
//	public static event OnSteeringSwipeEvent OnSteeringSwipe;
//
//	//ParticleSystem for touch feedback
//	public ParticleSystem touchFeedback_1;
//
//	public Camera mainCam;
//
//	void Start()
//	{
//		
//	}
//
//	void Update()
//	{
//		StartCoroutine ("DrivingState");
//	}
//
//	IEnumerator DrivingState() 
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
////			SwipeState(touch, fingerCount);
//			ScreenPressed (touch, fingerCount);
//		}
//		yield return null;
//	}
//
//	void ScreenPressed(Touch touch, int fingerCount)
//	{
//		if (touch.position.x >= Screen.width / 2) 
//		{
//			RightButtonPressed ();
//		}
//		if(touch.position.x < Screen.width / 2 )
//		{
//			LeftButtonPressed ();
//		}
//	}
//
//	public void RightButtonPressed()
//	{
//		string swipeDirection = "Right";
//		OnSteeringSwipe(1f, swipeDirection, -0.1f);
//	}
//	public void LeftButtonPressed()
//	{
//		string swipeDirection = "Left";
//		OnSteeringSwipe(1f, swipeDirection, 0.1f);
//	}
//
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
//		if(touch.phase == TouchPhase.Began)
//		{
//			startTime = Time.time;
//			startPos = touch.position;
//		}
//
//		if(touch.phase == TouchPhase.Ended)
//		{
//			if(speedInScreenWidthsY < comfortZoneForHorizontalSwipe)
//			{
//				float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
//
//
//				if(swipeDistHorizontal > minSwipeDistHorizontal)
//				{
//					if(speedInScreenWidthsX > minSpeedSwipe && speedInScreenWidthsX < maxSpeedWipe)
//					{
//						string swipeDirection = "Right";
//						Debug.Log ("RIGHT SWIPE!!!"+speedInScreenWidthsX);
//						if(OnSteeringSwipe != null)
//						OnSteeringSwipe(1f, swipeDirection, speedInScreenWidthsX);
//					} 
//					if(speedInScreenWidthsX < -minSpeedSwipe && speedInScreenWidthsX < maxSpeedWipe)
//					{
//						string swipeDirection = "Left";
//						Debug.Log ("LEFT SWIPE!!!"+speedInScreenWidthsX);
//						if(OnSteeringSwipe != null)
//						OnSteeringSwipe(1f, swipeDirection, speedInScreenWidthsX);
//					}
//				}
//			}
//			else if(speedInScreenWidthsX < comfortZoneForVerticalSwipe && speedInScreenWidthsX > -comfortZoneForVerticalSwipe)
//			{
//				float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
//								
//					if(swipeDistVertical > minSwipeDistVertical)
//					{
//					if(speedInScreenWidthsY < minSpeedSwipe && speedInScreenWidthsY < maxSpeedWipe)
//						{
//							string swipeDirection = "Down";
//							Debug.Log ("Down SWIPE!!!");
//							if(OnSteeringSwipe != null)
//							OnSteeringSwipe(1f, swipeDirection, speedInScreenWidthsY);
//						}
//					if(speedInScreenWidthsY > minSpeedSwipe && speedInScreenWidthsY < maxSpeedWipe && speedInScreenWidthsX <= 0)
//						{
//							string swipeDirection = "Left";
//							Debug.Log ("UP-LEFT SWIPE!!!");
//							if(OnSteeringSwipe != null)
//							OnSteeringSwipe(1f, swipeDirection, speedInScreenWidthsY+speedInScreenWidthsX/1.5f);
//						}
//					if(speedInScreenWidthsY > minSpeedSwipe && speedInScreenWidthsY < maxSpeedWipe && speedInScreenWidthsX >= 0)
//					{
//						string swipeDirection = "Right";
//						Debug.Log ("UP-RIGHT SWIPE!!!");
//						if(OnSteeringSwipe != null)
//							OnSteeringSwipe(1f, swipeDirection, speedInScreenWidthsY+speedInScreenWidthsX/1.5f);
//					}
//					}
//			}
//		}
//
//	}
}
