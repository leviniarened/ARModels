using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ARModel
{
    public string Model;
    public int Marker;
}

[System.Serializable]
public class Save
{
    public List<ARModel> Model;
}
