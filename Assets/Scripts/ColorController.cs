using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    public enum ColorType
    {
        NoColor, Red, Yellow, Blue, Green
    }

    [System.Serializable]
    public struct NamedMaterial {
        public ColorType colorName;
        public Material colorMaterial;
    }

    public struct TimedColor {
        public ColorType colorName;
        public float remainingTime;

        public TimedColor(ColorType name, float duration)
        {
            colorName = name;
            remainingTime = duration;
        }
    }

    [System.Serializable]
    public struct SomeColStruct {
        public ColorType colortype;
        public Color color;
    }

    [SerializeField] private bool isStack = false;

    [SerializeField] private float defaultColorDuration = 4;

    // this must be in seconds
    [SerializeField] public float MAXHOURGLASSDURATION = 12;

    // this is to be seen by the inspector, and will be converted to a dictionary at Start
    [SerializeField] private NamedMaterial[] neonMaterialList = default;

    [SerializeField] private NamedMaterial[] wallMaterialList = default;
    [SerializeField] private GameObject fullLaby = null;
    private List<Renderer> wallList = new List<Renderer>();
    private Dictionary<ColorType, Material> wallMaterialDictionary = new Dictionary<ColorType, Material>();


    public delegate void ColorChange(ColorType newCol);
    public static ColorChange onColChange;

    public delegate void HourglassTypeChange(bool newIsStack);
    public static HourglassTypeChange onHourglassTypeChange;

    public delegate void GlobalColorChange(ColorType newCol);
    public static GlobalColorChange onGlobalColorChange;

    public static readonly string coloredTag = "ColorIndicate";
    public static ColorController instance;
    private Dictionary<ColorType, Material> materialList = new Dictionary<ColorType, Material>();

    private GameObject[] coloredObjects;

    private ColorType currentCol;
    [SerializeField] private ColorType currentGlobalCol = ColorType.NoColor;

    public List<TimedColor> currentQueue = new List<TimedColor>();

    // TODO HERE
    [SerializeField] private SomeColStruct[] colorList = null;

    public static Color getColorFromType(ColorType theCol)
    {
        foreach (SomeColStruct el in instance.colorList)
        {
            if (el.colortype == theCol)
                return el.color;
        }
        foreach (SomeColStruct el in instance.colorList)
        {
            if (el.colortype == ColorType.NoColor)
                return el.color;
        }
        return Color.white;
    }

    public ColorType CurrentCol
    {
        get { return currentCol; }
    }

    public ColorType CurrentGlobalCol
    {
        get { return currentGlobalCol; }
    }

    public List<TimedColor> CurrentQueue
    {
        get { return currentQueue; }
    }

    // gives the index of the color to use in the currentQueue
    private int ActiveColorIndex
    {
        get { return (isStack ? currentQueue.Count - 1 : 0); }
    }

    public bool IsStack
    {
        get { return isStack; }
        set
        {
            if  (isStack != value)
            {
                isStack = value;
                updatecol();
                if (onHourglassTypeChange != null)
                    onHourglassTypeChange(isStack);
            }
        }
    }

    public bool toggleHourglassType()
    {
        IsStack = !isStack;
        return isStack;
    }

    public Material GetMaterialFromType(ColorType what)
    {
        return materialList[what];
    }

    void OnEnable()
    {
        if (instance != null)
            Application.Quit(); // replace this with a proper throw statement
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (NamedMaterial item in neonMaterialList)
            materialList.Add(item.colorName, item.colorMaterial);
        foreach (NamedMaterial item in wallMaterialList)
            wallMaterialDictionary.Add(item.colorName, item.colorMaterial);

        // TODO fetch this ever update...
        coloredObjects = GameObject.FindGameObjectsWithTag(coloredTag);
        changeCols(currentGlobalCol);

        foreach (Renderer child in fullLaby.GetComponentsInChildren<Renderer>())
        {
            GameObject inter = child.gameObject;
            if (inter.tag == coloredTag)
                continue;
            wallList.Add(child);
        }
    }

    void changeCols(ColorType newCol)
    {
        currentCol = newCol;
        foreach (GameObject toColor in coloredObjects)
        {
            toColor.GetComponent<Renderer>().material = materialList[currentCol];
        }
        foreach (Renderer wallToColor in wallList)
        {
            wallToColor.material = wallMaterialDictionary[currentCol];
        }
        if (onColChange != null)
            onColChange(currentCol);
    }

    // this will find what the color should be and uses changeCols to do it
    void updatecol()
    {
        ColorType shouldHave;
        if (currentQueue.Count == 0)
            shouldHave = currentGlobalCol;
        else
            shouldHave = currentQueue[ActiveColorIndex].colorName;
        if (shouldHave != currentCol)
            changeCols(shouldHave);
    }

    public float getQueueDuration(bool takeLast = true)
    {
        float rep = 0;
        for (int i = 0; i < currentQueue.Count; i++)
        {
            if (takeLast || i != (currentQueue.Count - 1))
                rep += currentQueue[i].remainingTime;
        }
        return rep;
    }

    public void enqueueNewCol(ColorType newCol, float duration)
    {
        bool done = false;

        if (currentQueue.Count > 0)
        {
            // this seems to duplicate it
            TimedColor lastCol = currentQueue[currentQueue.Count - 1];
            if (lastCol.colorName == newCol)
            {
                // without taking the last one
                float currQueueLen = getQueueDuration(false);

                if (lastCol.remainingTime <= duration)
                {
                    if (currQueueLen + duration < MAXHOURGLASSDURATION)
                        lastCol.remainingTime = duration;
                    else
                        lastCol.remainingTime = MAXHOURGLASSDURATION - currQueueLen;
                    currentQueue[currentQueue.Count - 1] = lastCol; //is this necessary?
                }

                done = true;
            }
        }
        if (!done)
        {
            float currQueueLen = getQueueDuration();
            bool changeit = isStack || (currentQueue.Count == 0);
            if (MAXHOURGLASSDURATION - currQueueLen >= duration)
                currentQueue.Add(new TimedColor(newCol, duration));
            else
                currentQueue.Add(new TimedColor(newCol, MAXHOURGLASSDURATION - currQueueLen));
            if (changeit)
                changeCols(newCol);
        }
    }

    public void enqueueNewCol(ColorType newCol)
    {
        enqueueNewCol(newCol, defaultColorDuration);
    }

    public void setMainCol(ColorType newCol)
    {
        if (currentGlobalCol == newCol && currentQueue.Count == 0)
            return;
        currentQueue.Clear();
        currentGlobalCol = newCol;
        if (onGlobalColorChange != null)
            onGlobalColorChange(newCol);
        changeCols(newCol);
    }

    // Update is called once per frame
    void Update()
    {
        float timeToReduce = Time.deltaTime;
        bool done = false;
        bool changed = false;

        while (!done && currentQueue.Count != 0)
        {
            TimedColor topCol = currentQueue[ActiveColorIndex];
            if (topCol.remainingTime - timeToReduce > Mathf.Epsilon)
            {
                topCol.remainingTime -= timeToReduce;
                currentQueue[ActiveColorIndex] = topCol;
                done = true;
            }
            else
            {
                float removedTime = topCol.remainingTime;
                currentQueue.RemoveAt(ActiveColorIndex);
                changed = true;
                timeToReduce -= removedTime;
            }
        }
        if (changed)
            updatecol();
    }

    public static string getColorNameFromType(ColorType col)
    {
        switch(col)
        {
            case ColorType.Red:
                return "Red";
            case ColorType.Yellow:
                return "Yellow";
            case ColorType.Blue:
                return "Blue";
            case ColorType.Green:
                return "Green";
            case ColorType.NoColor:
            default:
                return "NoColor";
        }
    }
}
