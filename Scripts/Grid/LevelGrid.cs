using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private GridSystem<GridObject> gridSystem;

    public static event EventHandler OnAnyUnitMovedGridPosition;
    public static event EventHandler OnAnyUnitGetRemoved;

    public static LevelGrid Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        gridSystem = new GridSystem<GridObject>(width, height, cellSize,
                (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

    }

    private void Start()
    {
        Bubble.OnAnyBubbleDead += Bubble_OnAnyBubbleDead;
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Bubble bubble)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(bubble);
    }

    public List<Bubble> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetBubbleList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Bubble bubble)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(bubble);
        OnAnyUnitGetRemoved?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition(Vector2 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector2 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public void UnitMoveGridPosition(Bubble bubble, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, bubble);
        AddUnitAtGridPosition(toGridPosition, bubble);
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyBubble();
    }

    public Bubble GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetBubble();
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    private void Bubble_OnAnyBubbleDead(object sender, EventArgs e)
    {
        Bubble bubble = sender as Bubble;
        GridPosition gridPosition = bubble.GetGridPosition();

        bool oddRow = gridPosition.y % 2 == 1;

        List<GridPosition> gridPositions = new List<GridPosition>
        {
            gridPosition + new GridPosition(-1,0),
            gridPosition + new GridPosition(1,0),

            gridPosition + new GridPosition(0,1),
            gridPosition + new GridPosition(0,-1),

            gridPosition + new GridPosition(oddRow ? +1 : -1, +1),
            gridPosition + new GridPosition(oddRow ? +1 : -1, -1),
        };

        foreach (GridPosition gridPositionInList in gridPositions.ToList())
        {
            if (!IsValidGridPosition(gridPositionInList))
            {                
                gridPositions.Remove(gridPositionInList);
            }
        }

        /*Debug.Log("Valid GPs");

        foreach (GridPosition gridPositionInList in gridPositions)
        {
            Debug.Log(gridPositionInList);
        }
        */

        if (gridPositions.Count == 0) return;

        if (gridPositions.Count != 0)
        {
            foreach (GridPosition gridPositionInList in gridPositions)
            {
                if (HasAnyUnitOnGridPosition(gridPositionInList))
                {
                    Bubble bubbleColorCheck = GetUnitAtGridPosition(gridPositionInList);
                    if (bubbleColorCheck.GetColor() == bubble.GetColor())
                    {
                        //Debug.Log("Color of " + bubble.gameObject + "is" + bubble.GetColor() + " and" + bubbleColorCheck.gameObject + "color is " + bubbleColorCheck.GetColor());
                        RemoveUnitAtGridPosition(bubble.GetGridPosition(), bubble);
                        //Debug.Log("Destroyed at " + bubbleColorCheck.GetGridPosition());
                        if (bubbleColorCheck != null)
                            bubbleColorCheck.DestroyBubble();
                    }
                }
            }
        }        
    }
}
