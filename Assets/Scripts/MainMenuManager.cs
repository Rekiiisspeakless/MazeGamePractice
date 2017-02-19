using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class MainMenuManager : MonoBehaviour {
	public AudioSource musicPlayer; 
	public AudioClip buttonClick;
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

	void AudioManagement(){
		if (!musicPlayer.isPlaying) {
			musicPlayer.Play ();
		} 
	}
	void ButtonClick(){
		musicPlayer.Stop ();
		musicPlayer.clip = buttonClick;
		musicPlayer.volume = 0.7f;
		musicPlayer.loop = false;
		musicPlayer.Play ();
	}
}
