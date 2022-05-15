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

    public void StartGame()
    {
        m_State = States.Playing;
        m_MainMenuUI.enabled = false;
        m_InGameUI.enabled = true;
    }

    public bool IsGamePlaying()
    {
        return m_State == States.Playing;
    }

}
