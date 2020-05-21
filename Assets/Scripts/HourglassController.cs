using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ColorType = ColorController.ColorType;

public class HourglassController : MonoBehaviour
{
    [System.Serializable]
    public struct NamedChunk {
        public ColorType colorType;
        public GameObject chunkObject;
    }

    [SerializeField] private NamedChunk[] hourglassChunkPrefabs = null;
    [SerializeField] private bool indicateHourglassType = true;
    [SerializeField] private GameObject queueIndicator = null;
    [SerializeField] private GameObject stackIndicator = null;
    [SerializeField] private GameObject borderContainer = null;
    private Image[] borders = null;
    [SerializeField] private bool coloredBorders = true;

    private List<NamedChunk> chunkList = new List<NamedChunk>();

    private float hourglassSize;

    private bool isStack;
    private RectTransform mainTransform;
    private float mainWidth;

    NamedChunk createNamedChunk(ColorType color, float duration)
    {
        NamedChunk rep;
        foreach (NamedChunk item in hourglassChunkPrefabs)
        {
            if (item.colorType == color)
            {
                rep.colorType = color;
                rep.chunkObject = Instantiate(item.chunkObject, mainTransform);
                
                RectTransform newTransform = rep.chunkObject.GetComponent<RectTransform>();
                newTransform.sizeDelta = new Vector2(getWidth(duration), newTransform.sizeDelta.y);
                newTransform.SetAsFirstSibling();

                return rep;
            }
        }
        Debug.LogError("Error: no possible chunk for asked color: " + color);
        rep.colorType = ColorType.NoColor;
        rep.chunkObject = null;
        return rep;
    }

    void clearChunkList()
    {
        foreach (NamedChunk item in chunkList)
        {
            Destroy(item.chunkObject);
        }
        chunkList.Clear();
    }

    float getWidth(float duration)
    {
        return mainWidth * (duration / hourglassSize);
    }

    // Start is called before the first frame update
    void Start()
    {
        isStack = ColorController.instance.IsStack;
        hourglassSize = ColorController.instance.MAXHOURGLASSDURATION;
        mainTransform = GetComponent<RectTransform>();
        mainWidth = mainTransform.sizeDelta.x;

        if (indicateHourglassType)
        {
            stackIndicator.SetActive(isStack);
            queueIndicator.SetActive(!isStack);
        }
        else
        {
            stackIndicator.SetActive(false);
            queueIndicator.SetActive(false);
        }

        updateChunks();

        borders = borderContainer.GetComponentsInChildren<Image>();
    }

    void OnEnable()
    {
        ColorController.onHourglassTypeChange += onHourglassTypeChange;
        ColorController.onGlobalColorChange += onGlobalColorChange;
    }
    
    void OnDisable()
    {
        ColorController.onHourglassTypeChange -= onHourglassTypeChange;
        ColorController.onGlobalColorChange -= onGlobalColorChange;
    }

    void onGlobalColorChange(ColorType newCol)
    {
        if (coloredBorders)
        {
            Color inter = ColorController.getColorFromType(newCol);

            foreach (Image image in borders)
            {
                image.color = inter;
            }
        }
    }

    void onHourglassTypeChange(bool newIsStack)
    {
        isStack = newIsStack;
        if (indicateHourglassType)
        {
            stackIndicator.SetActive(isStack);
            queueIndicator.SetActive(!isStack);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // no point in calculating or updating stuff that didn't change
        if (ColorController.instance.IsPaused)
            return;
        updateChunks();
    }

    void updateChunks()
    {
        List<ColorController.TimedColor> controllerList = ColorController.instance.CurrentQueue;
        bool noOrderChange = true;
        float leftOffset;
        int chunkIndex;

        if (controllerList.Count == 0)
        {
            clearChunkList();
            return;
        }

        if (controllerList.Count != chunkList.Count)
            noOrderChange = false;
        else
        {
            chunkIndex = 0;
            foreach (ColorController.TimedColor item in controllerList)
            {
                if (chunkList[chunkIndex].colorType != item.colorName)
                {
                    noOrderChange = false;
                    break;
                }
                chunkIndex++;
            }
        }

        leftOffset = (isStack ? (mainWidth - getWidth(ColorController.instance.getQueueDuration())) : 0);

        if (noOrderChange)
        {
            chunkIndex = 0;
            
            foreach (ColorController.TimedColor item in controllerList)
            {
                RectTransform currTransform = chunkList[chunkIndex].chunkObject.GetComponent<RectTransform>();
                currTransform.sizeDelta = new Vector2(getWidth(item.remainingTime), currTransform.sizeDelta.y);
                currTransform.localPosition = new Vector2(leftOffset, 0);
                leftOffset += currTransform.sizeDelta.x;
                chunkIndex++;
            }
        }
        else
        {
            clearChunkList();
            foreach (ColorController.TimedColor item in controllerList)
            {
                NamedChunk newOne = createNamedChunk(item.colorName, item.remainingTime);
                RectTransform currTransform = newOne.chunkObject.GetComponent<RectTransform>();
                currTransform.localPosition = new Vector2(leftOffset, 0);
                leftOffset += currTransform.sizeDelta.x;
                chunkList.Add(newOne);
            }
        }
    }
}
