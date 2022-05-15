using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private GameObject m_EnemyPrefab;
    [SerializeField] private float m_EnemySpawnDistance = 10.0f;
    [SerializeField] private float m_EnemySpawnInterval = 3.0f;
    [SerializeField] private float m_SpawnOffsetFromGround = 0.1f;

    [Space]
    [SerializeField] private const int m_TimePerLevel = 60;
    [SerializeField] private List<Sprite> m_DigitTextures;
    [SerializeField] private Image m_TimerFirstDigit;
    [SerializeField] private Image m_TimerSecondDigit;

    private GameObject m_Frog;

    public float m_TimePassed;
    public float m_EnemyCooldown;

    void Start()
    {
        m_Frog = GameObject.Find("/Frog");
    }

    void Update()
    {
        m_TimePassed += 1.0f * Time.deltaTime;
        m_EnemyCooldown += 1.0f * Time.deltaTime;

        TrySpawningEnemy();
        
        Timer();
    }

    void TrySpawningEnemy()
    {
        if (m_EnemyCooldown < m_EnemySpawnInterval)
            return;

        m_EnemyCooldown = 0.0f;
        float enemySpawnOffset = Random.Range(m_SpawnOffsetFromGround, Mathf.PI - m_SpawnOffsetFromGround);

        Vector3 pos = new Vector3(Mathf.Cos(enemySpawnOffset), Mathf.Sin(enemySpawnOffset), 0.0f) * m_EnemySpawnDistance + m_Frog.transform.position;
        Instantiate(m_EnemyPrefab, pos, Quaternion.identity);
    }

    void Timer()
    {
        int leftTime = m_TimePerLevel - (int)m_TimePassed;
        if (leftTime < 0)
            return;

        int firstDigit = leftTime / 10;
        
        m_TimerFirstDigit.sprite = m_DigitTextures[firstDigit];

        int secondDigit = leftTime % 10;
        m_TimerSecondDigit.sprite = m_DigitTextures[secondDigit];
    }


    void OnDrawGizmos()
    {
        if (!m_Frog)
        {
            m_Frog = GameObject.Find("/Frog");
        }
        
        for (float i = m_SpawnOffsetFromGround; i < Mathf.PI - m_SpawnOffsetFromGround; i += 0.05f)
        {
            Vector3 pos = new Vector3(Mathf.Cos(i), Mathf.Sin(i), 0.0f) * m_EnemySpawnDistance + m_Frog.transform.position;
            Gizmos.DrawSphere(pos, 0.25f);
        }
        

    }
}
