using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public Canvas m_MainMenuUI;
    public Canvas m_InGameUI;
    public Canvas m_LostGameUI;
    public Canvas m_WonGameUI;

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
        
        instance.GetComponent<AudioSource>().Play();
    }

    static public bool IsGamePlaying()
    {
        return instance.m_State == States.Playing;
    }

}
