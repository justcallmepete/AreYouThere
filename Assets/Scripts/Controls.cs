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
    private bool isInExperience;
    public AFKManager afkManager;
    private Button currentButton;

    void Awake()
    {
        if (trackingSpace == null)
        {
            Debug.LogWarning("OVRRawRaycaster did not have a tracking space set. Looking for one");
            trackingSpace = OVRInputHelpers.FindTrackingSpace();
        }
    }

    void OnEnable() {
        AFKManager.StoryStarted += SetExperienceBoolTrue;
        AFKManager.StoryStopped += SetExperienceBoolFalse;
    }

    private void OnDisable() {
        
    }

    private void SetExperienceBoolTrue(){
            isInExperience = true;
    }

    private void SetExperienceBoolFalse(){
            isInExperience = false;
    }

    void Update()
    {
      //  if(isInExperience)
        ControllerInput();
        RaycastFromController();
    }

    private void ControllerInput()
    {
        activeController = OVRInputHelpers.GetControllerForButton(OVRInput.Button.PrimaryIndexTrigger, activeController);
       
        if (OVRInput.Get(primaryButton, activeController)){
            if (currentButton){
                currentButton.onClick.Invoke();
            }
        }

        if (OVRInput.Get(secondaryButton, activeController))
        {
                if(isInExperience){
            afkManager.StopAll();
                } else {
                    // show fadeUI that tells you not to click the button in menu
                }
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("reeeeee");
            if(isInExperience){
            afkManager.StopAll();
                } else {
                    // show fadeUI that tells you not to click the button in menu
                }
        }
    }

    private void RaycastFromController(){
        activeController =
        OVRInputHelpers.GetControllerForButton(OVRInput.Button.PrimaryIndexTrigger, activeController);
        Ray pointer = OVRInputHelpers.GetSelectionRay(activeController, trackingSpace);
        RaycastHit hit; // Was anything hit?

            if (Physics.Raycast(pointer, out hit, raycastDistance, 9))
        {
                gameObject.GetComponentInChildren<LineRenderer>().material.color = Color.green;
        // indicator.UpdateComponent(hit.transform.gameObject, hit);
        if (hit.transform.GetComponent<Button>())
        {
        Button btn = hit.transform.GetComponent<Button>();
        }
    }
        else gameObject.GetComponentInChildren<LineRenderer>().material.color = Color.red;
        }

}
}