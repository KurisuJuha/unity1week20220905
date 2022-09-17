using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardTable : MonoBehaviour
{
    public int index;

    public Image[] cardimages;
    public int[] cards = new int[3];
    public GameObject selecter;
    public bool select;

    void Start()
    {
        
    }

    void Update()
    {
        index = Mathf.Clamp(index, 0, 2);
        selecter.transform.position = cardimages[index].transform.position;
    }

    public void Set()
    {
        for (int i = 0; i < 3; i++)
        {
            cards[i] = Random.Range(0, GameManager.Instance.settings.cards.Length);
            cardimages[i].sprite = GameManager.Instance.settings.cards[cards[i]].sprite;
        }
        select = false;
        GameInputSetter.Instance.CardTableInputMask = true;
    }

    public void Select()
    {
        if (!select)
        {
            SkillManager.Instance.quantity[cards[index]]++;
            select = true;
            GameInputSetter.Instance.CardTableInputMask = false;
            SkillManager.Instance.show = false;
            switch (cards[index])
            {
                case 0:
                    SkillManager.Instance.card_critical += GameManager.Instance.settings.cards[cards[index]].add;
                    break;
                case 1:
                    SkillManager.Instance.card_luck += GameManager.Instance.settings.cards[cards[index]].add;
                    break;
                case 2:
                    SkillManager.Instance.card_power += GameManager.Instance.settings.cards[cards[index]].add;
                    break;
                case 3:
                    SkillManager.Instance.card_speed += GameManager.Instance.settings.cards[cards[index]].add;
                    break;
                default:
                    break;
            }
        }
    }

    public void Change(int index)
    {
        this.index = index;
    }
}
