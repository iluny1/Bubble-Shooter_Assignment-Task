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
        audioMixer.SetFloat("masterVolume", value); //����������� ������ ��������� � ������������ � ��������� ��������
    }

    public void ChangeAudioStatus(bool isSoundEnable)
    {
        if (isSoundEnable == true) //���� ���� �������, ��...
        {
            AudioListener.pause = false; //������ �� ����� "����������"
        }
        else //���� ���, ��...
        {
            AudioListener.pause = true; //������ � ����� ����������
            AudioSource[] allAudio = FindObjectsOfType<AudioSource>(); //������� ��� �������� ����� 
            // ��� ��� ����� ������ ������, �� �������� ������ ��������� ��� ���������� � �������� 
            foreach (AudioSource audio in allAudio)
            {
                audio.Stop();
                audio.Play();
            }
        }
    }
}
