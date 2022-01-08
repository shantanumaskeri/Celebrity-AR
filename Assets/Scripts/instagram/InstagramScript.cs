using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstagramScript : MonoBehaviour 
{
	private Texture2D _texture;
	private RenderTexture _renderTexture;

	public void ShareOnInstagram ()
	{
		//StartCoroutine (PostToInstagram());
	}

	IEnumerator PostToInstagram()
	{
		yield return new WaitForEndOfFrame ();

		InstagramShare.PostToInstagram ("Hello from Unity!", GrabScreenshot());
	}

	byte[] GrabScreenshot ()
	{
		if(_texture != null)
			Destroy(_texture);

		// Initialize and render
		_renderTexture = new RenderTexture (Screen.width, Screen.height, 24);
		Camera.main.targetTexture = _renderTexture;
		Camera.main.Render ();
		RenderTexture.active = _renderTexture;

		// Read pixels
		_texture = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
		_texture.ReadPixels (new Rect (0,0,Screen.width,Screen.height), 0, 0);

		// Clean up
		Camera.main.targetTexture = null;
		RenderTexture.active = null;
		DestroyImmediate (_renderTexture);

		return _texture.EncodeToPNG ();
	}
}
