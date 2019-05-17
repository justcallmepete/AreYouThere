using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : MonoBehaviour {
	public string sceneName;

	private void Start() {
		StartCoroutine(loadRoutine());
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
}
