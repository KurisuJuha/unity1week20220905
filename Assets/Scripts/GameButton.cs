using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameButton : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onClick;
    public Image image;
    public Sprite mainSprite;
    public Sprite clickSprite;

    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = clickSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = mainSprite;
        onClick.Invoke();
    }
}
