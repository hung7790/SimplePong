using UnityEngine;
using System.Collections;

public class ComputerPlayerController : MonoBehaviour, IPlayerController
{
    int fieldHeight = 80;
    Ball Ball;
    Paddle Paddle;
    float delta = 1f;
    float normalPositionY;
    int top;
    int down;
    void Start()
    {
        Ball = FindObjectOfType<Ball>();
        Paddle = this.GetComponent<Paddle>();
        normalPositionY = Paddle.startPosition.y;
        top = Mathf.RoundToInt(normalPositionY + fieldHeight / 2);
        down = Mathf.RoundToInt(normalPositionY - fieldHeight / 2);
    }
    float TimeForBallArrive()
    {
        return h_DistanceToBall() / Ball.GetBallVelocity().x;
    }
    float h_DistanceToBall()
    {
        return Mathf.Abs((Ball.GetBallPosition().x - transform.position.x));
    }
    float v_DistanceToBall()
    {
        return Ball.GetBallPosition().y - transform.position.y;
    }
    //calculate the arriving position
    float PredictArrivePositionY()
    {
        int predictPositionY = Mathf.RoundToInt((Ball.GetBallPosition().y + TimeForBallArrive() * Ball.GetBallVelocity().y));
        int numberOfBound = predictPositionY / fieldHeight;
        int boundRemainder = predictPositionY % fieldHeight;
        if (numberOfBound % 2 == 0)
        {
            predictPositionY = boundRemainder;
        }
        else
        {
            predictPositionY = -boundRemainder;
        }
        if (predictPositionY < down)
        {
            predictPositionY = down * 2 - predictPositionY;
        }
        else if (predictPositionY > top)
        {
            predictPositionY = top * 2 - predictPositionY;
        }
        return predictPositionY;
    }
    float targetPosition;
    void SetTargetPositionY()
    {
        //Make it don't be too smart
        if (Mathf.Abs(targetPosition - PredictArrivePositionY()) > 12)
        {
            targetPosition = Random.value + PredictArrivePositionY();
        }
    }
    public Vector2 GetInput()
    {
        if (Ball.GetBallVelocity().x > 0)
        {
            SetTargetPositionY();
            if (Mathf.Abs(targetPosition - transform.position.y) / Paddle.speed + Random.value > TimeForBallArrive())
            {
                if (targetPosition > transform.position.y + delta)
                {
                    return Vector2.up;
                }
                else if (targetPosition < transform.position.y - delta)
                {
                    return Vector2.down;
                }
            }
        }
        return Vector2.zero;
    }
}
