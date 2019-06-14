using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

namespace ControllerSelection
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
        public OVRInput.Button secondaryButton = OVRInput.Button.Back;

        [Tooltip("Layers to exclude from raycast")]
        public LayerMask excludeLayers;

        [Tooltip("Maximum raycast distance")] public float raycastDistance = 500;

        [HideInInspector] public OVRInput.Controller activeController = OVRInput.Controller.None;
        public bool isInExperience;
        public AFKManager afkManager;
        private Button currentButton;
        public GameObject controller;
        public FadePopUphint popUp;

        void Awake()
        {
            if (trackingSpace == null)
                Debug.LogWarning("OVRRawRaycaster did not have a tracking space set. Looking for one");
            trackingSpace = OVRInputHelpers.FindTrackingSpace();
        }

        void OnEnable()
        {
            AFKManager.StoryStarted += SetExperienceBoolTrue;
            AFKManager.StoryStopped += SetExperienceBoolFalse;
        }

        private void OnDisable()
        {

        }

        public void SetExperienceBoolTrue()
        {
            isInExperience = true;
        }

        public void SetExperienceBoolFalse()
        {
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

            if (OVRInput.GetDown(primaryButton, activeController))
            {
                if (currentButton)
                {
                    currentButton.onClick.Invoke();
                }
            }

            if (OVRInput.GetDown(secondaryButton, activeController))
            {
                if (isInExperience)
                {
                    afkManager.StopAll();
                }
                else
                {
                     popUp.StartCoroutine(popUp.FadeTo(0,1));
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("reeeeee");
                if (isInExperience)
                {
                    afkManager.StopAll();
                }
                else
                {
                    popUp.StartCoroutine(popUp.FadeTo(0,1));
                }
            }
        }
            private void RaycastFromController()
            {

                activeController =
                OVRInputHelpers.GetControllerForButton(OVRInput.Button.PrimaryIndexTrigger, activeController);
                Ray pointer = OVRInputHelpers.GetSelectionRay(activeController, trackingSpace);
                RaycastHit hit; // Was anything hit?
                                //Debug.DrawRay(controller.transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white); 
            if (Physics.Raycast(pointer, out hit, Mathf.Infinity, LayerMask.GetMask("WorldUI")))
            {
                Debug.Log("RayHit " + hit.transform.gameObject.ToString());
                controller.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.green);
                if (hit.transform.GetComponentInChildren<Button>() && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                {
                    {
                        Button btn = hit.transform.GetComponentInChildren<Button>();
                        btn.onClick.Invoke();
                    }
                }
            }
             else     controller.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.blue);

            }

            }
        }

