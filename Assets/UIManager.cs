using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager GameManager;
    public Transform[] scoreParent;
    public GameObject scorePrefab;
    Sprite[] numberSpriteList;
    List<List<GameObject>> scoreGOList = new List<List<GameObject>>();
    public GameObject pauseMenu;
    public GameObject result;
    public Text resultText;
    public GameObject startButton;
    public void Initalize()
    {
        numberSpriteList = Resources.LoadAll<Sprite>("scoreSpriteSheet");
        while (scoreGOList.Count < scoreParent.Length)
        {
            scoreGOList.Add(new List<GameObject>());
        }
    }
    public void SetScoreUI(int[] score)
    {
        for (int i = 0; i < score.Length; i++)
        {

            string scoreString = score[i].ToString();
            {
                //remove unused digit
                if (scoreGOList[i].Count > scoreString.Length)
                {
                    for (int j = scoreString.Length; j < scoreGOList[i].Count; j++)
                    {
                        Destroy(scoreGOList[i][j]);
                    }
                    scoreGOList[i].RemoveRange(scoreString.Length, scoreGOList[i].Count - scoreString.Length);
                }
                //load image of each digit one by one
                for (int j = 0; j < scoreString.Length; j++)
                {
                    if (j == scoreGOList[i].Count)
                    {
                        GameObject scoreGameObject = Instantiate<GameObject>(scorePrefab);
                        scoreGameObject.GetComponent<Transform>().parent = scoreParent[i];
                        scoreGameObject.transform.localPosition = new Vector2(j * 5, 0);
                        scoreGOList[i].Add(scoreGameObject);
                    }
                    scoreGOList[i][j].GetComponent<SpriteRenderer>().sprite = numberSpriteList[int.Parse(scoreString[j].ToString())];

                }
            }
        }
    }
    public void EnablePauseMenu(bool enable)
    {
        pauseMenu.SetActive(enable);
    }
    public void ShowResultText(bool win)
    {
        StartCoroutine(Result(win));
    }
    public void EnableStartButton(bool enable)
    {
        startButton.SetActive(enable);
    }
    IEnumerator Result(bool win)
    {
        result.SetActive(true);
        if (win)
        {
            resultText.text = "You Win!";
        }
        else
        {
            resultText.text = "You lose!";
        }
        yield return new WaitForSeconds(2);
        result.SetActive(false);
        GameManager.ResetMatch();
    }



}
