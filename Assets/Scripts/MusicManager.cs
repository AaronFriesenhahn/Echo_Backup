using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource MainMenuMusic;
    [SerializeField] AudioSource IntroMusic;
    [SerializeField] AudioSource LevelMusic;
    [SerializeField] AudioSource BossMusic;

    [SerializeField] GameManager _gameManager;

    public bool MainMenuMusicOn = false;
    public bool IntroLevelMusicOn = false;
    public bool LevelMusicOn = false;
    public bool BossMusicOn = false;

    public int MusicPlaying;
    //1 = main music
    //2 = intro music
    //3 = level music
    //4 = boss music
    public bool _musicCurrentlyPlaying;

    //testing playonce stuff
    int x = 0;


    private AudioSource[] allAudioSources;

    void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }

    public void DetectAudioPlaying(int value)
    {
        if (value == 1)
        {
            MainMenuMusicOn = true;
            MusicPlaying = 1;
            Debug.Log("Playing menu music.");
        }
        else if (value == 2)
        {
            IntroLevelMusicOn = true;
            MusicPlaying = 2;
            Debug.Log("Playing intro music.");
        }
        else if (value == 3)
        {
            LevelMusicOn = true;
            MusicPlaying = 3;
            Debug.Log("Playing level music.");
        }
        else if (value == 4)
        {
            BossMusicOn = true;
            MusicPlaying = 4;
            Debug.Log("Playing boss music.");
        }
    }

    public void PlayMusic()
    {
        if (GameManager.GameIsPaused == false)
        {
            if (MusicPlaying == 2)
            {
                StopAllAudio();
                IntroMusic.Play();
                MainMenuMusicOn = false;
                IntroLevelMusicOn = true;
                LevelMusicOn = false;
                BossMusicOn = false;
                _musicCurrentlyPlaying = true;
            }
            else if (MusicPlaying == 3)
            {
                StopAllAudio();
                LevelMusic.Play();
                MainMenuMusicOn = false;
                IntroLevelMusicOn = false;
                LevelMusicOn = true;
                BossMusicOn = false;
                _musicCurrentlyPlaying = true;
            }
            else if (MusicPlaying == 4)
            {
                StopAllAudio();
                BossMusic.Play();
                MainMenuMusicOn = false;
                IntroLevelMusicOn = false;
                LevelMusicOn = false;
                BossMusicOn = true;
                _musicCurrentlyPlaying = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameIsPaused == true)
        {
            x = 0;
            if (MainMenuMusicOn == false)
            {
                StopAllAudio();
                Debug.Log("Play Main Menu Music.");
                MainMenuMusic.Play();
                MainMenuMusicOn = true;
                IntroLevelMusicOn = false;
                LevelMusicOn = false;
                BossMusicOn = false;
            }
        }
        if (GameManager.GameIsPaused == false)
        {
            if (x == 0)
            {
                PlayMusic();
                x = 1;
            }
        }
    }
}
