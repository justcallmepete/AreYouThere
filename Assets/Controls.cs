using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace  ControllerSelection
{
public class Controls : MonoBehaviour
{
    [Header("(Optional) Tracking space")]
    [Tooltip(
    "Tracking space of the OVRCameraRig.\nIf tracking space is not set, the scene will be searched.\nThis search is expensive.")]
    public Transform trackingSpace = null;

    [Header("Selection")]
    [Tooltip("Primary selection button")]
    public OVRInput.Button primaryButton = OVRInput.Button.PrimaryIndexTrigger;

    [Tooltip("Secondary selection button")]
    public OVRInput.Button secondaryButton = OVRInput.Button.PrimaryTouchpad;

    [Tooltip("Layers to exclude from raycast")]
    public LayerMask excludeLayers;

    [Tooltip("Maximum raycast distance")] public float raycastDistance = 500;

    [HideInInspector] public OVRInput.Controller activeController = OVRInput.Controller.None;
    [SerializeField]
    private VideoPlayer videoPlayer;


    private AudioSource audioSource;
    private bool isMounted;

    void Awake()
    {
        if (trackingSpace == null)
        {
            Debug.LogWarning("OVRRawRaycaster did not have a tracking space set. Looking for one");
            trackingSpace = OVRInputHelpers.FindTrackingSpace();
        }
        audioSource = GetComponent<AudioSource>();
            videoPlayer.Prepare();
    }

    void OnEnable()
    {
        OVRManager.HMDUnmounted += StopVideo;
        OVRManager.HMDUnmounted += setUnMounted;
        OVRManager.HMDMounted += SetMounted;
        videoPlayer.loopPointReached += OnMovieFinished;
    }

    void OnDisable()
    {
        OVRManager.HMDUnmounted -= StopVideo;
        OVRManager.HMDUnmounted -= setUnMounted;
        OVRManager.HMDMounted -= SetMounted;
        videoPlayer.loopPointReached -= OnMovieFinished;
        }

    void Update()
    {
        ControllerInput();
    }

    void SetMounted()
    {
        isMounted = true;
    }

    void setUnMounted()
    {
        isMounted = false;
    }

    void StopVideo()
    {
        //if (videoPlayer.isPlaying)
        //{
        //    videoPlayer.Stop();
        //}
        ////ToDO: show Intro image
        //fade.FadeIn();
        StartCoroutine(ResetApplication());
    }

    void StartVideo()
    {
        audioSource.Stop();
        if (!videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }

    private IEnumerator ResetApplication()
    {
        yield return new WaitForSecondsRealtime(3f);
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
        audioSource.Play();
    }

    void OnMovieFinished(VideoPlayer player)
    {
        StopVideo();
    }

        private void ControllerInput()
    {
        activeController =
            OVRInputHelpers.GetControllerForButton(OVRInput.Button.PrimaryIndexTrigger, activeController);

        if (OVRInput.GetDown(primaryButton, activeController) || OVRInput.Get(secondaryButton, activeController))
        {
            if(isMounted)
                StartVideo();
        }

#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.A))
        {
            StartVideo();
        }
#endif
        }
    }

}