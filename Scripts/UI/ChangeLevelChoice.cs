using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevelChoice : MonoBehaviour
{
    private GameObject pressedButtonRef;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void setGameObject(GameObject gameObject)
    {
        this.pressedButtonRef = gameObject;
    }

    public void PositiveChoice()
    {
        pressedButtonRef.GetComponent<Button_ChangeLevel>().ChangeLevel();
    }

    public void NegativeChoice()
    {
        pressedButtonRef.GetComponent<Button_ChangeLevel>().CloseChangeLevelMenu();
    }
}
