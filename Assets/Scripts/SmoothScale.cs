using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothScale : MonoBehaviour {

    public Vector3 scale;
    int scalingFramesLeft = 0;
     public Vector3 scaleGoal;
     Vector3 reverseScale = new Vector3(-1f,-1f,-1f);

    public GameObject player;
    public bool dirScale;

    void OnValidate () {
        scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        scaleGoal = scale * 1.2f;
        if (player != null) transform.LookAt(player.transform.position);
	}

    private void Update()
    {
        ScaleUp(dirScale);
    }
    public void ScaleUp(bool dir)
    {
        scalingFramesLeft = 10;
        if (scalingFramesLeft > 0 && dir)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scaleGoal, Time.deltaTime * 15);
            scalingFramesLeft--;
        }

        if (scalingFramesLeft > 0 && !dir)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scale , Time.deltaTime * 15);
            scalingFramesLeft--;
        }


    }

  
}
