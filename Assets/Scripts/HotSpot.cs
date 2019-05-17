using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class HotSpot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler  {

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void LoadAScene(string name){
        if(WorldManager.current)
        WorldManager.current.LoadOtherSceneSingleton(name);
    }

    public void UnLoadAScene(string name)
    {
      //  Debug.Log("unloading");
        if(WorldManager.current){
        Debug.Log("found pitbull");
        WorldManager.current.UnloadOtherSceneSingleton(name);
        }
    }
}
