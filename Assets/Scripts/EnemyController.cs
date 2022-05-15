using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject m_Cake;
    [SerializeField] private List<AudioClip> m_StickingSounds;
    [SerializeField] private List<AudioClip> m_DeathSounds;
    [SerializeField] private float m_Speed = 1.0f;
    
    private Rigidbody2D m_Rigidbody;
    
    private bool m_IsStuck = false;

    void Start()
    {
        m_Cake = GameObject.Find("/Cake");
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!GameplayManager.IsGamePlaying())
        {
            m_Rigidbody.velocity = Vector2.zero;
            return;
        }

        MoveTowardsCake();
    }

    void MoveTowardsCake()
    {
        if (m_IsStuck)
            return;

        Vector3 directionToCake = m_Cake.transform.position - transform.position;
        directionToCake = directionToCake.normalized * m_Speed;

        m_Rigidbody.velocity = directionToCake * Time.fixedDeltaTime;
    }


    void StickThisToTongue(GameObject tongueSeg)
    {
        SpringJoint2D joint = tongueSeg.AddComponent<SpringJoint2D>();
        joint.connectedBody = GetComponent<Rigidbody2D>();
        
        joint.autoConfigureDistance = false;
        joint.distance = 0.5f;
        joint.dampingRatio = 1.0f;
        joint.frequency = 0.0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "PlayerTongue")
        {
            StickThisToTongue(collision.collider.gameObject);
            m_IsStuck = true;
            GameplayManager.PlayAudioClip(m_StickingSounds);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GameplayManager.PlayAudioClip(m_DeathSounds);
            Destroy(gameObject);
        }

        if (collider.gameObject.tag == "Cake")
        {

        }
    }
}
