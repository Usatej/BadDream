using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonEvents : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public UnityEvent buttonUnityEvent;

    public bool activ = true;
    private bool wasDown = false;

    public Text text;
    public Color textColor;

    private Color oldTextColor;

    public List<Transform> objects;
    public List<Color> colors;

    private List<Color> oldColors;

    private void Start()
    {
        oldColors = new List<Color>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!activ) return;
        wasDown = true;
        oldColors.Clear();
        oldTextColor = text.color;
        text.color = textColor;
        int i = 0;
        foreach(Transform x in objects)
        {
            SpriteRenderer sp = x.GetComponent<SpriteRenderer>();
            oldColors.Add(sp.color);
            sp.color = colors[i];
            i++;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!wasDown) return;
        text.color = oldTextColor;
        int i = 0;
        foreach (Transform x in objects)
        {
            SpriteRenderer sp = x.GetComponent<SpriteRenderer>();
            sp.color = oldColors[i];
            i++;
        }
        if (!activ) return;
        if (RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera))
        {
            buttonUnityEvent.Invoke();
        }
    }

    public void SetActive(bool isActive)
    {
        activ = isActive;
    }
}
