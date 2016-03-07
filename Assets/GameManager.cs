using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public interface IPlayerController
{
    Vector2 GetInput();
}
public abstract class Moveable : MonoBehaviour
{
    public Vector2 startPosition;
    public Vector2 storedPosition;
    protected Vector2 storedVelocity;
    protected Rigidbody2D Rigidbody2D;
    public GameManager GameManager;
    void Start()
    {
        startPosition = this.transform.localPosition;
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    public void ResetPositionAndVelocity()
    {
        this.transform.localPosition = startPosition;
        Rigidbody2D.velocity = Vector2.zero;
    }
    public void ClearStored()
    {
        storedPosition = Vector2.zero;
        storedVelocity = Vector2.zero;
    }
    public void Pause(bool pause)
    {

        if (pause)
        {
            storedPosition = this.transform.localPosition;
            storedVelocity = Rigidbody2D.velocity;
            Rigidbody2D.velocity = Vector2.zero;
        }
        else
        {
            this.transform.localPosition = storedPosition;
            Rigidbody2D.velocity = storedVelocity;
        }

    }
}

public class GameManager : MonoBehaviour
{
    public int scoreToWin = 20;
    public bool GamePaused { get; private set; }
    public bool GameStarted { get; internal set; }
    public int[] score = new int[2];
    public List<GameObject> paddleGOList = new List<GameObject>(2);
    List<Paddle> paddleList = new List<Paddle>();
    List<IPlayerController> playerList = new List<IPlayerController>();
    List<Moveable> moveableList = new List<Moveable>();
    public Ball Ball;
    [HideInInspector]
    public UIManager UIManager;
    [HideInInspector]
    public SoundManager SoundManager;

    // Program initalization, get all reference needed
    void Initalize()
    {
        for (int i = 0; i < paddleGOList.Count; i++)
        {
            if (paddleGOList[i] != null)
            {
                Paddle paddle = paddleGOList[i].GetComponent<Paddle>();
                paddle.GameManager = this;
                moveableList.Add(paddle);
                if (paddle != null)
                {
                    IPlayerController player = paddleGOList[i].GetComponent<IPlayerController>();
                    if (player != null)
                    {
                        playerList.Add(player);
                    }
                    else
                    {
                        Debug.LogError("Please Add player controller to GameObject");
                        continue;
                    }
                    paddleList.Add(paddle);
                }
                else
                {
                    Debug.LogError("Please Add Paddle to GameObject");
                }
            }
            if (Ball == null)
            {
                Ball = FindObjectOfType<Ball>();
                if (Ball != null)
                {
                    Ball.GameManager = this;
                    moveableList.Add(Ball);
                }
                else
                {
                    Debug.LogError("Please Add a Ball");
                }
            }
            UIManager = FindObjectOfType<UIManager>();
            UIManager.GameManager = this;
            UIManager.Initalize();
            if (Ball == null)
            {
                Debug.LogError("Please Add a UIManager");
            }
            UIManager.SetScoreUI(score);
            SoundManager = FindObjectOfType<SoundManager>();
        }
    }
    //Reset all
    public void ResetMatch()
    {
        PauseGame(false);
        GameStarted = false;
        UIManager.EnablePauseMenu(false);
        ResetGame();
        score = new int[2];
        UIManager.SetScoreUI(score);
        UIManager.EnableStartButton(true);
    }
    //Reset game without reset score
    public void ResetGame()
    {
        PauseGame(false);
        GameStarted = false;
        UIManager.EnablePauseMenu(false);
        foreach (Moveable obj in moveableList)
        {
            obj.ResetPositionAndVelocity();
            obj.ClearStored();
        }
        UIManager.EnableStartButton(true);
    }
    //Start a new round
    public void GameStart()
    {
        GameStarted = true;
        UIManager.EnableStartButton(false);
        Ball.SetVelocity();
    }
    public void PauseGame(bool pause)
    {
        if (GamePaused == pause) return;
        GamePaused = pause;
        foreach (Moveable obj in moveableList)
        {
            obj.Pause(pause);
        }
    }
    void Start()
    {
        Initalize();
        GamePaused = false;
        GameStarted = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameStarted)
            {
                PauseGame(true);
                UIManager.EnablePauseMenu(true);
            }
        }
    }
    void FixedUpdate()
    {
        if (GameStarted && !GamePaused)
        {
            Ball.SetCurrentSpeed();
            for (int i = 0; i < paddleList.Count; i++)
            {
                paddleList[i].SetVelocity(playerList[i].GetInput());
            }
        }
    }
    //left side score = 0 right side score = 1;
    public void Score(int side)
    {
        GameStarted = false;
        score[side]++;
        UIManager.SetScoreUI(score);
        if (score[side] >= scoreToWin)
        {
            PauseGame(true);
            if (side == 0)
            {
                UIManager.ShowResultText(true);
                SoundManager.SoundEvent(3);
            }
            else if (side == 1)
            {
                UIManager.ShowResultText(false);
                SoundManager.SoundEvent(4);
            }
        }
        else
        {
            if (side == 0)
            {

                SoundManager.SoundEvent(1);
            }
            else if (side == 1)
            {
                SoundManager.SoundEvent(2);
            }
            ResetGame();
        }
    }
}
