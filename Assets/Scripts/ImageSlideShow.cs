using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlideShow : MonoBehaviour {
    public   List<Sprite> slideshowImages = new List<Sprite>();
    private int imageCount;
	// Use this for initialization
	void Start () {
        GetComponentInChildren<Image>().overrideSprite = slideshowImages[imageCount];
        GetComponentInChildren<Image>().preserveAspect = true;
    }

    // Update is called once per frame
    public void ImageSlider(bool next)
    {
        if (next)
        {
            if(imageCount == slideshowImages.Count - 1)
            {
                imageCount = 0;
            }
            else imageCount++;
        }
        if (!next)
        {
            if(imageCount == 0)
            {
                imageCount = slideshowImages.Count - 1;
            }

            else imageCount--;
        }

        GetComponentInChildren<Image>().overrideSprite = slideshowImages[imageCount];

    }
}

    