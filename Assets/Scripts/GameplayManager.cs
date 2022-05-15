using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public Canvas m_MainMenuUI;
    public Canvas m_InGameUI;
    public Canvas m_LostGameUI;
    public Canvas m_WonGameUI;
    
    [Space(10)]
    public AudioClip m_ButtonClick;

    enum States
    {
        MainMenu,
        Playing,
        Won,
        Lost
    }

    private States m_State = States.MainMenu;

    static public GameplayManager instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Instance already exists, destroying 'this'!");
            Destroy(this);
        }
    }

    void Update()
    {
        switch (m_State)
        {
            case States.MainMenu:
                break;
            case States.Playing:
                break;
            case States.Won:
                break;
            case States.Lost:
                break;
        }
    }

    static public void StartGame()
    {
        instance.m_State = States.Playing;
        instance.m_MainMenuUI.gameObject.SetActive(false);
        instance.m_InGameUI.gameObject.SetActive(true);
        
        instance.GetComponent<AudioSource>().PlayOneShot(instance.m_ButtonClick);
    }

    static public void WinGame()
    {
        if (instance.m_State == States.Won)
            return;

        instance.m_State = States.Won;
        GameObject.Find("/CakeTable").GetComponent<Collider2D>().isTrigger = false;
        GameObject.Find("/Cake").GetComponent<Collider2D>().isTrigger = false;
        GameObject.Find("/Cake").GetComponent<Rigidbody2D>().isKinematic = false;

        instance.m_WonGameUI.gameObject.SetActive(true);
    }

    static public void LoseGame()
    {

    }

    static public bool IsGamePlaying()
    {
        return instance.m_State == States.Playing;
    }

    static public bool IsGameWon()
    {
        return instance.m_State == States.Won;
    }

    static public void PlayAudioClip(AudioClip clip)
    {
        instance.GetComponent<AudioSource>().PlayOneShot(clip);
    }

    static public void PlayAudioClip(List<AudioClip> clips)
    {
        instance.GetComponent<AudioSource>().PlayOneShot(clips[Random.Range(0, clips.Count-1)]);
    }

    static public void OpenMinijamWebsite()
    {
        Application.OpenURL("https://itch.io/jam/mini-jam-106-frogs");
    }

}
