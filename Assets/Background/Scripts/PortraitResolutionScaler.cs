using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitResolutionScaler : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        //transform.localScale = Vector3.one * 2;
        float scale = (1 / _spriteRenderer.sprite.textureRect.size.y * _spriteRenderer.sprite.pixelsPerUnit * Camera.main.orthographicSize) * 2;
        Debug.Log(Camera.main.orthographicSize);
        transform.localScale = Vector3.one * scale;
    }

    //void Update()
    //{

    //    float scale = (1 / _spriteRenderer.sprite.textureRect.size.y * _spriteRenderer.sprite.pixelsPerUnit * Camera.main.orthographicSize) * 2;
    //    Debug.Log(Camera.main.orthographicSize);
    //    transform.localScale = Vector3.one * scale;
    //}
}
