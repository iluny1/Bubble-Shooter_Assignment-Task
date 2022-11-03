using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BubbleShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bubbleBulletPrefab;
    [SerializeField] private Transform bubbleBullets;
    [SerializeField] private Transform bubbleParent;
    [SerializeField] private LayerMask layerMask; 
    [SerializeField] private string bubbleBulletColor;
    [SerializeField] private Canvas canvas;
    [SerializeField] private List<GameObject> bubbleShootList;

    private Vector2 direction;
    private bool canShoot = true;

    private void Start()
    {
        //Bubble.OnAnyBubbleDead += Bubble_OnAnyBubbleDead;
        BubbleBullet.OnAnyBubbleBulletDestroy += BubbleBullet_OnAnyBubbleBulletDestroy;
        BubbleBullet.OnAnyBubbleBulletShot += BubbleBullet_OnAnyBubbleBulletShot;

        SetBubbleBullets();
    }

    private void SetBubbleBullets()
    {
        bubbleShootList.Clear();

        int childCount = bubbleBullets.childCount;

        for (int i = 0; i < childCount; i++)
        {
            bubbleShootList.Add(bubbleBullets.GetChild(i).gameObject);
        }
    }

    private void BubbleBullet_OnAnyBubbleBulletShot(object sender, EventArgs e)
    {
        bubbleShootList.RemoveAt(0);
    }

    private void GetDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Touchscreen.current.position.ReadValue());
        Vector2 direction = (mousePosition - transform.position);
        this.direction = direction;
    }

    private void GetColor()
    {
        bubbleBulletColor = bubbleBullets.GetChild(0).GetComponent<BubbleBullet>().GetColor();
    }

    public void GetPath(InputAction.CallbackContext context)
    {
        if(canvas.GetComponent<PauseLogic>().GetIsPaused()) return;
        if (context.started | context.performed) return;
        if (!canShoot) return;
        if (bubbleShootList.Count == 0) return;

        GetDirection();
        GetColor();

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, direction, float.MaxValue, layerMask);

        if (hit1.collider.gameObject.tag == "Border")
        {
            Vector2 hit2Direction = Vector2.Reflect(direction, hit1.normal);
            RaycastHit2D hit2 = Physics2D.Raycast(hit1.point, hit2Direction, float.MaxValue, layerMask);


            Transform bubbleBullet = Instantiate(bubbleBulletPrefab, transform.position, Quaternion.identity);
            bubbleBullet.GetComponent<BubbleBullet>().PrepareToLaunch(hit1.point, hit2.point, hit2.transform, bubbleBulletColor, bubbleParent);
        }

        if (hit1.collider.gameObject.tag == "Bubble")
        {
            Transform bubbleBullet = Instantiate(bubbleBulletPrefab, transform.position, Quaternion.identity);
            bubbleBullet.GetComponent<BubbleBullet>().PrepareToLaunch(hit1.point, hit1.transform, bubbleBulletColor, bubbleParent);
        }

        canShoot = false;
    }

    private void BubbleBullet_OnAnyBubbleBulletDestroy(object sender, System.EventArgs e)
    {
        canShoot = true;
        Debug.Log("Bullets GO count = " + bubbleBullets.childCount);
        SetBubbleBullets();
        Debug.Log("Bullets GO count = " + bubbleBullets.childCount);
    }
}
