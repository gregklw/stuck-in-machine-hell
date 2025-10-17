using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovementIndicator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 _currentPosition;

    [SerializeField] private Image _joystickBg, _joystick, _directionArrow;

    [SerializeField] private Player _playerRef;

    private bool _isDragging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //FindFirstObjectByType
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    EnableIndicator();
        //}
        //else if (Input.GetMouseButton(0))
        //{
        if (!_isDragging) return;

        Vector2 inputDragDirection = Input.mousePosition - _joystickBg.rectTransform.position;


        // Calculate rotation angle in degrees
        float angle = Mathf.Atan2(inputDragDirection.y, inputDragDirection.x) * Mathf.Rad2Deg;

        // Apply rotation (assuming arrow’s default forward direction is right)
        _directionArrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        Vector2 arrowDimensions = _directionArrow.rectTransform.sizeDelta;
        Vector2 finalDirectionVector = inputDragDirection;
        float bgRadius = _joystickBg.rectTransform.sizeDelta.x / 2;
        if (inputDragDirection.magnitude < bgRadius)
        {
            arrowDimensions.x = inputDragDirection.magnitude;
            _directionArrow.rectTransform.sizeDelta = arrowDimensions;
            _joystick.rectTransform.position = Input.mousePosition;
        }
        else
        {
            finalDirectionVector = (Vector3)inputDragDirection.normalized * bgRadius;
            _joystick.rectTransform.position = _joystickBg.rectTransform.position + (Vector3)finalDirectionVector;
        }

        //float movespeed = _directionArrow.rectTransform.sizeDelta.magnitude;
        _playerRef.MovementAction?.Invoke(finalDirectionVector);
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    DisablebleIndicator();
        //}
    }

    private void EnableIndicator()
    {
        _joystickBg.rectTransform.position = Input.mousePosition;
        _joystickBg.gameObject.SetActive(true);
        _isDragging = true;
    }

    private void DisablebleIndicator()
    {
        _isDragging = false;
        _joystickBg.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        EnableIndicator();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DisablebleIndicator();
    }
}
