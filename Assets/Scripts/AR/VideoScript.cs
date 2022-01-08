using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VideoScript : MonoBehaviour 
{
	public static VideoScript Instance;
	public GUIText guiDisplay;
	public MediaPlayerCtrl srcMedia;
	public Image downloadStatic;
	public Image downloadAnimated;
	public Image error;

	[HideInInspector]
	public bool isPlaying = false;

	private Animator animator;
	private string path;

	private void Start () 
	{
		Instance = this;

		animator = downloadAnimated.GetComponent<Animator>();
		animator.speed = 0;

		LoadVideo ();
	}

	private void Update ()
	{
		SeekVideo ();
		CheckTouchInput ();
		//guiDisplay.text = srcMedia.GetSeekPosition () + " : " + srcMedia.GetCurrentSeekPercent ();
	}

	private void LoadVideo ()
	{
		srcMedia.Load ("Chroma_Video.mp4");
		srcMedia.m_bLoop = true;
	}

	private void SeekVideo ()
	{
		if (srcMedia.GetSeekPosition () >= 22000)
		{
			srcMedia.SeekTo (0);
		}	
	}

	public void PlayVideo ()
	{
		srcMedia.Play ();
		isPlaying = true;
	}

	public void StopVideo ()
	{
		srcMedia.Pause ();
		isPlaying = false;
		//srcMedia.Stop ();
		//srcMedia.UnLoad ();
	}
		
	public void DownloadVideo ()
	{
		string url = "http://dev.happyfinish.com/StAR_India/Chroma_Video.mp4";

		StartCoroutine (SaveVideo (url));
	}

	IEnumerator SaveVideo (string url)
	{
		//guiDisplay.text = "Save Video";
		#if UNITY_ANDROID && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.Android) 
		{
			path = Application.persistentDataPath;
			string androidPath = Path.Combine ("Star India/Videos", "Chroma_Video.mp4");
			path = Path.Combine (Application.persistentDataPath, androidPath);
			string pathonly = Path.GetDirectoryName (path);
			Directory.CreateDirectory (pathonly);
		}
		#endif

		#if UNITY_IPHONE && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			path = "file://" + Application.persistentDataPath;
			string iosPath = Path.Combine ("Star India/Videos", "Chroma_Video.mp4");
			path = Path.Combine ("file://" + Application.persistentDataPath, iosPath);
			string pathonly = Path.GetDirectoryName (path);
			Directory.CreateDirectory (pathonly);
		}
		#endif

		WWW www = new WWW (url);

		while (!www.isDone)
		{
			//guiDisplay.text = "Progress: " + Mathf.Round (www.progress * 100) + "%";
			ConfigureUIAndAnimation (1, false, false, true, "Downloading...");
			yield return null;
		}

		if (!string.IsNullOrEmpty (www.error))
		{
			//guiDisplay.text = "Error: " + www.error;
			ConfigureUIAndAnimation (0, true, false, false, "");
			Invoke ("ResetErrorDisplay", 3.0f);
			yield break;
		}
		//guiDisplay.text = "Downloaded to " + path;
		ConfigureUIAndAnimation (0, false, true, false, "");

		File.WriteAllBytes (path, www.bytes);
	}

	private void ConfigureUIAndAnimation (int speed, bool errorFlag, bool staticFlag, bool animFlag, string text)
	{
		animator.speed = speed;
		error.gameObject.SetActive (errorFlag);
		downloadStatic.gameObject.SetActive (staticFlag);
		downloadAnimated.gameObject.SetActive (animFlag);
		downloadAnimated.GetComponentInChildren<Text>().text = text;
	}

	private void ResetErrorDisplay ()
	{
		ConfigureUIAndAnimation (0, false, true, false, "");
	}

	private void CheckTouchInput ()
	{
		for (var i = 0; i < Input.touchCount; i++)
		{
			if (Input.GetTouch (i).phase == TouchPhase.Began)
			{
				if (Input.GetTouch (i).tapCount == 2)
				{
					if (!isPlaying)
					{
						PlayVideo ();
					}
					else
					{
						StopVideo ();
					}
				}
			}
		}
	}
}
