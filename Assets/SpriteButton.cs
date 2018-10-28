using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Makes a vanilla GO with a SpriteRenderer act like a button.
// Tints the sprite based on the button state.
// TODO instead of tinting, some other kind of highlighting would be nice
public class SpriteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
    public event Action OnClick;

    [SerializeField] private Color restTint;
    [SerializeField] private Color mouseOverTint;
    [SerializeField] private Color mouseDownTint;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool mouseOver = false;
    private bool pressed = false;

    public void OnPointerEnter(PointerEventData data)
    {
        mouseOver = true;
        UpdateSprite();
    }

    public void OnPointerExit(PointerEventData data)
    {
        mouseOver = false;
        UpdateSprite();
    }

    public void OnPointerDown(PointerEventData data)
    {
        pressed = true;
        UpdateSprite();
    }

    public void OnPointerUp(PointerEventData data)
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
