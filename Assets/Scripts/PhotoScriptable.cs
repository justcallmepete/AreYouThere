using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PhotoScriptable", menuName = "Inventory/List")]
public class PhotoScriptable : ScriptableObject {

public List<Material> PhotoList = new List<Material>();
}
