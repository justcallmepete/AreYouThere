using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class HotSpot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler  {

    public string scene; // :)

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
    public void GobBack()
    {
        //singelton.current.LoadScene(scene);
    }

  
}
