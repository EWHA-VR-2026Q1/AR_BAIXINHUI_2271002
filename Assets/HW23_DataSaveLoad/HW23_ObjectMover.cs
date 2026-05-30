using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Physics Raycaster + EventSystem을 통해 오브젝트를 마우스 드래그로 XZ 평면 이동
/// </summary>
[RequireComponent(typeof(Collider))]
public class HW23_ObjectMover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        offset = transform.position - GetWorldPos(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        transform.position = GetWorldPos(eventData.position) + offset;
    }

    private Vector3 GetWorldPos(Vector2 screenPos)
    {
        if (mainCam == null) return transform.position;
        Ray ray = mainCam.ScreenPointToRay(new Vector3(screenPos.x, screenPos.y, 0));
        if (new Plane(Vector3.up, transform.position).Raycast(ray, out float dist))
            return ray.GetPoint(dist);
        return transform.position;
    }
}
