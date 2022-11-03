using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class Bubble : MonoBehaviour
{
    [SerializeField] private GameObject bubbleExp;
    [SerializeField] private bool isPresetColor;
    [SerializeField] private string color;
    [SerializeField] private string[] colors = { "red", "blue", "yellow", "green", "pink" };

    private GridPosition gridPosition;

    public static event EventHandler OnAnyBubbleSpawn;
    public static event EventHandler OnAnyBubbleDead;

    private void Awake()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        transform.position = LevelGrid.Instance.GetWorldPosition(gridPosition);

        if (isPresetColor) SetColor(color);
        else
        {
            GetColorFromColors();
            SetColor(color);
        }
    }

    private void Start()
    {
        OnAnyBubbleSpawn?.Invoke(this, EventArgs.Empty);
    }

    public void OnArtificialSpawn()
    {
        OnAnyBubbleDead?.Invoke(this, EventArgs.Empty);
    }

    private void MoveBubble()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMoveGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
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
        Color color = new Color();
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
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    public string GetColor()
    {
        return this.color;
    }

    public void DestroyBubble()
    {        
        if (bubbleExp != null) Instantiate(bubbleExp, transform.position, Quaternion.identity);
        //Debug.Log("DestroyBubble Event");
        OnAnyBubbleDead?.Invoke(this, EventArgs.Empty);        
        Destroy(gameObject);
    }
}
