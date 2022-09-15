using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public Stack<UIElement> UIElementStack = new Stack<UIElement>();

    void Start()
    {
        GameInput.UIPrev = () =>
        {
            prev();
        };
    }

    void Update()
    {
        GameInputSetter.Instance.UIInputMask = UIElementStack.Count != 0;
    }

    public void prev()
    {
        if (UIElementStack.TryPop(out UIElement e))
        {
            e.gameObject.SetActive(false);
        }
        else
        {
            GameManager.Instance.game = false;
        }
    }

    public static void Prev() => Instance.prev();

    public void show(UIElement uIElement)
    {
        UIElementStack.Push(uIElement);
        uIElement.gameObject.SetActive(true);
    }

    public static void Show(UIElement uIElement) => Instance.show(uIElement);
}
