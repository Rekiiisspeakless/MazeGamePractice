using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverMenuManager : MonoBehaviour {

	public AudioSource musicPlayer; 
	//public AudioClip buttonClick;
	// Use this for initialization
	void Start () {
		musicPlayer = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void StartGame(){
		ButtonClick ();
		LoadingScreenManager.LoadScene(1);
	}

	public void ExitGame(){
		ButtonClick ();
		Application.Quit ();
	}
	void ButtonClick(){
		musicPlayer.Play ();
	}
}
