using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ProgressManager : MonoBehaviour {
	public VideoPlayer videoPlayer;
	//AFKManager AFKManager;
	// list with stories
	// string nameOf Person, 

	public class VideoFormat{
		public VideoFormat(double p, double d){
			progress = p;
			duration = d;
		}
		//string name;
		public double progress;
		public double duration;
	}

	Dictionary<string, VideoFormat> videoDictionary = new Dictionary<string, VideoFormat>();

	public void startding(){
		Debug.Log("staartding aangeroepen");
		AFKManager.StoryStarted += SetCurrentProgress;
		AFKManager.StoryStopped += SaveCurrentProgress;
	}

	private void SetCurrentProgress(){
		VideoFormat vid = null;
		if(videoDictionary.TryGetValue(videoPlayer.clip.ToString(), out vid)){
			videoPlayer.time = vid.progress;
			Debug.Log("player is at: " + vid.progress + " seconds again");
		}
	}

	private void SaveCurrentProgress(){
		VideoFormat vid = null;
		if (videoDictionary.TryGetValue(videoPlayer.clip.ToString(), out vid)){
			vid.progress = videoPlayer.time;
			Debug.Log("Time saved at: " + videoPlayer.time);
		} else {
			Debug.Log("player is at: " + videoPlayer.time + " seconds");
			if (videoPlayer.isPlaying && videoPlayer.time < videoPlayer.clip.length)
			videoDictionary.Add(videoPlayer.clip.ToString(), new VideoFormat(videoPlayer.time , videoPlayer.clip.length));
			}
		}

		public void ResetProgress(string clipName){
			videoDictionary.Remove(videoPlayer.clip.ToString());
		}

	private void ResetAllProgress(){
		videoDictionary.Clear();
	}
}
