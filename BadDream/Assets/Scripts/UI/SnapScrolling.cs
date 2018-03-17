using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour {

    [System.Serializable]
    public struct Level
    {
        public Sprite image;
        public string name;
    }

    [Header("Controllers")]
    [Range(0, 500)]
    public int panelsOffset;
    [Range(0, 20)]
    public int snapSpeed ;
    [Range(0, 5f)]
    public float scaleOffset;
    [Range(0, 20f)]
    public float scaleSpeed;
    [Range(0, 2000f)]
    public float scrollSpeedReset;

    [Header("Other objects")]
    public GameObject panelPrefab;
    public ScrollRect scrollRect;
    public Canvas canvas;
    [Header("Levels init")]
    public List<Level> levels;

    private GameObject[] levelPans;
    private Vector2[] pansPos;
    private Vector2[] pansScale;

    private RectTransform contentRect;
    private Vector2 contentVector;

    private int selectedPanelIndex;

    private bool isScrolling;

    public bool init = false;

	void Start () {
        Init();
	}

    public void Init()
    {
        
        contentRect = GetComponent<RectTransform>();
        levelPans = new GameObject[levels.Count];
        pansPos = new Vector2[levels.Count];
        pansScale = new Vector2[levels.Count];
        for (int i = 0; i < levels.Count; i++)
        {
            
            
            levelPans[i] = Instantiate(panelPrefab, transform, false);
            float size = levelPans[i].GetComponentInChildren<RectTransform>().rect.size.x;
            levelPans[i].GetComponentInChildren<Image>().sprite = levels[i].image;
            levelPans[i].GetComponentInChildren<Text>().text = levels[i].name;
            
            if (i == 0) continue;
            levelPans[i].transform.localPosition = new Vector2(levelPans[i - 1].transform.localPosition.x +
                           size + panelsOffset, transform.localPosition.y);
            pansPos[i] = -levelPans[i].transform.localPosition;
        }
        init = true;
    }

    private void FixedUpdate()
    {
        if (!init) return;
        if (contentRect.anchoredPosition.x >= pansPos[0].x && !isScrolling || contentRect.anchoredPosition.x <= pansPos[levels.Count - 1].x && !isScrolling)
        { 
            scrollRect.inertia = false;
        }
        float minD = float.MaxValue;
        for(int i = 0; i < levels.Count; i++)
        {
            float d = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
            if(d < minD)
            {
                minD = d;
                selectedPanelIndex = i;
            }
            float scale = Mathf.Clamp(1 / (d / panelsOffset) * scaleOffset, 0.5f, 1f);
            pansScale[i].x = Mathf.SmoothStep(levelPans[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
            pansScale[i].y = Mathf.SmoothStep(levelPans[i].transform.localScale.y, scale, scaleSpeed * Time.fixedDeltaTime);
            levelPans[i].transform.localScale = pansScale[i];
        }
        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < scrollSpeedReset && !isScrolling) scrollRect.inertia = false;
        if (isScrolling || scrollVelocity > scrollSpeedReset) return;
        if (isScrolling) return;
        contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanelIndex].x, snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll) scrollRect.inertia = true;
    }
}
