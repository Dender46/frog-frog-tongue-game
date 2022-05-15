using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private GameObject m_EnemyPrefab;
    [SerializeField] private float m_EnemySpawnDistance = 10.0f;
    [SerializeField] private float m_EnemySpawnInterval = 3.0f;

    private GameObject m_Frog;

    public float timeOffset;

    void Start()
    {
        m_Frog = GameObject.Find("/Frog");
    }

    void Update()
    {
        timeOffset += 1.0f * Time.deltaTime;
        if (timeOffset < m_EnemySpawnInterval)
            return;

        timeOffset = 0.0f;
        float enemySpawnOffset = Random.Range(0.1f, Mathf.PI - 0.1f);

        Vector3 pos = new Vector3(Mathf.Cos(enemySpawnOffset), Mathf.Sin(enemySpawnOffset), 0.0f) * m_EnemySpawnDistance + m_Frog.transform.position;
        Instantiate(m_EnemyPrefab, pos, Quaternion.identity);
    }
}
