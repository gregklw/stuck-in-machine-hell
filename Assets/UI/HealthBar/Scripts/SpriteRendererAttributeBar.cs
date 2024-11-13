using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererAttributeBar : MonoBehaviour, IAttributeBar
{
    [SerializeField] private SpriteRenderer _attributeBarRenderer, _attributeBarInnerRenderer;
    [SerializeField] private float _barThickness;
    private Vector2 _maxSize;
    private Transform _pivot;

    //private IEnumerator Start()
    //{
    //    yield return new WaitForEndOfFrame();
    //    _maxSize = _attributeBarRenderer.size;
    //    _attributeBarInnerRenderer.size = _maxSize;
    //}

    //public void CenterBarPosX()
    //{
    //    transform.position -= new Vector3(0, _maxSize.y, 0);
    //}

    public void SetBarAmount(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        _pivot.localScale = new Vector2(percentage, _pivot.localScale.y);
        //Debug.Log(_pivot.localScale);
    }

    public void SetupBar(float width, Vector2 position)
    {
        _attributeBarRenderer.size = new Vector2(width, _barThickness);
        _attributeBarInnerRenderer.size = new Vector2(width, _barThickness);
        _maxSize = _attributeBarRenderer.size;
        //_attributeBarInnerRenderer.size = _maxSize;
        CreatePivotForHealthBar();
        transform.position = position - new Vector2(0, _maxSize.y);
    }

    private void CreatePivotForHealthBar()
    {
        Vector2 boundsDiff = _attributeBarInnerRenderer.bounds.max - _attributeBarInnerRenderer.bounds.min;
        //Debug.Log($"{boundsDiff.x}/{_attributeBarInnerRenderer.bounds.min}/{_attributeBarInnerRenderer.bounds.max}");

        _pivot = _pivot ? _pivot : new GameObject().transform;
        _attributeBarInnerRenderer.transform.SetParent(_pivot.transform, false);

        _pivot.transform.SetParent(_attributeBarRenderer.transform, false);
        _pivot.transform.localPosition = Vector2.zero;
        _pivot.transform.localPosition -= new Vector3(boundsDiff.x / 2, 0);
        _attributeBarInnerRenderer.transform.localPosition = Vector2.zero;
        _attributeBarInnerRenderer.transform.localPosition += new Vector3(boundsDiff.x / 2, 0);
    }
}
