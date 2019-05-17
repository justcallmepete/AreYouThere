using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Events;

public class TestManager : MonoBehaviour {
	public string sceneName;
	public UnityEvent newEvent;
	public Material matVideo;
	public Material matPhoto;
	public VideoPlayer videoPlayer;

	private void Start() {
	//	StartCoroutine(loadRoutine());
		videoPlayer.loopPointReached += OnVideoFinished;
	}

	IEnumerator loadRoutine(){
		AsyncOperation asyncLoad = 	SceneManager.LoadSceneAsync("LoadScreen", LoadSceneMode.Additive);
		asyncLoad.allowSceneActivation = true;
		asyncLoad.completed += SendMessageToManager;
		yield return null;
	}

	private void SendMessageToManager(AsyncOperation op){
			if(WorldManager.current){
		WorldManager.current.onChangeScene(sceneName);
		op.completed -= SendMessageToManager;
			}
	}

	void OnVideoFinished(VideoPlayer vid){
		RenderSettings.skybox = matPhoto;
	}
}
