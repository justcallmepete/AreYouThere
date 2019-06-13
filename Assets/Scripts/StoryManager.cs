using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using ControllerSelection;

public class StoryManager : MonoBehaviour {

	public VideoClip clip;
	public bool firstTimeInMenu = true;
	public VideoPlayer videoPlayer;
	public AFKManager afkManager;

	private void OnEnable() {
		OVRManager.HMDMounted += EquippedHeadset;
		OVRManager.HMDUnmounted += UnequippedHeadset;
		videoPlayer.loopPointReached += OnVideoFinished;
	}

	private void OnDisable() {
		OVRManager.HMDMounted += EquippedHeadset;
		videoPlayer.loopPointReached += OnVideoFinished;
		AFKManager.OnReset -= PerformReset;
	}

	private void Start() {
		AFKManager.OnReset += PerformReset;
			if(firstTimeInMenu){
			videoPlayer.clip = clip;
			videoPlayer.Prepare();
			videoPlayer.Play();
		}
		UnequippedHeadset();
		EquippedHeadset();
	}

	void EquippedHeadset(){
		if(firstTimeInMenu){
			videoPlayer.clip = clip;
			videoPlayer.Prepare();
			videoPlayer.Play();
		}
	}

	void UnequippedHeadset(){
		if(firstTimeInMenu){
			videoPlayer.Stop();
		}
	}

	void OnVideoFinished(VideoPlayer vp){
		Debug.Log("Video Finished");
		if(firstTimeInMenu){
			videoPlayer.loopPointReached -= OnVideoFinished;
			firstTimeInMenu = false;
			afkManager.StopAll();
			afkManager.StartTheExperience();			
		}
	}

	private void PerformReset(){
		firstTimeInMenu = true;
		videoPlayer.clip = clip;
		videoPlayer.Prepare();
	}
}
