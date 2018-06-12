using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Markers/List", order = 1)]
public class MarkerSpriteColllection : ScriptableObject
{
    public List<Sprite> MarkersImages = new List<Sprite>();
}
