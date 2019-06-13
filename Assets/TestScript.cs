using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ControllerSelection;
using UnityEngine.Video;

public class TestScript : MonoBehaviour {

	AFKManager manager;

	public float timer;
	public VideoClip clip;
	
	private void Awake() {
		manager = GetComponent<AFKManager>();
	}

	void Start(){
//		manager.StartTheExperience(clip);
	}
}
