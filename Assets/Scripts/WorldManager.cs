using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour {

	 private static WorldManager worldManager;

	 public delegate void ChangeScene(string sceneName);
	 public ChangeScene onChangeScene;

	 public static WorldManager current
    {
        get
        {
            if (!worldManager)
            {
                worldManager = FindObjectOfType(typeof(WorldManager)) as WorldManager;
/* 
                if (!gameSettings)
                {
                    // Debug.LogError("No QuestManager script found in the scene, add it to an empty gameobject");
                }
                else
                {
                    gameSettings.Init();
                } */
            }
            return worldManager;
        }
    }

	public string holdText;
	public string sceneToLoad = "";

	private void OnEnable() {
		//SceneManager.sceneLoaded += LoadOtherScene;
		onChangeScene += LoadOtherSceneSingleton;
	}

	private void OnDisable() {
			//SceneManager.sceneLoaded -= LoadOtherScene;
			onChangeScene -= LoadOtherSceneSingleton;
	}

	private void LoadOtherSceneSingleton(string name){
		SceneManager.UnloadSceneAsync("Start");
		SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
		//ToDo unload the loadscreen
	}

	private void LoadOtherScene(Scene scene, LoadSceneMode mode){
		Debug.Log("Scene is : "+scene.name);
		if(sceneToLoad != ""){
			SceneManager.UnloadSceneAsync("Start");
			SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
		}
	}
}
