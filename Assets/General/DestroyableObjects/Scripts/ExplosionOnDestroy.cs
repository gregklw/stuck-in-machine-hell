using Unity.VisualScripting;
using UnityEngine;

public class ExplosionOnDestroy : MonoBehaviour
{

    public void InitSize(SpriteRenderer destroyedObjectRenderer)
    {
        Vector3 explosionRendererSize = GetComponent<SpriteRenderer>().bounds.size;
        Vector3 destroyedRendererSize = destroyedObjectRenderer.bounds.size;
        float xAspectRatio = destroyedRendererSize.x / explosionRendererSize.x;
        float yAspectRatio = destroyedRendererSize.y / explosionRendererSize.y;

        float preferredDimension = destroyedRendererSize.x > destroyedRendererSize.y ? xAspectRatio : yAspectRatio;

        transform.localScale = new Vector3(preferredDimension, preferredDimension, preferredDimension);
    }

    public void DestroyObject()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject);
    }
}
