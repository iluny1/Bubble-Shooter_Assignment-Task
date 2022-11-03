using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleManager : MonoBehaviour
{
    public static BubbleManager Instance { get; private set; }

    [SerializeField] private Transform bubbleChain;
    [SerializeField] private Transform bubbleBullets;
    [SerializeField] private Transform bubbleSprite;
    [SerializeField] private Transform bubbleBulletPrefab;
    [SerializeField] private Canvas canvas;    
    [SerializeField] private bool isRandomChain;
    [SerializeField] private List<Bubble> bubbleList;
    [SerializeField] private List<BubbleBullet> bubbleShootList;

    private Transform bubbleBulletLoop;

    private void Awake()
    {
        Instance = this;        
    }

    private void Start()
    {
        Bubble.OnAnyBubbleSpawn += Bubble_OnAnyBubbleSpawn;
        Bubble.OnAnyBubbleDead += Bubble_OnAnyBubbleDead;
        LevelGrid.OnAnyUnitGetRemoved += LevelGrid_OnAnyUnitGetRemoved;
        BubbleBullet.OnAnyBubbleBulletDestroy += BubbleBullet_OnAnyBubbleBulletDestroy;

        SetBubbleList();
        SetBubbleBullets();
        SetBubbleChain();
        SetLoopBubbleBullets();
    }

    private void Bubble_OnAnyBubbleSpawn(object sender, EventArgs e)
    {
        Bubble bubble = sender as Bubble;
        bubbleList.Add(bubble);
        CheckBubblesPosition();
    }

    private void Bubble_OnAnyBubbleDead(object sender, EventArgs e)
    {
        Bubble bubble = sender as Bubble;
        bubbleList.Remove(bubble);
        CheckBubblesCount();
        Debug.Log("Bullets GO count = " + bubbleBullets.childCount);
    }

    private void BubbleBullet_OnAnyBubbleBulletDestroy(object sender, EventArgs e)
    {
        Debug.Log("Bubble Shot");       
        RemoveBubbleBulletFromWorld(bubbleBullets.GetChild(0).gameObject);
        SetBubbleBullets();
        SetBubbleChain();
        CheckChain();
        Debug.Log("BULLETSHOT_EVENT_END");
        Debug.Log("Bullets GO count = " + bubbleBullets.childCount);
    }

    private void LevelGrid_OnAnyUnitGetRemoved(object sender, EventArgs e)
    {
        Bubble bubble = sender as Bubble;
        bubbleList.Remove(bubble);        
    }

    private void SetBubbleList()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int y = 0; y < LevelGrid.Instance.GetHeight(); y++)
            {
                GridPosition gridPosition = new GridPosition(x, y);
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(gridPosition))
                {
                    Bubble bubble = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
                    bubbleList.Add(bubble);
                }
            }
        }
    }

    public List<Bubble> GetBubbleList()
    {
        return bubbleList;
    }

    private void SetBubbleBullets()
    {
        bubbleShootList.Clear(); Debug.Log("BubbleShootList Clear"); Debug.Log("BubbleShootList Count = " + bubbleShootList.Count);

        int childCount = bubbleBullets.childCount;
        Debug.Log("ChildCount" + childCount); 

        for (int i = 0; i < childCount; i++)
        {
            bubbleShootList.Add(bubbleBullets.GetChild(i).GetComponent<BubbleBullet>());
            Debug.Log("Added bullet in bubbleShootList");
        }        
    }

    private void SetLoopBubbleBullets()
    {
        bubbleBulletLoop = Instantiate(bubbleBullets, bubbleBullets.position, Quaternion.identity);

        for (int i = 0; i < bubbleBulletLoop.childCount; i++)
        {
            bubbleBulletLoop.transform.GetChild(i).GetComponent<BubbleBullet>().SetColor(bubbleBullets.GetChild(i).GetComponent<BubbleBullet>().GetColor());
            Debug.Log("Added bullet in BulletsLoop");
        }
    }


    private void RemoveBubbleBulletFromWorld(GameObject bullet)
    {
        if (bubbleBullets.childCount != 0)
        
        bubbleShootList.RemoveAt(0); Debug.Log("Removed from List");
        Debug.Log("Bullets GO count = " + bubbleBullets.childCount);
        Destroy(bullet); Debug.Log("Removed from World"); Debug.Log("Bullets GO count = " + bubbleBullets.childCount);
        Destroy(bubbleChain.GetChild(0).gameObject); Debug.Log("Removed from Chain"); Debug.Log("Bullets GO count = " + bubbleBullets.childCount);
    }

    private void AddBubbleBulletToWorld()
    {
        for (int i = 0; i < bubbleBulletLoop.childCount; i++)
        {
            Transform bubbleBulletTransform = Instantiate(bubbleBulletLoop.transform.GetChild(i), bubbleBullets.position, Quaternion.identity, bubbleBullets);
            bubbleBulletTransform.GetComponent<BubbleBullet>().SetColor(bubbleBulletLoop.transform.GetChild(i).GetComponent<BubbleBullet>().GetColor());
        }

        SetBubbleBullets();
        SetBubbleChain();        
    }

    private void SetBubbleChain()
    {
        ClearChain();
        Debug.Log("Cleared Chain");

        int bubbleBulletsCount = bubbleShootList.Count;

        Debug.Log("bubbleShootList has childs = " + bubbleBulletsCount);

        for (int i = 0; i < bubbleBulletsCount; i++)
        {
            Transform bubbleSpriteTransform = Instantiate(bubbleSprite, bubbleChain.position, Quaternion.identity, bubbleChain);
            Debug.Log("Created in chain: " + bubbleSpriteTransform.gameObject + " As child #" + i);
            Debug.Log("Color of ref is " + bubbleShootList[i].GetComponent<SpriteRenderer>().color);
            bubbleSpriteTransform.GetComponent<Image>().color = bubbleShootList[i].GetComponent<SpriteRenderer>().color;
            Debug.Log("Color of Sprite#" + i + " is " + bubbleSpriteTransform.GetComponent<Image>().color);
        }
    }

    private void ClearChain()
    {
        Debug.Log("Cleaning chain");
        int bubbleCountInChain = bubbleChain.childCount;
        for (short i = 0; i < bubbleCountInChain; i++)
        {            
            Destroy(bubbleChain.GetChild(i).gameObject);
            Debug.Log("Cleaning Child #" + i);
        }
    }

    public void CleanBubbleChain()
    {
        Destroy(bubbleChain.GetChild(0).gameObject);
    }

    private void CheckBubblesPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            GridPosition gridPosition = new GridPosition(x, 0);
            if (LevelGrid.Instance.HasAnyUnitOnGridPosition(gridPosition)) GameOver();
        }
    }

    private void RandomAddToChain()
    {
        Instantiate(bubbleBulletPrefab, bubbleChain.position, Quaternion.identity, bubbleBullets);
    }

    private void CheckChain()
    {
        if (bubbleBullets.childCount < 3)
        {
            if (isRandomChain)
            {
                RandomAddToChain();
                SetBubbleChain();
            }
            else
            {
                AddBubbleBulletToWorld();
                SetBubbleChain();
            }            
        }
    }

    private void CheckBubblesCount()
    {
        if (bubbleList.Count == 0) GameOver();
    }

    private void GameOver()
    {
        canvas.GetComponent<PauseLogic>().GameOver();
    }
}
