using UnityEngine;
using System.Collections;

public class FloorIlluminationController : MonoBehaviour {

	private Texture2D floorGridTex;
	private Vector2 floorMainTexScale;
	private Color[] colors;

	private Renderer floorRender;

	// Use this for initialization
	void Start () {
		floorRender = GetComponent<Renderer> ();
		//init texture details for floor illumination
		floorMainTexScale = floorRender.material.mainTextureScale;
		floorGridTex = new Texture2D ((int)floorMainTexScale.x, (int)floorMainTexScale.y, TextureFormat.ARGB32, true, true);
		floorGridTex.filterMode = FilterMode.Point;
		floorRender.material.SetTexture("_GridTex",floorGridTex);
		//TODO - replace with some kind of Blit so we don't have to keep all these color object in memory
		colors = new Color[floorGridTex.width * floorGridTex.height];
		for (int i = 0; i < floorGridTex.width * floorGridTex.height; ++i) {
			colors [i] = Color.white;
		}
		floorGridTex.SetPixels(0,0,floorGridTex.width,floorGridTex.height,colors);
		floorGridTex.Apply ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnWillRenderObject() {
		floorGridTex.Apply ();
	}

	void OnRenderObject() {
		//resetGridIllumination ();
	}
		
	public void resetGridIllumination() {
		//TODO - replace with some kind of Blit so we don't have to keep all these color object in memory
		floorGridTex.SetPixels(0,0,floorGridTex.width,floorGridTex.height,colors);
	}

	public void illuminateGridAtPositionWithColor(float xPos, float yPos, Color illuminationColor) {
		int xOffset = (int)(xPos * floorMainTexScale.x/transform.localScale.x);
		int yOffset = (int)(yPos * floorMainTexScale.y/transform.localScale.y);
		floorGridTex.SetPixel (xOffset, yOffset, illuminationColor);
	}
}
