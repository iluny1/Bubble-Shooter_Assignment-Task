using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Button_ChangeLevel : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private Transform ChangeLevelChoice;
    [SerializeField] private TextMeshProUGUI ChoicePlate;
    [SerializeField] private string ChoicePlateText;



    public void ChangeLevelMenu()
    {
        GameObject gameObject = this.gameObject;
        ChangeLevelChoice.gameObject.SetActive(true);
        ChangeLevelChoice.GetComponent<ChangeLevelChoice>().setGameObject(gameObject);
        ChoicePlate.text = ChoicePlateText + "\n" + "\n" + levelName + "?";
    }

    public void CloseChangeLevelMenu()
    {
        ChangeLevelChoice.gameObject.SetActive(false);
        ChoicePlate.text = ChoicePlateText;
    }

    public void ChangeLevel()
    {
        SceneManager.LoadScene(levelName);
    }

}
