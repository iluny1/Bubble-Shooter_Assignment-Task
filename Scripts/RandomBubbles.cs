using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBubbles : MonoBehaviour
{
    [SerializeField] private Transform bubbleFolder;
    [SerializeField] private Transform bubblePrefab;

    private void Awake()
    {
        int height = Random.Range(3, LevelGrid.Instance.GetHeight() - 1);

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int y = height; y < LevelGrid.Instance.GetHeight(); y++)
            {
                Vector2 position = LevelGrid.Instance.GetWorldPosition(new GridPosition(x, y));
                Instantiate(bubblePrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
