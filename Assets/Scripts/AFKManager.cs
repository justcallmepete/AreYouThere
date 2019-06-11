using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControllerSelection;
using UnityEngine.Video;

public class AFKManager : MonoBehaviour {

	public delegate void videoChange(VideoClip clip);
	public static videoChange OnVideoChange;

	[SerializeField] float waitTime;
	public VideoPlayer videoPlayer;	

	private void OnEnable() {
		OVRManager.HMDUnmounted += OnTakeOff;
		OVRManager.HMDMounted += HeadsetPutOn;
		OnVideoChange += SetVideo;
	}

	private void OnDisable() {
		OnVideoChange -= SetVideo;
	}

	private void SetVideo(VideoClip clip){
		videoPlayer.clip = clip;
		videoPlayer.Prepare();
		videoPlayer.Play();
	}

	private void OnTakeOff(){
		// after 7 seconds reset 

	}

	private IEnumerator ResetProgress(){
		yield return new WaitForSecondsRealtime(waitTime);
		// reset to selection screen
	}

	private void HeadsetPutOn(){

	}
}
