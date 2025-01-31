using UnityEngine;

public class PlayerBoundaryGenerator : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _left, _right, _top, _bottom;
    private Vector3[] _worldCameraPositions;
    private CameraEdgeCalculator _cameraEdgeCalculator;

    private void Awake()
    {
        _cameraEdgeCalculator = FindObjectOfType<CameraEdgeCalculator>();
    }

    private void Update()
    {
        _cameraEdgeCalculator = FindObjectOfType<CameraEdgeCalculator>();

        Vector2 bottomBorderSize = _cameraEdgeCalculator.CamBottomRight - _cameraEdgeCalculator.CamBottomLeft;
        Vector2 topBorderSize = _cameraEdgeCalculator.CamTopRight - _cameraEdgeCalculator.CamTopLeft;
        Vector2 leftBorderSize = _cameraEdgeCalculator.CamTopLeft - _cameraEdgeCalculator.CamBottomLeft;
        Vector2 rightBorderSize = _cameraEdgeCalculator.CamTopRight - _cameraEdgeCalculator.CamBottomRight;

        _left.size = new Vector2(1, leftBorderSize.y);
        _right.size = new Vector2(1, rightBorderSize.y);
        _top.size = new Vector2(topBorderSize.x, 1);
        _bottom.size = new Vector2(bottomBorderSize.x, 1);

        Vector2 leftBorderPos = (_cameraEdgeCalculator.CamTopLeft + _cameraEdgeCalculator.CamBottomLeft) / 2;
        leftBorderPos.x -= _left.size.x / 2;
        _left.transform.position = leftBorderPos;

        Vector2 rightBorderPos = (_cameraEdgeCalculator.CamTopRight + _cameraEdgeCalculator.CamBottomRight) / 2;
        rightBorderPos.x += _right.size.x / 2;
        _right.transform.position = rightBorderPos;

        Vector2 topBorderPos = (_cameraEdgeCalculator.CamTopRight + _cameraEdgeCalculator.CamTopLeft) / 2;
        topBorderPos.y -= _top.size.y / 2;
        _top.transform.position = topBorderPos;

        Vector2 bottomBorderPos = (_cameraEdgeCalculator.CamBottomRight + _cameraEdgeCalculator.CamBottomLeft) / 2;
        bottomBorderPos.y += _bottom.size.y / 2;
        _bottom.transform.position = bottomBorderPos;
    }
}
