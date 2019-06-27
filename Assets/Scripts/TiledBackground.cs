using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiledBackground : MonoBehaviour {
	public int textureSize = 32;
	public bool scaleHorizontially = true;
	public bool scaleVertically = true;

	public static float scale = 1f;
    public Vector2 nativeResolution = new Vector2(240, 160);
    public static float pixelsToUnits = 1f;
	public Camera cam;
	// Use this for initialization
	void Start () {
        scale = Screen.height / nativeResolution.y;
        pixelsToUnits *= scale;
		var newWidth = !scaleHorizontially ? 1 : Mathf.Ceil(Screen.height*2 / (textureSize * scale));
		var newHeight = !scaleVertically ? 1 : Mathf.Ceil(Screen.height / (textureSize * PixelPerfectCamera.scale));
		transform.localScale = new Vector3(newWidth * textureSize, newHeight * textureSize, 1);
		GetComponent<Renderer>().material.mainTextureScale = new Vector3(newWidth, newHeight, 1);
		if(GameObject.Find("Foreground") != null){
			GameObject.Find("Foreground").GetComponent<Transform>().localPosition = new Vector3(0,-62-1);

		}
	}
}
