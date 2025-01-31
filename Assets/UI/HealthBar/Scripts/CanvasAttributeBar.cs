using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAttributeBar : MonoBehaviour, IAttributeBar
{
    [SerializeField] private RectTransform _attributeBarRect, _attributeBarRectInner;
    private Vector2 _maxSizeDelta;

    private void Start()
    {
        _maxSizeDelta = _attributeBarRect.rect.size;
        _attributeBarRectInner.sizeDelta = _maxSizeDelta;
    }

    //public void CenterBarPosX()
    //{
    //    Vector3[] arrayCorners = new Vector3[4];
    //    _attributeBarRect.GetWorldCorners(arrayCorners);
    //    //if this gets the world corners of rect then how does this scale depending on the image?

    //    Vector3 dir = arrayCorners[2] - arrayCorners[1];
    //    _attributeBarRect.position -= dir / 2;
    //    _maxSizeDelta = _attributeBarRect.sizeDelta;
    //}

    public void SetupBar(float width, Vector2 position)
    {
        _attributeBarRect.sizeDelta = new Vector2(width, _attributeBarRect.sizeDelta.y);
        _attributeBarRectInner.sizeDelta = new Vector2(width, _attributeBarRect.sizeDelta.y);

        Vector3[] arrayCorners = new Vector3[4];
        _attributeBarRect.GetWorldCorners(arrayCorners);
        //if this gets the world corners of rect then how does this scale depending on the image?

        Vector3 dir = arrayCorners[2] - arrayCorners[1];
        _attributeBarRect.position -= dir / 2;
        _maxSizeDelta = _attributeBarRect.sizeDelta;

        _maxSizeDelta = _attributeBarRect.rect.size;
        _attributeBarRectInner.sizeDelta = _maxSizeDelta;
    }

    public void SetBarAmount(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        _attributeBarRectInner.sizeDelta = new Vector2(percentage * _maxSizeDelta.x, _maxSizeDelta.y);
    }
}
