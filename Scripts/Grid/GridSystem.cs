using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private const float HEX_VERTICAL_OFFSET_MULTI = 0.75f;
    private const float HEX_HORIZONTAL_OFFSET_MULTI = 0.5f;
    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridObjectArray;

    public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new TGridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridPosition gridPosition = new GridPosition(x, y);
                gridObjectArray[x, y] = createGridObject(this, gridPosition);
            }
        }
    }

    public Vector2 GetWorldPosition(GridPosition gridPosition)
    {
        return
            new Vector2(gridPosition.x, 0) * cellSize +
            new Vector2(0, gridPosition.y) * cellSize * HEX_VERTICAL_OFFSET_MULTI +
            (((gridPosition.y % 2) == 1) ? new Vector2(1, 0) * cellSize * HEX_HORIZONTAL_OFFSET_MULTI : Vector2.zero);
    }

    public GridPosition GetGridPosition(Vector2 worldPosition)
    {
        GridPosition roughXY = new GridPosition(Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.y / cellSize / HEX_VERTICAL_OFFSET_MULTI));

        bool oddRow = roughXY.y % 2 == 1;

        List<GridPosition> gridPositions = new List<GridPosition>
        {
            roughXY,
            roughXY + new GridPosition(-1,0),
            roughXY + new GridPosition(1,0),

            roughXY + new GridPosition(0,1),
            roughXY + new GridPosition(0,-1),

            roughXY + new GridPosition(oddRow ? +1 : -1, +1),
            roughXY + new GridPosition(oddRow ? +1 : -1, -1),
        };

        GridPosition closestGridPositions = new GridPosition(int.MaxValue, int.MaxValue);

        foreach (GridPosition gridPosition in gridPositions)
        {
            float distanceWPGP = Vector2.Distance(worldPosition, GetWorldPosition(gridPosition)); //Debug.Log("Distance to gridPosition " + distanceWPGP);
            float distanceWPCGP = Vector2.Distance(worldPosition, GetWorldPosition(closestGridPositions)); //Debug.Log("Distance to CurgridPosition " + distanceWPCGP);
            if ((distanceWPGP < distanceWPCGP) && (IsValidGridPosition(gridPosition)) &&
                LevelGrid.Instance.HasAnyUnitOnGridPosition(gridPosition) != true)
            {
                closestGridPositions = gridPosition; //Debug.Log("This POS better");
            }
        }

        return closestGridPositions;
    }

    /*public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridPosition gridPosition = new GridPosition(x, y);

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);

                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();

                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }*/

    public TGridObject GetGridObject(GridPosition gridPosition)
    {        
        return gridObjectArray[gridPosition.x, gridPosition.y];        
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
               gridPosition.y >= 0 &&
               gridPosition.x < width &&
               gridPosition.y < height;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}
