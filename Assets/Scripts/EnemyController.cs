using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject m_Cake;
    [SerializeField] private float m_Speed = 1.0f;
    
    private Rigidbody2D m_Rigidbody;

    void Start()
    {
        m_Cake = GameObject.Find("/Cake");
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 directionToCake = m_Cake.transform.position - transform.position;
        directionToCake = directionToCake.normalized;// * m_Speed;
        Debug.Log(directionToCake);
        Debug.Log(directionToCake.magnitude);

        m_Rigidbody.velocity = directionToCake * Time.fixedDeltaTime;
    }
}
