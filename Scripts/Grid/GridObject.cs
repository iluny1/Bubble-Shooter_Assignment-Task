using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Bubble> bubbleList;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        bubbleList = new List<Bubble>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Bubble unit in bubbleList)
        {
            unitString += unit + "\n";
        }

        return gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(Bubble unit)
    {
        bubbleList.Add(unit);
    }

    public void RemoveUnit(Bubble unit)
    {
        bubbleList.Remove(unit);
    }

    public List<Bubble> GetBubbleList()
    {
        return bubbleList;
    }

    public bool HasAnyBubble()
    {
        return bubbleList.Count > 0;
    }

    public Bubble GetBubble()
    {
        if (HasAnyBubble())
        {
            return bubbleList[0];
        }
        else
        {
            return null;
        }
    }
}
