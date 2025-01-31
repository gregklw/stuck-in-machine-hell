using UnityEngine;

public class CameraEdgeCalculator : MonoBehaviour
{
    private Camera _mainCamera;
    private Vector2[][] _camCornerPairs;
    private Vector2 _cameraCenterPoint;
    public Vector2 CamBottomLeft { get; private set; }
    public Vector2 CamBottomRight { get; private set; }
    public Vector2 CamTopLeft { get; private set; }
    public Vector2 CamTopRight { get; private set; }


    private void Awake()
    {
        _mainCamera = Camera.main;
        CamBottomLeft = _mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        CamBottomRight = _mainCamera.ViewportToWorldPoint(new Vector2(1, 0));
        CamTopLeft = _mainCamera.ViewportToWorldPoint(new Vector2(0, 1));
        CamTopRight = _mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        _camCornerPairs = new Vector2[][]{
            new Vector2[]{
                CamBottomLeft,
                CamBottomRight
            },
            new Vector2[]{
                CamBottomLeft,
                CamTopLeft
            },
            new Vector2[]{
                CamTopLeft,
                CamTopRight
            },
            new Vector2[]{
                CamBottomRight,
                CamTopRight
            }
        };

        _cameraCenterPoint = _mainCamera.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
    }

    public Vector2 GetRandomStartPointAlongEdge(float pushAwayStartPointFactor = 0)
    {
        Vector2[] camCornerPair = GetRandomCameraCornerPair();

        //Get random point on edge of camera
        Vector2 randomCameraEdgePoint = Vector2.Lerp(camCornerPair[0], camCornerPair[1], Random.Range(0.0f, 1.0f));

        //Create a direction vector that will push the point further away from the center
        Vector2 pushAwayDirection = (randomCameraEdgePoint - _cameraCenterPoint).normalized * pushAwayStartPointFactor;

        //Add or subtract direction based on how far awar from screen you want spawn point to be
        return randomCameraEdgePoint + pushAwayDirection;
    }

    private Vector2[] GetRandomCameraCornerPair()
    {
        return _camCornerPairs[Random.Range(0, 4)];
    }
}
