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

	public void LoadOtherSceneSingleton(string name){
		Debug.Log("Loading scene: "+name);
		SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
	}

	public void UnloadOtherSceneSingleton(string name){
		Debug.Log("Unloading scene: "+name);
		SceneManager.UnloadSceneAsync(name);
	}
}
