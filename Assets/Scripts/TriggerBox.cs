using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MusicManager))]
public class TriggerBox : MonoBehaviour
{
    [SerializeField] GameObject _textToDisplay = null;
    [SerializeField] AudioSource _MusicToTrigger = null;
    [SerializeField] AudioClip _audioToTrigger = null;
    [SerializeField] GameObject _objectToTrigger = null;

    private AudioSource[] allAudioSources;

    public bool TextTrigger = false;
    public bool MusicTrigger = false;
    public bool AudioTrigger = false;
    public bool ObjectTrigger = false;
    public bool SceneTrigger = false;
    //-------Trigger Types---------//
    //If 1, text box trigger
    //If 2, music box trigger
    //If 3, audio box trigger
    //If 4, object box trigger
    // maybe more?
    //-----------------------------//

    [SerializeField] int SpecifyMusicType;
    //---------Music Types---------//
    //If 1, Main Menu
    //If 2, Intro
    //If 3, Level
    //If 4, Boss
    // maybe more?
    //-----------------------------//

    [SerializeField] MusicManager _musicManager;
    [SerializeField] GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (TextTrigger == true)
            {
                _textToDisplay.SetActive(true);
            }
            if (MusicTrigger == true)
            {
                StopAllAudio();
            }
            if (ObjectTrigger == true)
            {
                _objectToTrigger.SetActive(false);
            }
            if (SceneTrigger == true)
            {
                //add special effects like a fadeout before switching levels
                StartCoroutine(WaitToChangeScenes());
                _gameManager.LoadNextLevel();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TextTrigger == true)
        {
            _textToDisplay.SetActive(false);
        }
        if (MusicTrigger == true)
        {
            //_MusicToTrigger.Play();
            //_musicManager.MusicPlaying = SpecifyMusicType;
            _musicManager.DetectAudioPlaying(SpecifyMusicType);
            _musicManager.PlayMusic();
        }
        if (ObjectTrigger == true)
        {
            _objectToTrigger.SetActive(true);
        }
    }

    IEnumerator WaitToChangeScenes()
    {
        yield return new WaitForSeconds(3f);
    }
}
