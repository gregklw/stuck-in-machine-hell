using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class AttributeBar : MonoBehaviour
{
    [SerializeField] private RectTransform _attributeBarRect, _attributeBarRectInner;
    private Vector2 _maxSizeDelta;
    private Camera _mainCamera;
    private Camera MainCamera
    {
        get
        {
            _mainCamera ??= GetComponent<Camera>();
            return _mainCamera;
        }
    }

    public Vector2 BarSize => _attributeBarRect.sizeDelta;

    public Vector2 BarPosition
    {
        get => _attributeBarRect.position;
        set => _attributeBarRect.position = value;
    }
    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();
        _maxSizeDelta = _attributeBarRect.rect.size;
        _attributeBarRectInner.sizeDelta = _maxSizeDelta;
    }

    public void RealignBar()
    {
        Vector3[] arrayCorners = new Vector3[4];
        _attributeBarRect.GetWorldCorners(arrayCorners);
        Vector3 dir = arrayCorners[2] - arrayCorners[1];
        _attributeBarRect.position -= dir / 2;
        _maxSizeDelta = _attributeBarRect.sizeDelta;
    }

    public void SetBarDisplayWidth(float xSizeDelta)
    {
        _attributeBarRect.sizeDelta = new Vector2(xSizeDelta, _attributeBarRect.sizeDelta.y);
        _attributeBarRectInner.sizeDelta = new Vector2(xSizeDelta, _attributeBarRect.sizeDelta.y);
    }

    public void SetBarAmount(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        _attributeBarRectInner.sizeDelta = new Vector2(percentage * _maxSizeDelta.x, _maxSizeDelta.y);
    }
}
