using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text txtScore;

    public Text txtLife;
    public Text txtWin;
    public Text txtLose;

    private int scoreTotal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.scoreChangedEvent.AddListener(UpdateScore);
        GameManager.Instance.lifeChangedEvent.AddListener(UpdateLife);
        scoreTotal = GameManager.Instance.GetScoreTotal();
        SetScore();
        SetLife();
    }
    
    private void SetScore()
    {
        txtScore.text = String.Format("Score: {0}/{1}", GameManager.Instance.GetScore(), scoreTotal);
    }

    private void SetLife()
    {
        txtLife.text = String.Format("Life: {0}", GameManager.Instance.GetLife());
    }

    private void UpdateScore()
    {
        SetScore();
    }
    
    private void UpdateLife()
    {
        SetLife();
    }

    public void ShowWin(bool show)
    {
        txtWin.gameObject.SetActive(show);
    }
    
    public void ShowLose(bool show)
    {
        txtLose.gameObject.SetActive(show);
    }
}
