using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using Facebook.MiniJSON;
using System;
using System.Collections;

public class FacebookScript : MonoBehaviour 
{
	public GUIText guiDisplay;

	private void Awake ()
	{
		if (!FB.IsInitialized)
		{
			FB.Init(() =>
				{
					if (FB.IsInitialized)
						FB.ActivateApp ();
					else
						Debug.LogError ("Couldn't initialize");
				},
				isGameShown =>
				{
					if (!isGameShown)
						Time.timeScale = 0;
					else
						Time.timeScale = 1;
				});
		}
		else
			FB.ActivateApp ();
	}

	#region Login/Logout
	public void FacebookLogin ()
	{
		var permissions = new List<string>() { "public_profile", "email", "user_friends" };
		FB.LogInWithReadPermissions (permissions);
	}

	public void FacebookLogout ()
	{
		FB.LogOut ();
	}
	#endregion

	public void ShareOnFacebook ()
	{
		FB.ShareLink (contentTitle:"Facebook Test",  contentURL:new System.Uri ("http://dev.happyfinish.com/StAR_India/Chroma_Video.mp4"), contentDescription:"Watch this Live AR!", callback:OnFacebookShareComplete);
		//StartCoroutine (UploadVideo ());
	}

	private void OnFacebookShareComplete (IShareResult result)
	{
		if (result.Cancelled || !string.IsNullOrEmpty (result.Error))
		{
			//guiDisplay.text = "Error in sharing: " + result.Error;
		}
		else if (!string.IsNullOrEmpty (result.PostId))
		{
			//guiDisplay.text = "Post id: " + result.PostId;
		}
		else
		{
			//guiDisplay.text = "Successful share";
		}
	}

	private IEnumerator UploadVideo ()
	{
		yield return new WaitForEndOfFrame ();

		WWW www = new WWW ("http://dev.happyfinish.com/StAR_India/Chroma_Video.mp4");
		while (!www.isDone)
		{
			yield return null;
			//guiDisplay.text = "Progress: " + www.progress;
		}
		//guiDisplay.text = "Size: " + www.size;
		var wwwForm = new WWWForm ();
		wwwForm.AddBinaryData ("file", www.bytes, "Chroma_Video.mp4", "multipart/form-data");
		wwwForm.AddField ("title", "Facebook Test");
		wwwForm.AddField ("description", "Watch this AR Live!");

		FB.API ("me/videos", HttpMethod.POST, UploadSuccess, wwwForm);
	}

	private void UploadSuccess (IGraphResult result)
	{
		//guiDisplay.text = "result: " + result.ToString () + " , error: " + result.Error;
	}
}