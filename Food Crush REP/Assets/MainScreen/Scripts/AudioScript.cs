using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioScript : MonoBehaviour
{
    [SerializeField] AudioSource ButtonClickSound;
    [SerializeField] AudioSource ElementsMatchSound;
    [SerializeField] AudioSource ElementsMoveSound;
    [SerializeField] AudioSource BombMadeSound;


    [SerializeField] AudioMixer MusicMixer;
    [SerializeField] Slider MusicSlider;

    [SerializeField] AudioMixer SoundMixer;
    [SerializeField] Slider SoundSlider;

    private float SoundVolume;
    private float MusicVolume;
    private void Start()
    {
        SetSlidersValue();

    }
    IEnumerator WaitSoundOnStart()
    {
        yield return new WaitForSeconds(0.5f);
        ButtonClickSound.volume = 1;

    }
    public void ButtonClickedAudio()
    {
        if (!ButtonClickSound.isPlaying)
        {
            ButtonClickSound.Play();
        }
    }
    public void SetMusicVolume()
    {
        MusicVolume = MusicSlider.value;
        MusicMixer.SetFloat("Music", MusicVolume);
    }
    public void SetSoundVolume()
    {
        SoundVolume = SoundSlider.value;
        SoundMixer.SetFloat("Buttons", SoundVolume);
    }
    public void PlayElementCombinedAudio()
    {
        ElementsMatchSound.Play();
    }
    public void PlayElementMovedAudio()
    {
        ElementsMoveSound.Play();
    }
    public void BombCreationAudio()
    {
        BombMadeSound.Play();

    }
    public void SavePlayerData()
    {
        PlayerPrefs.SetFloat("MusicLevel", MusicVolume);
        PlayerPrefs.SetFloat("SoundLevel", SoundVolume);
        PlayerPrefs.Save();
    }
    public void SetSlidersValue()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicLevel");
        SoundSlider.value = PlayerPrefs.GetFloat("SoundLevel");
        StartCoroutine(WaitSoundOnStart());
    }

}
