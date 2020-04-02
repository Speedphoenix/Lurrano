using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorType = ColorController.ColorType;

public class TestingScript : MonoBehaviour
{
    GameObject testingWall;

    // Start is called before the first frame update
    void Start()
    {
        testingWall = GameObject.FindGameObjectWithTag("Testing");

        // StartCoroutine("sendColors");
    }
    
    private void OnEnable()
    {
        ColorController.onColChange += onColorChange;
    }

    private void OnDisable()
    {
        ColorController.onColChange -= onColorChange;
    }

    private void onColorChange(ColorType newCol)
    {
        testingWall.GetComponent<Renderer>().material.SetColor("_Color", ColorController.getColorFromType(newCol));
    }

    /*
    IEnumerator sendColors()
    {
        
        ColorController instance = ColorController.instance;
        
        yield return new WaitForSeconds(0.5f);
        instance.enqueueNewCol(Blue);
        yield return new WaitForSeconds(0.5f);
        instance.enqueueNewCol(Red);
        yield return new WaitForSeconds(0.3f);
        instance.enqueueNewCol(Yellow);
        yield return new WaitForSeconds(2f);
        instance.IsStack = true;
        yield return new WaitForSeconds(0.3f);
        instance.enqueueNewCol(Yellow);
        yield return new WaitForSeconds(0.5f);
        instance.enqueueNewCol(Blue);
        yield return new WaitForSeconds(2f);
        instance.toggleHourglassType();
        yield return new WaitForSeconds(2f);
        instance.toggleHourglassType();
        yield return new WaitForSeconds(9f);
        instance.setMainCol(Green);
        
    }
    */
}
