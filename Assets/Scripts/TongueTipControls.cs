using UnityEngine;

public class TongueTipControls : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1000.0f;
    
    [SerializeField] float speedBasedOnLengthStep = 1000.0f;

    Rigidbody2D m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!GameplayManager.IsGamePlaying() && !GameplayManager.IsGameWon() || GameplayManager.IsGameLost())
        {
            m_Rigidbody.velocity = Vector2.zero;
            return;
        }

        ManageInput();
    }

    void ManageInput()
    {
        Vector2 movementForce = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            movementForce.y += movementSpeed;
        if (Input.GetKey(KeyCode.A))
            movementForce.x += -movementSpeed;
        if (Input.GetKey(KeyCode.S))
            movementForce.y += -movementSpeed;
        if (Input.GetKey(KeyCode.D))
            movementForce.x += movementSpeed;

        // Limit diagonal boost movement
        if (movementForce.x != 0.0f && movementForce.y != 0.0f)
        {
            movementForce.x *= 0.8f;
            movementForce.y *= 0.8f;
        }

        m_Rigidbody.velocity = movementForce * Time.fixedDeltaTime;
    }

    public void IncreaseSpeedBasedOnLength()
    {
        movementSpeed += speedBasedOnLengthStep;
    }

    public void DecreaseSpeedBasedOnLength()
    {
        movementSpeed -= speedBasedOnLengthStep;
    }
}
