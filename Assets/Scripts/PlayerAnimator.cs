using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public PlayerAnimationState state;
    private PlayerAnimationState _state;
    public List<animation> animations;
    public SpriteRenderer sr;
    public float animationinterval;
    public float elapsed;
    public int animationtick;

    void Start()
    {
        
    }

    void Update()
    {
        if (elapsed > animationinterval)
        {
            animation anim = animations[(int)state];
            if (animationtick >= anim.sprites.Length) animationtick = 0;
            sr.sprite = anim.sprites[animationtick];

            animationtick++;
            elapsed = 0;
        }

        if (_state != state) animationtick = 0;

        elapsed += Time.deltaTime;
        _state = state;
    }
}

public enum PlayerAnimationState
{
    Idle,
    Walk,
}

[System.Serializable]
public class animation
{
    public Sprite[] sprites;
}