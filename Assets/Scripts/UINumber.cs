using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINumber : MonoBehaviour
{
    public int number;
    public int c_n;
    public List<int> numbers;
    public List<Image> spriterenderers;
    public GameObject obj;

    void Start()
    {

    }

    void Update()
    {
        SetNumbers();
        c_n = numbers.Count;

        while (spriterenderers.Count > c_n)
        {
            Destroy(spriterenderers[spriterenderers.Count - 1].gameObject);
            spriterenderers.RemoveAt(spriterenderers.Count - 1);
        }
        while (spriterenderers.Count < c_n)
        {
            GameObject go = Instantiate(obj);
            Image sr = go.GetComponent<Image>();
            spriterenderers.Add(sr);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.SetParent(transform);
            rt.localPosition = new Vector3(-(spriterenderers.Count - 1) * 4, 0, 0);
        }

        for (int i = 0; i < spriterenderers.Count; i++)
        {
            spriterenderers[i].sprite = GameManager.Instance.settings.numbers[numbers[i]];
        }
    }

    public void SetNumbers()
    {
        int num = number;
        numbers.Clear();
        while (0 < num)
        {
            numbers.Add(num % 10);
            num = num / 10;
        }
    }
}
