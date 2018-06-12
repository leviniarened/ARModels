using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Vuforia;
using Image = UnityEngine.UI.Image;

public enum Markers
{
    Astronaut=0, Drone, Fissure
}

public class ARItem : MonoBehaviour
{
    [SerializeField]
    private Text filename;

    [SerializeField]
    private Image marker;

    private string mdlfilename;

    private int index = 0;

    [SerializeField]
    private MarkerSpriteColllection spriteCollection;

    [SerializeField]
    private GameObject markerSelectionWindow;

    public void SetUp (string fn, int markerindex)
	{
	    filename.text = fn;
	    mdlfilename = fn;
	    marker.sprite = spriteCollection.MarkersImages[markerindex];
	    index = markerindex;
	}

    public void UpdateConfig(int index)
    {
        IIS_Core.UpdateCfg(mdlfilename, index, () =>
        {
            marker.sprite = spriteCollection.MarkersImages[index];
            this.index = index;
            Debug.Log("Successfully updated");
        });
    }

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            string filePath = Application.persistentDataPath + '/' + mdlfilename;
            if (!File.Exists(filePath))
            {
                IIS_Core.DownloadModel(mdlfilename, s =>
                {
                    if (!File.Exists(filePath))
                    {
                        using (FileStream fs = File.Create(filePath))
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes(s);
                            fs.Write(info, 0, info.Length);
                        }
                    }
                    var obj = OBJLoader.LoadOBJFile(filePath);
                    SetupObject(obj);
                });
            }
            else
            {
                var obj = OBJLoader.LoadOBJFile(filePath);
                SetupObject(obj);
            }
        });
        markerSelectionWindow.SetActive(false);
    }

    void SetupObject(GameObject g)
    {
        g.transform.localScale = Vector3.one * 0.01f;
        /*
        var mtl = Resources.Load<Material>("Default");
        foreach (var c in g.GetComponentsInChildren<MeshRenderer>())
        {
            c.material = mtl;
        }*/

        var imagetarget = FindObjectsOfType<ImageTargetBehaviour>();
        var target = imagetarget.First(s => s.ImageTarget.Name == Enum.GetName(typeof(Markers), index));
        g.transform.parent = target.transform;
        g.transform.position = Vector3.zero;
    }


}
