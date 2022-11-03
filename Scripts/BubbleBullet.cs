using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class BubbleBullet : MonoBehaviour
{
    [SerializeField] private Transform bubbleExp;
    [SerializeField] private Transform bubblePrefab;
    [SerializeField] private Collider2D bubbleBulletCollider;
    [SerializeField] private bool isPresetColor;
    [SerializeField] private string color;
    [SerializeField] private LayerMask layerMask;

    private string[] colors = { "red", "blue", "yellow", "green", "pink" };
    private float bulletSpeed = 10f;
    private bool isLaunched = false;
    private int pointsCount;
    private int activePoint = 1;
    private List<Vector2> points;
    private ContactFilter2D contactFilter;
    private Transform target;
    private Transform parent;


    public static event EventHandler OnAnyBubbleBulletDestroy;
    public static event EventHandler OnAnyBubbleBulletShot;

    private void Awake()
    {
        contactFilter.layerMask = layerMask;
        points = new List<Vector2>();

        if (isPresetColor) SetColor(color);
        else
        {
            GetColorFromColors();
            SetColor(color);
        }
    }

    private void Update()
    {
        if (isLaunched)
        {
            switch (pointsCount)
            {
                case 1:
                    MoveToTarget(points[0], target);
                    break;
                case 2:
                    MoveToTarget(points[0], points[1], target);
                    break;
                default:
                    Debug.Log("No such case");
                    break;
            }
        }
    }

    private void MoveToTarget(Vector2 targetPosition, Transform target)
    {
        Collider2D[] result = new Collider2D[pointsCount];
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, bulletSpeed * Time.deltaTime);
        if (transform.position == new Vector3(targetPosition.x, targetPosition.y, 0))
        {
            GotToPoint(target);
        }
    }

    private void MoveToTarget(Vector2 targetPosition1, Vector2 targetPosition2, Transform target)
    {
        if (activePoint == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition1, bulletSpeed * Time.deltaTime);
            if (transform.position == new Vector3(targetPosition1.x, targetPosition1.y, 0)) activePoint = 2;
        }

        if (activePoint == 2)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition2, bulletSpeed * Time.deltaTime);
            if (transform.position == new Vector3(targetPosition2.x, targetPosition2.y, 0))
            {
                GotToPoint(target);
            }
        }
    }


    public void PrepareToLaunch(Vector2 targetPosition, Transform target, string color, Transform parent)
    {
        pointsCount = 1;
        points.Add(targetPosition);
        this.target = target;
        this.parent = parent;
        SetColor(color);
        isLaunched = true;
        OnAnyBubbleBulletShot?.Invoke(this, EventArgs.Empty);

    }

    public void PrepareToLaunch(Vector2 targetPosition1, Vector2 targetPosition2, Transform target, string color, Transform parent)
    {
        pointsCount = 2;
        points.Add(targetPosition1);
        points.Add(targetPosition2);
        this.target = target;
        this.parent = parent;
        SetColor(color);
        isLaunched = true;
        OnAnyBubbleBulletShot?.Invoke(this, EventArgs.Empty);
    }

    private void GotToPoint(Transform target)
    {
        if (target.gameObject.tag == "Bubble")
        {
            BubbleManager(target);
        }
        else
        {
            OnAnyBubbleBulletDestroy?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }

    private void BubbleManager(Transform target)
    {
        if (this.color == target.GetComponent<Bubble>().GetColor())
        {
            target.GetComponent<Bubble>().DestroyBubble();
        }
        else
        {
            Transform newBubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity, parent);
            newBubble.GetComponent<Bubble>().SetColor(GetColor());
            newBubble.GetComponent<Bubble>().OnArtificialSpawn();
        }

        OnAnyBubbleBulletDestroy?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    public Vector2 GetWorldPosition()
    {
        return transform.position;
    }

    private void GetColorFromColors()
    {
        int c = UnityEngine.Random.Range(0, colors.Length);
        color = colors[c];
    }

    public void SetColor(string colorName)
    {
        this.color = colorName;
        Color color;
        switch (colorName)
        {
            case "red":
                color = Color.red;
                break;
            case "blue":
                color = Color.blue;
                break;
            case "yellow":
                color = Color.yellow;
                break;
            case "green":
                color = Color.green;
                break;
            case "pink":
                color = new Color(206, 0, 112);
                break;
            default:
                color = new Color(0, 0, 0);
                break;
        }
        transform.GetComponent<SpriteRenderer>().color = color;
    }

    public string GetColor()
    {
        return this.color;
    }

    public void DestroyBubble()
    {
        if (bubbleExp != null) Instantiate(bubbleExp, transform.position, Quaternion.identity);
        OnAnyBubbleBulletDestroy?.Invoke(this, EventArgs.Empty);
        Destroy(this);
    }
}
