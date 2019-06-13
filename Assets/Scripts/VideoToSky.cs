using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoToSky : MonoBehaviour {

    [SerializeField]
    Texture staticImage;
    [SerializeField]
    VideoClip newVid;
    [SerializeField]
    RenderTexture newTex;
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        gameObject.GetComponent<VideoPlayer>().loopPointReached += EndReached;

    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = vp.playbackSpeed / 10.0F;
        RenderSettings.skybox.SetTexture("_MainTex", staticImage);
        gameObject.SetActive(false);

    }

    public void Restart(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = 1;
        gameObject.GetComponent<VideoPlayer>().clip = newVid;
        gameObject.GetComponent<VideoPlayer>().Play();

        RenderSettings.skybox.SetTexture("_MainTex", newTex);
    }
}   
