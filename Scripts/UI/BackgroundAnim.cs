using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundAnim : MonoBehaviour
{
    [SerializeField] private Transform Background;
    [SerializeField] private float moveSpeed;

    private float currentX;
    private float currentY;
    private const int MAX_X_POS = 600;
    private const int MIN_X_POS = -600;
    private const int MAX_Y_POS = 330;
    private const int MIN_Y_POS = -330;
    private bool positiveDirectionX;
    private bool positiveDirectionY;

    private void Awake()
    {
        currentX = Background.localPosition.x;
        currentY = Background.localPosition.y;
        positiveDirectionX = true;
        positiveDirectionY = true;
    }

    private void Update()
    {
        CheckPosition();
        MoveBackground();
    }

    private void CheckPosition()
    {
        if (positiveDirectionX)
        {
            if (currentX > MAX_X_POS)
            {
                positiveDirectionX = false;
            }
        }

        if (!positiveDirectionX)
        {
            if (currentX < MIN_X_POS)
            {
                positiveDirectionX = true;
            }
        }

        if (positiveDirectionY)
        {
            if (currentY > MAX_Y_POS)
            {
                positiveDirectionY = false;
            }
        }

        if (!positiveDirectionY)
        {
            if (currentY < MIN_Y_POS)
            {
                positiveDirectionY = true;
            }
        }
    }

    private void MoveBackground()
    {
        if (positiveDirectionX)
        {
            currentX += moveSpeed * Time.deltaTime;
            Background.localPosition = new Vector2(currentX, currentY);
        }

        if (!positiveDirectionX)
        {
            currentX -= moveSpeed * Time.deltaTime;
            Background.localPosition = new Vector2(currentX, currentY);
        }

        if (positiveDirectionY)
        {
            currentY += moveSpeed * Time.deltaTime;
            Background.localPosition = new Vector2(currentX, currentY);
        }

        if (!positiveDirectionY)
        {
            currentY -= moveSpeed * Time.deltaTime;
            Background.localPosition = new Vector2(currentX, currentY);
        }
    }
}
