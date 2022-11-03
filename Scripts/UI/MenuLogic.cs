using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] private Transform MainMenu;
    [SerializeField] private Transform SettingsMenu;
    [SerializeField] private Transform NewGameMenu;
    [SerializeField] private Transform TitlePlate;
    [SerializeField] private Transform NewGamePlate;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle audioToggle;
    [SerializeField] private Settings settingsGameObject;
    private string sceneName = "Menu";

    private void Awake()
    {
        MainMenu.gameObject.SetActive(true);
        TitlePlate.gameObject.SetActive(true);
        SettingsMenu.gameObject.SetActive(false);

        settingsGameObject.SetSettings(sceneName);
    }

    public void NewGamePressed()
    {
        MainMenu.gameObject.SetActive(false); //Выключение главного меню
        TitlePlate.gameObject.SetActive(false); //Выключение таблички с названием
        NewGameMenu.gameObject.SetActive(true);//Включение меню выбора режима 
    }

    public void SettingsPressed()
    {
        MainMenu.gameObject.SetActive(false); //Выключение главного меню
        TitlePlate.gameObject.SetActive(false); //Выключение таблички с названием
        SettingsMenu.gameObject.SetActive(true); //Включение меню настроек
    }

    public void ExitPressed()
    {
        Application.Quit(); //Выход из приложения по нажатию кнопки
    }

    public void SettingsBackPressed()
    {
        MainMenu.gameObject.SetActive(true); //Включение главного меню
        TitlePlate.gameObject.SetActive(true); //Включение таблички с названием
        SettingsMenu.gameObject.SetActive(false); //Выключение меню настроек
    }

    public void NewGameMenuBackPressed()
    {
        NewGameMenu.gameObject.SetActive(false);
        NewGamePlate.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(true);
        TitlePlate.gameObject.SetActive(true);
    }

    public void LevelModePressed()
    {
        SceneManager.LoadScene(1);
    }

    public void InfinteModePressed()
    {
        SceneManager.LoadScene("InfiniteLevel");
    }

    public void GetCurrentValues(float sliderValue, bool toggleValue)
    {
        volumeSlider.value = sliderValue;
        audioToggle.isOn = !toggleValue;
    }
}
