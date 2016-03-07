using UnityEngine;
using System.Collections;

public class Paddle : Moveable
{
    public float speed = 40;
    public void SetVelocity(Vector2 v)
    {
        if (!GameManager.GamePaused && GameManager.GameStarted)
        {
            Rigidbody2D.velocity = v * speed;
        }
    }
}
