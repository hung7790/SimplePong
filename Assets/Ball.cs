using UnityEngine;
using System.Collections;

public class Ball : Moveable
{
    public float initalSpeed = 40;
    public float currentSpeed;
    public float boundFactor = 1.5f;
    public Vector2 GetBallPosition()
    {
        return transform.position;
    }
    public Vector2 GetBallVelocity()
    {
        return Rigidbody2D.velocity;
    }
    public void SetVelocity()
    {
        Rigidbody2D.velocity =  new Vector2(-0.5f,0.5f).normalized * initalSpeed;
    }
    void SetVelocity(Vector2 Velocity)
    {
        if (!GameManager.GamePaused && GameManager.GameStarted)
        {
            Rigidbody2D.velocity = Velocity;
        }
    }
    Vector2 GetReboundDirection(Collision2D col)
    {
        if (Rigidbody2D.velocity.x > 0)
        {
            return new Vector2(1, (transform.position.y - col.transform.position.y) / col.collider.bounds.size.y).normalized;
        }
        else
        {
            return new Vector2(-1, (transform.position.y - col.transform.position.y) / col.collider.bounds.size.y).normalized;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Paddle")
        {
            Rigidbody2D.velocity = GetReboundDirection(col) * Rigidbody2D.velocity.magnitude * boundFactor;
            GameManager.SoundManager.SoundEvent(0);
        }
        else if (col.gameObject.name == "LeftBar")
        {
            GameManager.Score(1);
        }
        else if (col.gameObject.name == "RightBar")
        {
            GameManager.Score(0);
        }
    }
    public void SetCurrentSpeed()
    {
        currentSpeed = Rigidbody2D.velocity.magnitude;
    }
}
