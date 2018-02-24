using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Image bgImage;
    private Image joyImage;


    public Vector2 Input { set; get; } 


    private void Start()
    {
        bgImage = GetComponent<Image>();
        joyImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData evData)
    {
        Vector2 ps = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, evData.position, evData.enterEventCamera, out ps))
        {
            ps.x = (ps.x / bgImage.rectTransform.rect.width) * 2 - 1;
            ps.y = (ps.y / bgImage.rectTransform.rect.height) * 2 - 1;
            
            Input = (ps.magnitude > 1) ? ps.normalized : ps;
            joyImage.rectTransform.anchoredPosition = new Vector2(Input.x * (bgImage.rectTransform.rect.width / 3), Input.y * (bgImage.rectTransform.rect.height / 3));
        }
    }

    public void OnPointerDown(PointerEventData evData)
    {
        OnDrag(evData);
    }

    public void OnPointerUp(PointerEventData evData)
    {
        joyImage.rectTransform.anchoredPosition = Input = Vector2.zero;
    }

    public bool IsThereInput()
    {
        return Input != Vector2.zero;
    }
}
