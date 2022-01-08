using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationScript : MonoBehaviour 
{
	public static ApplicationScript Instance;
	public GUIText guiDisplay;
	public Canvas canvas;
	public Image[] imageArray;
	public Vector3[] positionArrayLandscape;
	public Vector3[] positionArrayPortrait;

	private bool isFirstTime = true;

	void Start ()
	{
		Instance = this;

		canvas.gameObject.SetActive (false);
	}

	void Update () 
	{
		//guiDisplay.text = GameObject.Find ("Quad").transform.localPosition + "\n" + GameObject.Find ("Quad").transform.localScale;
		CheckDeviceOrientation ();
		QuitApplication ();
	}

	public void ConfigureSceneComponents (bool compFlag)
	{
		canvas.gameObject.SetActive (compFlag);

		if (compFlag == true)
		{
			VideoScript.Instance.PlayVideo ();

			if (isFirstTime)
			{
				Invoke ("InvokeStopVideo", 0.1f);	
			}
		}
		else
		{
			if (VideoScript.Instance != null)
			{
				VideoScript.Instance.StopVideo ();
			}

			isFirstTime = true;
		}

		CheckDeviceOrientation ();
	}

	private void CheckDeviceOrientation ()
	{
		//guiDisplay.text = "Orientation : " + Input.deviceOrientation;
		if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown || Input.deviceOrientation == DeviceOrientation.FaceUp)
		{
			if (VideoScript.Instance.isPlaying)
			{
				//guiDisplay.text = "Am i in portrait : " + facebook.transform.position;
				for (var i = 0; i < imageArray.Length; i++)
				{
					imageArray[i].rectTransform.localPosition = positionArrayPortrait[i];
				}
			}	
		}
		else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight || Input.deviceOrientation == DeviceOrientation.FaceUp)
		{
			if (VideoScript.Instance.isPlaying)
			{
				//guiDisplay.text = "Am i in landscape : " + facebook.transform.position;
				for (var i = 0; i < imageArray.Length; i++)
				{
					imageArray[i].rectTransform.localPosition = positionArrayLandscape[i];
				}
			}	
		}
	}

	private void QuitApplication ()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			Application.Quit ();
		}
	}

	private void InvokeStopVideo ()
	{
		VideoScript.Instance.StopVideo ();
		isFirstTime = false;
	}
}
