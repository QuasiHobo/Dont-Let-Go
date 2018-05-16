using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPScounter : MonoBehaviour {

	public int avgFrameRate;
	public Text display_FPS;

	public void Update()
	{
		float current = 0;
		current = (int)(1f / Time.unscaledDeltaTime);
		avgFrameRate = (int)current;
		display_FPS.text = avgFrameRate.ToString()+ " FPS";
	}


}
