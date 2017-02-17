using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour {

	public Image[] loadingAnim; 
	public Image loadingImage;
	public int num = 0;

	// Use this for initialization
	void Start () {
		loadingImage = GetComponent<Image> ();
		loadingAnim = Resources.LoadAll<Image> ("loading");
	}
	
	// Update is called once per frame
	void Update () {
		loadingImage = loadingAnim [num];
		num++;
		num %= 120;
	}
}
