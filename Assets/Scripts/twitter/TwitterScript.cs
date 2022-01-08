using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitterKit.Unity;
using UnityEngine.UI;
using System.IO;

public class TwitterScript : MonoBehaviour 
{
	public GUIText guiDisplay;
	public Image camera;

	private string path;
	private string twitterURL = "";
	private TwitterSession session;

	private void Start ()
	{
		Twitter.Init ();
		//guiDisplay.text = "Init called";
	}

	#region Login/Logout
	public void TwitterLogin ()
	{
		if (!camera.gameObject.activeSelf)
		{
			//guiDisplay.text = "Login called";
			TwitterSession _session = Twitter.Session;
			if (_session == null)
			{
				Twitter.LogIn (LoginComplete, LoginFailure);
			} 
			else
			{
				LoginComplete (_session);
			}	
		}
	}

	private void LoginComplete (TwitterSession _session)
	{
		//guiDisplay.text = "Login successful";
		ShareOnTwitter (_session);
	}

	private void LoginFailure (ApiError error)
	{
		Debug.Log ("code = " + error.code + " msg = " + error.message);
	}

	public void TwitterLogout ()
	{
		Twitter.LogOut ();
	}
	#endregion

	private void ShareOnTwitter (TwitterSession _session)
	{
		//guiDisplay.text = "Twitter share called";
		session = _session;
		camera.gameObject.SetActive (true);
	}

	public void SaveScreenshot ()
	{
		//guiDisplay.text = "Screenshot saving begins";
		RenderTexture rt = new RenderTexture (Screen.width, Screen.height, 24);
		Camera.main.targetTexture = rt;
		Texture2D screenShot = new Texture2D (rt.width, rt.height, TextureFormat.RGB24, false);
		Camera.main.Render ();
		RenderTexture.active = rt;
		screenShot.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);
		Camera.main.targetTexture = null;
		RenderTexture.active = null;

		byte[] bytes;
		bytes = screenShot.EncodeToJPG ();
		string date = System.DateTime.Now.ToString ("hh-mm-ss_dd-MM-yy");
		string screenshotFilename = "Screenshot" + "_" + date + ".jpg";

		#if UNITY_ANDROID && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.Android) 
		{
			path = Application.persistentDataPath + "/" + screenshotFilename;
			string androidPath = Path.Combine ("Star India/Photos", screenshotFilename);
			path = Path.Combine (Application.persistentDataPath, androidPath);
			string pathonly = Path.GetDirectoryName (path);
			Directory.CreateDirectory (pathonly);
		}
		#endif

		#if UNITY_IPHONE && !UNITY_EDITOR
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			path = "file://" + Application.persistentDataPath + "/" + screenshotFilename;
			string iosPath = Path.Combine ("Star India/Photos", screenshotFilename);
			path = Path.Combine ("file://" + Application.persistentDataPath, iosPath);
			string pathonly = Path.GetDirectoryName (path);
			Directory.CreateDirectory (pathonly);
		}
		#endif

		System.IO.File.WriteAllBytes (path, bytes);
		Destroy (rt);
		//guiDisplay.text = "Screenshot saved, now composing tweet";

		camera.gameObject.SetActive (false);
		twitterURL = "file://" + path;
		string[] hashtags = {"#TwitterTest", "#AR"};
		Twitter.Compose (session, twitterURL, "Watch this Live", hashtags, OnTwitterComposeSuccess, OnTwitterComposeFail, OnTwitterComposeCancel);
	}

	private void OnTwitterComposeSuccess (string _info) 
	{
		//guiDisplay.text = "Twitter Login Success: " + _info;
	}

	private void OnTwitterComposeFail (ApiError _error) 
	{
		//guiDisplay.text = "Twitter Compose Error: " + _error.message;
	}

	private void OnTwitterComposeCancel () 
	{
		//guiDisplay.text = "Twitter Compose Cancelled!";
	}
}
