using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControllerSelection;
using UnityEngine.Video;

public class AFKManager : MonoBehaviour {

	public delegate void VideoExperience();
	public VideoExperience StartTheExperience;
	public VideoExperience StopTheExperience;

	public delegate void VideoPlayerPreparationDone();
	public VideoPlayerPreparationDone OnPreparationDone;
	bool isprepared = false;

	[SerializeField] float waitTime;
	public VideoPlayer videoPlayer;	
	private bool resetting;

	private bool firstTimeInMenu = true;
	
	public Material skybox, circle;
	public GameObject holder;

	public float fadeTime = 2f;
    public float currentAlpha { get; private set; }

	void Awake() {
		skybox.SetFloat("_Exposure", 1);
	}

	void OnEnable() {
		StartTheExperience += DoneWithIntro;
	}
	
	private void DoneWithIntro() {
	OVRManager.HMDMounted += HeadsetPutOn;
	OVRManager.HMDUnmounted += HeadsetTakenOff;
	StopTheExperience += StopAll;
	videoPlayer.loopPointReached += OnVideoDone;
	}

		public bool isPrepared(){
		if (videoPlayer.isPrepared){
			return true;
		} 
		return false;
	}

	private void OnDisable() {
	OVRManager.HMDMounted -= HeadsetPutOn;
	OVRManager.HMDUnmounted -= HeadsetTakenOff;
	StopTheExperience -= StopAll;
	videoPlayer.loopPointReached -= OnVideoDone;
	}

	public void SetVideo(VideoClip clip){
		videoPlayer.clip = clip;
		videoPlayer.Prepare();
		StartCoroutine(StartExperience());
	}

	public void OnVideoDone(VideoPlayer vp){
		StopAll();
	}
	
	public void StopAll(){
		StartCoroutine(StopExperience());
	}

	private IEnumerator StartExperience(){
		yield return StartCoroutine(FadeObjects(0, 1));
		holder.SetActive(false);
		videoPlayer.Prepare();
		//yield return new WaitUntil(isPrepared);
		isprepared = false;
		videoPlayer.Play(); 
		StartCoroutine(Fade(0,1));
	}

	private IEnumerator StopExperience(){
		if(videoPlayer.isPlaying){
			videoPlayer.Stop();
		}
		yield return StartCoroutine(Fade(1,0));
		circle.SetColor("_Color", new Color(circle.color.r, circle.color.g, circle.color.b, 255));
		holder.SetActive(true);
		yield return StartCoroutine(FadeObjects(1, 0));	
	}


	private IEnumerator HardResetApplication(){
		resetting = true;
		// when taking off the headset wait for (amount_of_seconds)
		yield return new WaitForSecondsRealtime(waitTime);
		videoPlayer.Stop();
		skybox.SetFloat("_Exposure", 1);
		circle.SetColor("_Color", new Color(circle.color.r, circle.color.g, circle.color.b, 255));
		resetting = false;
	}

	private IEnumerator SoftResetApplication(){
		videoPlayer.Stop();
		// fade out the skybox
		yield return StartCoroutine(Fade(1,0));
		// fade in the select screen
		holder.SetActive(true);
		yield return StartCoroutine(FadeObjects(0,1));
	}

	private IEnumerator Fade(float startAlpha, float endAlpha)
	{
		float elapsedTime = 0.0f;
		while (elapsedTime < fadeTime)
		{
			elapsedTime += Time.deltaTime;
			float tempVal= Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / fadeTime));
            skybox.SetFloat("_Exposure", tempVal); 
			yield return null;
		}
	}

	private IEnumerator FadeObjects(float startAlpha, float endAlpha){
			float elapsedTime = 0.0f;
			Color col1 = circle.color;
		while (elapsedTime < fadeTime)
		{
			elapsedTime += Time.deltaTime;
			float tempVal= Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / fadeTime));
			col1.a = tempVal;
            circle.SetColor("_Color", col1);
			yield return null;
		}
	}

	private void HeadsetPutOn(){
	if(firstTimeInMenu){
		videoPlayer.Play();
	}
	if(resetting){
	Debug.Log("Stopped the reset");
	StopAllCoroutines();
	resetting = false;
		}
	}

	private void HeadsetTakenOff(){
		StartCoroutine(HardResetApplication());
	}
}
