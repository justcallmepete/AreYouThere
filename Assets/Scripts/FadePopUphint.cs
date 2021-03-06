﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePopUphint : MonoBehaviour
{

    Coroutine co;

     // Update is called once per frame
     void Update()
    {
        if (Input.GetKey(KeyCode.Space)) co = StartCoroutine(FadeTo(0.0f, 1f));
        
    }
        

    public IEnumerator FadeTo(float aValue, float aTime)
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 1f;
        yield return new WaitForSecondsRealtime(1f);
        float alpha = gameObject.GetComponent<CanvasGroup>().alpha;
            for (float t = 0.0f; t < 2f; t += Time.deltaTime / aTime)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
                gameObject.GetComponent<CanvasGroup>().alpha = newColor.a;
                yield return null;
                
        }

    }

    
}
