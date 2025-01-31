using UnityEngine;

public class CameraEdgeSpawnPoint : MonoBehaviour
{
    private CameraEdgeCalculator _cameraEdgeCalculator;
    private void Awake()
    {
        _cameraEdgeCalculator = FindObjectOfType<CameraEdgeCalculator>();
    }

    public void SetSpawnPoint(GameObject objToSpawn, float edgeDistanceMinRange = 0, float edgeDistanceMaxRange = 0)
    {
        float pushawayfactor = Random.Range(edgeDistanceMinRange, edgeDistanceMaxRange);
        objToSpawn.transform.position = _cameraEdgeCalculator.GetRandomStartPointAlongEdge(pushawayfactor);
    }

//#if UNITY_EDITOR
//    void OnDrawGizmosSelected()
//    {
//        //Vector3 p = _mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
//        Vector2 p = GenerateRandomSpawnPoint();
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawSphere(p, 0.1F);
//    }
//#endif
}
