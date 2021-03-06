using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private GameObject m_EnemyPrefab;
    [SerializeField] private float m_EnemySpawnDistance = 10.0f;
    [SerializeField] private float m_EnemySpawnInterval = 3.0f;
    [SerializeField] private float m_SpawnOffsetFromGround = 0.1f;
    [SerializeField] private float m_SpawnHardnessMulti = 0.6f;

    [Space(10)]
    [SerializeField] private List<Sprite> m_DigitTextures;
    [SerializeField] private int m_TimePerLevel = 60;
    [SerializeField] private Image m_TimerFirstDigit;
    [SerializeField] private Image m_TimerSecondDigit;
    [SerializeField] private Image m_TimerThirdDigit;

    private GameObject m_Frog;
    private GameObject m_Cake;

    public float m_TimePassed;
    public float m_EnemyCooldown;

    void Start()
    {
        m_Frog = GameObject.Find("/Frog");
        m_Cake = GameObject.Find("/Cake");
    }

    void Update()
    {
        if (!GameplayManager.IsGamePlaying() || GameplayManager.IsGameWon())
            return;

        m_TimePassed += 1.0f * Time.deltaTime;
        m_EnemyCooldown += 1.0f * Time.deltaTime;

        Color color = Color.HSVToRGB(0.0f, (1.0f - m_TimePassed / m_TimePerLevel) * 0.4f, 1.0f);
        m_Cake.GetComponent<SpriteRenderer>().color = color;

        TrySpawningEnemy();
        
        Timer();

        if (m_TimePassed >= m_TimePerLevel)
        {
            GameplayManager.WinGame();
        }
    }

    void TrySpawningEnemy()
    {
        bool isHarder = (m_TimePassed / m_TimePerLevel) > 0.15f;
        float spawnInterval = isHarder ? m_EnemySpawnInterval * m_SpawnHardnessMulti : m_EnemySpawnInterval;
        
        if (m_EnemyCooldown < spawnInterval)
            return;

        m_EnemyCooldown = 0.0f;
        float enemySpawnOffset = Random.Range(m_SpawnOffsetFromGround, Mathf.PI - m_SpawnOffsetFromGround);

        Vector3 pos = new Vector3(Mathf.Cos(enemySpawnOffset), Mathf.Sin(enemySpawnOffset), 0.0f) * m_EnemySpawnDistance + m_Frog.transform.position;
        GameObject newEnemy = Instantiate(m_EnemyPrefab, pos, Quaternion.identity);
        newEnemy.transform.parent = GameObject.Find("/EnemyList").transform;
    }

    void Timer()
    {
        int leftTime = m_TimePerLevel - (int)m_TimePassed;
        if (leftTime < 0)
            return;

        int firstDigit = leftTime / 100;
        
        m_TimerFirstDigit.sprite = m_DigitTextures[firstDigit];

        int secondDigit = (leftTime / 10) % 10;
        m_TimerSecondDigit.sprite = m_DigitTextures[secondDigit];

        int thirdDigit = leftTime % 10;
        m_TimerThirdDigit.sprite = m_DigitTextures[thirdDigit];
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
