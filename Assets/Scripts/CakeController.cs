using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CakeController : MonoBehaviour
{
    [SerializeField] private List<AudioClip> m_StickingSounds;
    [SerializeField] private List<AudioClip> m_DeathSounds;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "PlayerTongue")
        {
            StickThisToTongue(collision.collider.gameObject);
            GameplayManager.PlayAudioClip(m_StickingSounds);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GameplayManager.PlayAudioClip(m_DeathSounds);
            Destroy(gameObject);
            GameplayManager.RestartGame(10.0f);
        }
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
}
