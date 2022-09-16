using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onClick;
    public UnityEvent onRightClick;
    public UnityEvent onLeftClick;
    public Image image;
    public Sprite mainSprite;
    public Sprite clickSprite;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (clickSprite != null)
        {
            image.sprite = clickSprite;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (mainSprite != null)
        {
            image.sprite = mainSprite;
        }
        onClick.Invoke();
        switch (eventData.pointerId)
        {
            case -1:
                onLeftClick.Invoke();
                break;
            case -2:
                onRightClick.Invoke();
                break;
            default:
                break;
        }
    }
}
