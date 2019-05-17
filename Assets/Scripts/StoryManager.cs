using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour {

	public string storyText;

		void OnEnable() {
		SceneManager.sceneLoaded += SetText;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= SetText;
	}

	public void SetText(Scene scene, LoadSceneMode mode){
		if(WorldManager.current){
			storyText = WorldManager.current.holdText;
		}
	}
}
