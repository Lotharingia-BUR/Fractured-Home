using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHoverable : MonoBehaviour
{
    public RectTransform rectTransform;

    public Color defaultColor;
    public Color highlightColor;

    protected bool _isHovered;
    protected bool _wasHovered;

    protected Image _highlight;

    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        _highlight = GetComponent<Image>();
        _highlight.color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        LogicUpdate();
    }

    protected virtual void LogicUpdate()
    {
        _isHovered = RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition);

        if (_isHovered && !_wasHovered)
        {
            OnPointerEnter();
        }
        else if (!_isHovered && _wasHovered)
        {
            OnPointerExit();
        }

        if (_isHovered && Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnPointerDown();
        }
        else if (_isHovered && Input.GetKey(KeyCode.Mouse0))
        {
            OnPointerHold();
        }
        else if (_isHovered && Input.GetKeyUp(KeyCode.Mouse0))
        {
            OnPointerUp();
        }

        _wasHovered = _isHovered;
    }


    protected virtual void OnPointerEnter()
    {
        _highlight.color = highlightColor;
    }
    protected virtual void OnPointerExit()
    {
        _highlight.color = defaultColor;
    }
    protected virtual void OnPointerDown() { }
    protected virtual void OnPointerHold() { }
    protected virtual void OnPointerUp() { }
}