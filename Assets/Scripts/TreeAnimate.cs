using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAnimate : MonoBehaviour
{
    public float animLevel = 0.0F;
    public float minRotation = -55F;
    public float maxrotation = -45F;

    public List<Transform> quadList = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(MeshFilter el in GetComponentsInChildren(typeof(MeshFilter)))
        {
            GameObject toAdd = el.gameObject;
            if (toAdd.name == "Trunk" || toAdd.name == "Ornament")
                continue;
            quadList.Add(toAdd.GetComponent<Transform>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion newRotation = Quaternion.Euler(minRotation + (maxrotation - minRotation) * animLevel, 0, 0);
        foreach(Transform el in quadList)
        {
            el.localRotation = newRotation;
        }
    }
}
