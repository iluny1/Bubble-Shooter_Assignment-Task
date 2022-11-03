using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    public void SetSettings(string sceneName)
    {
        audioMixer.GetFloat("masterVolume", out float value);

        switch (sceneName)
        {
            case "Menu":
                FindObjectOfType<Canvas>().transform.GetChild(0).GetComponent<MenuLogic>().GetCurrentValues(value, AudioListener.pause);
                Debug.Log(AudioListener.pause);
                break;
            case "LevelChoosing":
                GameObject CanvasObject = GameObject.Find("SceneCanvas");
                CanvasObject.GetComponent<PauseLogic>().GetCurrentValues(value, AudioListener.pause);
                Debug.Log(AudioListener.pause);
                break;
            default:
                break;
        }
    }

    public void ChangeVolume(float value)
    {
        audioMixer.SetFloat("masterVolume", value); //Выставление уровня громкости в соответствии с значением слайдера
    }

    public void ChangeAudioStatus(bool isSoundEnable)
    {
        if (isSoundEnable == true) //Если звук включен, то...
        {
            AudioListener.pause = false; //Ставим на паузу "слушателей"
        }
        else //Если нет, то...
        {
            AudioListener.pause = true; //Ставим с паузы слушателей
            AudioSource[] allAudio = FindObjectsOfType<AudioSource>(); //Находим все активные звуки 
            // Так как здесь только музыка, то алгоритм просто выключает все аудиотреки и включает 
            foreach (AudioSource audio in allAudio)
            {
                audio.Stop();
                audio.Play();
            }
        }
    }
}
