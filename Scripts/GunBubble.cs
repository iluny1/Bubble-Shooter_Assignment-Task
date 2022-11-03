using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GunBubble : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform sight;
    [SerializeField] private Canvas canvas;

    private Vector2 direction;

    public void OnMouseMove()
    {
        if (canvas.GetComponent<PauseLogic>().GetIsPaused()) return;
        GetDirection();
        GunRotationUpdate(direction);
        GetPath();
    }

    private void GetDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Touchscreen.current.position.ReadValue());
        Vector2 direction = (mousePosition - transform.position).normalized;
        this.direction = direction;
    }

    private void GunRotationUpdate(Vector2 direction)
    {
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        if (angle > 90f) angle = 90f;
        if (angle < -90f) angle = -90f;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void GetPath()
    {
        Vector3 direction = sight.position - transform.position;
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, direction, float.MaxValue, layerMask);

        UpdateLine(hit1.point, hit1.point, hit1.point, 2);

        if (hit1.collider.gameObject.tag == "Border")
        {
            Vector2 hit2Direction = Vector2.Reflect(direction, hit1.normal);
            RaycastHit2D hit2 = Physics2D.Raycast(hit1.point, hit2Direction, float.MaxValue, layerMask);
            Debug.DrawRay(hit1.point, hit1.normal, Color.blue, 2f);
            UpdateLine(hit1.point, hit2.point, hit1.point, 3);
        }
    }

    private void UpdateLine(Vector2 hitPosition1, Vector2 hitPosition2, Vector2 hitPosition3, int numberOfHits)
    {
        Vector3[] positions = new Vector3[] { transform.position, hitPosition1, hitPosition2, hitPosition3 };
        lineRenderer.positionCount = numberOfHits;
        lineRenderer.SetPositions(positions);
    }
}
