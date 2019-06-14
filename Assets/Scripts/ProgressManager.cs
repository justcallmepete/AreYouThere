using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ProgressManager : MonoBehaviour {
	public VideoPlayer videoPlayer;
	//AFKManager AFKManager;
	// list with stories
	// string nameOf Person, 
	public AFKManager afkManager;

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
		AFKManager.StoryStarted += SetCurrentProgress;
		AFKManager.StoryStopped += SaveCurrentProgress;
	}

	private void SetCurrentProgress(){
		if(!afkManager.isVideoShow) return;
		VideoFormat vid = null;
		if(videoDictionary.TryGetValue(videoPlayer.clip.ToString(), out vid)){
			videoPlayer.time = vid.progress;
		}
	}

	private void SaveCurrentProgress(){
		if(!afkManager.isVideoShow) return;
		VideoFormat vid = null;
		if (videoDictionary.TryGetValue(videoPlayer.clip.ToString(), out vid)){
			if (videoPlayer.time < (videoPlayer.clip.length * .99f))
			vid.progress = videoPlayer.time;
		} else {
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
