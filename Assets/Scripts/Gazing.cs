﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Gazing : MonoBehaviour
{

    Canvas can;
    public float timer = 4f;
    // Use this for initialization
    int scalingFramesLeft = 0;
    bool scale = true;
    void Start()
    {
        timer = 4f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        int layerMask = LayerMask.GetMask("WorldUI");
        RaycastHit hit;
       
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            //if (scale) { 
            //      scalingFramesLeft = 10;
            //      scale = false;
            //    }


            //if (scalingFramesLeft > 0)
            //{
            //    hit.transform.localScale = Vector3.Lerp(hit.transform.localScale,hit.transform.localScale * 1.2f,Time.deltaTime * 10);
            //    scalingFramesLeft--;
            //}
            timer -= Time.deltaTime;
            gameObject.GetComponentInChildren<Image>().fillAmount += Time.deltaTime / 4;

            if (timer <= 0)
            {
                GazeTimer(hit);
            }
        }
        else
        {
            timer = 4f;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            gameObject.GetComponentInChildren<Image>().fillAmount = 0;
        }


    }
    void GazeTimer(RaycastHit hit)
    {
        hit.transform.gameObject.GetComponentInChildren<Button>().onClick.Invoke();
    }
}
