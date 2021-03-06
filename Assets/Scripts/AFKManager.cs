﻿using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class AFKManager : MonoBehaviour {

	public delegate void VideoExperience();
	public VideoExperience StartTheExperience;
	public static event VideoExperience StoryStarted;
	public static event VideoExperience StoryStopped;
    public GameObject exitButton;
	public delegate void VideoPlayerPreparationDone();
	public VideoPlayerPreparationDone OnPreparationDone;
	bool isprepared = false;

	public delegate void ResetExperience();
	public static event ResetExperience OnReset;

	public bool isVideoShow;

	private int currentPhotoIndex = 0;
	private PhotoScriptable photoObj;

	[SerializeField] float waitTime;
	public VideoPlayer videoPlayer;	
	public ProgressManager progressManager;
	private bool resetting;

	private bool firstTimeInMenu = true;
	
	public Material skybox, photoSkybox, circle, photocircle;
	public GameObject holder;
    public VideoClip videoClipIntro;

    public RenderTexture videoTexture;

	IEnumerator cycleRoutine = null;
	IEnumerator HardResetRoutine = null;
	IEnumerator fadeRoutine;

	public float photoCycleTime = 10f;

	public float fadeTime = 2f;
    public float currentAlpha { get; private set; }

	void Awake() {
		photocircle.SetColor("_Color", new Color(0,0,0,0));
		RenderSettings.skybox = skybox;
		skybox.SetFloat("_Exposure", 1);
		skybox.SetTexture("_Tex", videoTexture);
	}

	void Start(){
	//	HardResetRoutine = HardResetApplication();
	}

	void OnEnable() {
		StartTheExperience += DoneWithIntro;
		OnReset += PerformReset;
	}
	
	private void DoneWithIntro() {
	OVRManager.HMDMounted += HeadsetPutOn;
	OVRManager.HMDUnmounted += HeadsetTakenOff;
	videoPlayer.loopPointReached += OnVideoDone;
	progressManager.startding();
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
	videoPlayer.loopPointReached -= OnVideoDone;
	}

	public void SetVideo(VideoClip clip){
		RenderSettings.skybox = skybox;
		isVideoShow = true;
		videoPlayer.clip = clip;
		videoPlayer.Prepare();
		StartCoroutine(StartExperience());
	}

	public void SetPictureShow(PhotoScriptable obj){
			isVideoShow = false;
		StartCoroutine(StartPhotoExperience(obj));
		//StartCoroutine(StartPhotoExperience(obj));
	}

	public void OnVideoDone(VideoPlayer vp){
		progressManager.ResetProgress(vp.clip.ToString());
		StopAll();
	}
	
	public void StopAll(){
		StopAllCoroutines();
      //  exitButton.SetActive(false);
        StartCoroutine(StopExperience());
	}

	private IEnumerator StartPhotoExperience(PhotoScriptable obj){
	yield return FadeObjects(0, 1);
	holder.SetActive(false);
	RenderSettings.skybox = photoSkybox;
	RenderSettings.skybox = obj.PhotoList[0];
	//RenderSettings.skybox.SetColor("_Tint", new Color(0,0,0,0));
//	exitButton.SetActive(true);
	StoryStarted.Invoke(); 
	yield return FadePhotoBubble(1,0);
//	StartCoroutine(CycleThroughPhotoList(obj));
		cycleRoutine = CycleThroughPhotoList(obj);
		StartCoroutine(cycleRoutine);
	}

	private IEnumerator StartExperience(){
		yield return StartCoroutine(FadeObjects(0, 1));
		holder.SetActive(false);
		videoPlayer.Prepare();
		//yield return new WaitUntil(isPrepared);
		isprepared = false;
		videoPlayer.Play();
				StoryStarted.Invoke(); 
		StartCoroutine(Fade(0,1));
     //   exitButton.SetActive(true);

    }

    private IEnumerator StopExperience(){
			StoryStopped.Invoke();

			if(cycleRoutine!=null){
				Debug.Log("Stopping coroutine");
				StopCoroutine(cycleRoutine);
				cycleRoutine = null;
			}

		if(videoPlayer.isPlaying){
			videoPlayer.Stop();
		}
		yield return StartCoroutine(Fade(1,0));
		skybox.SetTexture("_Tex", videoTexture);
		circle.SetColor("_Color", new Color(circle.color.r, circle.color.g, circle.color.b, 255));
		holder.SetActive(true);
	//	exitButton.SetActive(false);
		yield return StartCoroutine(FadeObjects(1, 0));	
	}


	private IEnumerator HardResetApplication(){
		Debug.Log("resetting everything");
		//resetting = true;
		// when taking off the headset wait for (amount_of_seconds)
		yield return new WaitForSecondsRealtime(waitTime);
		PerformReset();
		//OnReset.Invoke();
		//videoPlayer.Stop();
	//	skybox.SetFloat("_Exposure", 1);
		//circle.SetColor("_Color", new Color(circle.color.r, circle.color.g, circle.color.b, 255));
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

		private IEnumerator FadePhotoBubble(float startAlpha, float endAlpha){
			float elapsedTime = 0.0f;
			Color col1 = photocircle.color;
		while (elapsedTime < fadeTime)
		{
			elapsedTime += Time.deltaTime;
			float tempVal= Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / fadeTime));
			col1.a = tempVal;
            photocircle.SetColor("_Color", col1);
			yield return null;
		}
	}

	private IEnumerator CycleThroughPhotoList(PhotoScriptable obj, int cycleIndex = 0){
		currentPhotoIndex = cycleIndex;
		photoObj = obj;
		do{
		currentPhotoIndex += 1;
		Debug.Log("next picture");
		yield return new WaitForSecondsRealtime(photoCycleTime);
		

		if(currentPhotoIndex > photoObj.PhotoList.Count -1){
		currentPhotoIndex = 0;
		}
		yield return  StartCoroutine(FadePhotoBubble(0,1));
		RenderSettings.skybox = obj.PhotoList[currentPhotoIndex];
		yield return StartCoroutine(FadePhotoBubble(1,0));
		} while (true);
	}

	private void HeadsetPutOn(){
    
	if(firstTimeInMenu){
            videoPlayer.clip = videoClipIntro;
		videoPlayer.Play();
	}
	if(isVideoShow){
		videoPlayer.Play();
	}
	//if(resetting){
	Debug.Log("Stopped the reset");
	if(HardResetRoutine != null){
	StopCoroutine(HardResetRoutine);
	HardResetRoutine = null;
	}
	resetting = false;
	//	}
	}
	
	private void Update() {
		if(Input.GetKeyDown(KeyCode.T)){
			HeadsetTakenOff();
			//StartCoroutine(HardResetRoutine);
		}
	}

	private void HeadsetTakenOff(){
		if(videoPlayer.isPlaying){
		videoPlayer.Pause();
		}
		resetting = true;
		HardResetRoutine = HardResetApplication();
		StartCoroutine(HardResetRoutine);
	}

	private void PerformReset(){
						StoryStopped.Invoke();
		resetting = false;	
		videoPlayer.Stop();
		StopAllCoroutines();
		holder.SetActive(false);
		circle.SetColor("_Color", new Color(circle.color.r, circle.color.g, circle.color.b, 0));
		RenderSettings.skybox = skybox;
		skybox.SetFloat("_Exposure", 1);
		skybox.SetTexture("_Tex", videoTexture);
		videoTexture.Release();
		firstTimeInMenu = true;
				//resetting = false;
				isVideoShow = false;

		//HeadsetPutOn();
	}

	public void CycleToNextPicture(){
		// stop timer coroutine,
		// cycle to next picture (if last, go to first)
		// start timer coroutine
		StopAllCoroutines();
		StartCoroutine(ManuallyCycleThroughSlideshow());
	}

	private IEnumerator ManuallyCycleThroughSlideshow(){
		if(photoObj == null) yield return null;
		yield return  StartCoroutine(FadePhotoBubble(0,1));
		if(currentPhotoIndex+1 > photoObj.PhotoList.Count-1){
			currentPhotoIndex = 0;
		} else {
			currentPhotoIndex++;
		}
		RenderSettings.skybox = photoObj.PhotoList[currentPhotoIndex];
		yield return StartCoroutine(FadePhotoBubble(1,0));
		cycleRoutine = CycleThroughPhotoList(photoObj, currentPhotoIndex);
		StartCoroutine(cycleRoutine);
	}
}
