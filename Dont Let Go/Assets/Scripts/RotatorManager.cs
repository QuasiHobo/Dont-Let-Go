using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotatorManager : MonoBehaviour {
		
	// Singleton
	private static RotatorManager instance;
	// Construct
	private RotatorManager ()
	{
	}
	// Instance
	public static RotatorManager Instance {
		get {
			if (instance == null)
				instance = GameObject.FindObjectOfType (typeof(RotatorManager)) as RotatorManager;

			return instance;
		}
	}
		
	float touchSpeed = 20f; //16-18 for mobile builds

	Rigidbody myRB;
	bool leftMovePressed = false;
	bool rightMovePressed = false;
	public bool doingHug = false;

	public Renderer char1Heart;
	public Renderer char2Heart;
	Color heartStartColor;
	public Color heartEndColor;

	public Image hugbar_Right;
	public Image hugbar_Left;
	public float hugDuration = 2f;
	float hugRegeneration = 85f;

	Color hugBarColor_Start = new Color32(98,191,223,255);
	Color hugBarColor_Collect = new Color32(189,238,255,255);

	int tapAmount = 0;

	public delegate void OnHugEvent();
	public static event OnHugEvent OnHug;

	public delegate void OnStopHugEvent();
	public static event OnStopHugEvent OnStopHug;


	void Start () 
	{
		//Events
		CollectDetector.OnCollectBoost += BoostStarted;
		CollectDetector.OnCollect += HugCollectSmall;
		CollectDetector.OnCollectBig += HugCollectBig;

		tapAmount = 0;
		heartStartColor = char1Heart.material.color;
		myRB = this.GetComponent<Rigidbody> ();
		hugbar_Right.fillAmount = 1f;
		hugbar_Left.fillAmount = 1f;
	}

	void OnDisable()
	{
		CollectDetector.OnCollectBoost -= BoostStarted;
		CollectDetector.OnCollect -= HugCollectSmall;
		CollectDetector.OnCollectBig -= HugCollectBig;
	}
	void BoostStarted()
	{
		
	}
	void HugCollectSmall()
	{
		hugbar_Right.fillAmount += 0.015f;
		hugbar_Left.fillAmount += 0.015f;
		StartCoroutine("ChangeFillbar");
	}
	void HugCollectBig()
	{
		hugbar_Right.fillAmount += 0.1f;
		hugbar_Left.fillAmount += 0.1f;
		StartCoroutine("ChangeFillbar");
	}
	IEnumerator ChangeFillbar()
	{
		hugbar_Left.color = hugBarColor_Collect;
		hugbar_Right.color = hugBarColor_Collect;
		yield return new WaitForSeconds(0.15f);
		hugbar_Left.color = hugBarColor_Start;
		hugbar_Right.color = hugBarColor_Start;
		yield return null;
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

			if (!doingHug && hugbar_Right.fillAmount <= 1) {
				hugbar_Right.fillAmount += Time.deltaTime / hugRegeneration;
				hugbar_Left.fillAmount += Time.deltaTime / hugRegeneration;
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
			if (hugbar_Right.fillAmount > 0) 
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
			if (hugbar_Right.fillAmount > 0) 
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
				if (hugbar_Right.fillAmount > 0) {
					hugbar_Right.fillAmount -= Time.deltaTime / hugDuration;
					hugbar_Left.fillAmount -= Time.deltaTime / hugDuration;
				}
				if (hugbar_Right.fillAmount <= 0) {
					doingHug = false;
				}
				yield return null;
			}
			OnStopHug ();
		}
	}
}
