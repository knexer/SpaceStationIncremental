using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes a vanilla GO with a SpriteRenderer act like a button.
// Tints the sprite based on the button state.
// TODO instead of tinting, some other kind of highlighting would be nice
public class SpriteButton : MonoBehaviour {
    public event Action OnClick;

    [SerializeField] private Color restTint;
    [SerializeField] private Color mouseOverTint;
    [SerializeField] private Color mouseDownTint;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool mouseOver = false;
    private bool pressed = false;

    private void OnMouseEnter()
    {
        mouseOver = true;
        UpdateSprite();
    }

    private void OnMouseExit()
    {
        mouseOver = false;
        UpdateSprite();
    }

    private void OnMouseDown()
    {
        pressed = true;
        UpdateSprite();
    }

    private void OnMouseUp()
    {
        pressed = false;
        UpdateSprite();

        if (mouseOver)
        {
            OnClick?.Invoke();
        }
    }

    private void OnDisable()
    {
        mouseOver = false;
        pressed = false;
        UpdateSprite();
    }

    private void OnEnable()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        Color tint = restTint;
        if (mouseOver)
        {
            tint = pressed ? mouseDownTint : mouseOverTint;
        }
        spriteRenderer.color = tint;
    }
}
