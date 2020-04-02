using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleKeeper : MonoBehaviour
{
    GameObject[] keyholes;

    [SerializeField] private float alwaysScale = 0.5f;
    [SerializeField] private string toScaleTag = "Testing";

    // Start is called before the first frame update
    void Start()
    {
        keyholes = GameObject.FindGameObjectsWithTag(toScaleTag);

        foreach (GameObject tosize in keyholes)
        {
            Transform child = tosize.transform;
            Transform parent = child.parent;
            child.parent = null;
            Vector3 parentscale = parent.localScale;
            float lowestScale = Mathf.Min(parentscale.x, parentscale.y, parentscale.z);
            child.localScale = new Vector3(lowestScale * alwaysScale, lowestScale * alwaysScale, lowestScale * alwaysScale);
            child.SetParent(parent);
        }
    }
}
