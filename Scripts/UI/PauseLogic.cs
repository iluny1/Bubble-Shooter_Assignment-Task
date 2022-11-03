using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseLogic : MonoBehaviour
{
    [SerializeField] private Transform Menu;
    [SerializeField] private Transform Background;
    [SerializeField] private Transform MenuButton;
    [SerializeField] private Transform SettingsMenu;
    [SerializeField] private Transform GameOverScreen;
    [SerializeField] private Settings settings;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle audioToggle;
    [SerializeField] private string sceneName;

    private bool isPaused = false;

    private void Awake()
    {
        settings.SetSettings(sceneName);
    }

    public void PauseMenuButtonPressed()
    {
        isPaused = true;
        MenuButton.gameObject.SetActive(false);
        Background.gameObject.SetActive(true);
        Menu.gameObject.SetActive(true);
    }

    public void UnpauseButtonPress()
    {
        isPaused = false;
        MenuButton.gameObject.SetActive(true);
        Background.gameObject.SetActive(false);
        Menu.gameObject.SetActive(false);
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }

    public void SettingsButtonPressed()
    {
        Menu.gameObject.SetActive(false);
        SettingsMenu.gameObject.SetActive(true);
    }

    public void SettingsBackButtonPressed()
    {
        SettingsMenu.gameObject.SetActive(false);
        Menu.gameObject.SetActive(true);
    }

    public void BackToMenuButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void BackToLevelChooseButtonPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void RestartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GetCurrentValues(float sliderValue, bool toggleValue)
    {
        volumeSlider.value = sliderValue;
        audioToggle.isOn = !toggleValue;
    }

    public void GameOver()
    {
        GameOverScreen.gameObject.SetActive(true);
    }

}
