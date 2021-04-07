using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wire : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Image _image;

    public bool IsLeftWire;
    public Color CustomColor;
    private LineRenderer _line;
    private Canvas _canvas;
    [SerializeField]
    private bool _isDrag = false;

    private WireTask _wireTask;
    public bool IsSuccess = false;

    public void Init()
    {
        _image = GetComponent<Image>();
        _line = GetComponent<LineRenderer>();
        _canvas = GetComponentInParent<Canvas>();
        _wireTask = GetComponentInParent<WireTask>();
        _isDrag = false;
        IsSuccess = false;
    }

    private void Update()
    {
        if (_isDrag) {
            Vector2 mPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
                Input.mousePosition, _canvas.worldCamera, out mPos);
            _line.SetPosition(0, transform.position);
            _line.SetPosition(1, _canvas.transform.TransformPoint(mPos));
        } else {
            if (!IsSuccess) {
                _line.SetPosition(0, Vector3.zero);
                _line.SetPosition(1, Vector3.zero);
            }
        }

        
        if (RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition,_canvas.worldCamera)) {
            _wireTask.currentHoveredWire = this;
        }
    }

    public void SetColor(Color color)
    {
        _image.color = color;
        _line.startColor = color;
        _line.endColor = color;
        CustomColor = color;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (_wireTask.currentHoveredWire != null) {
            if (_wireTask.currentHoveredWire.CustomColor == CustomColor && !_wireTask.currentHoveredWire.IsLeftWire) {
                IsSuccess = true;

                _wireTask.currentHoveredWire.IsSuccess = true;
            }
        }
        _isDrag = false;
        _wireTask.currentDraggedWire = null;
    }

    public void OnDrag(PointerEventData eventData) {
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (!IsLeftWire) { return; }
        if (IsSuccess) { return; }
        _isDrag = true;
        _wireTask.currentDraggedWire = this;
    }
}
