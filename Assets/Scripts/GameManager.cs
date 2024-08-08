using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject loseWindow;
    public BallController ballController; // ������ �� ��������� BallController
    [SerializeField] public GameObject StartPanel;


    void Start()
    {
        
    }

    void Update()
    {

    }

    public void Restart()
    {
        loseWindow.SetActive(false);
        BallController.score = 0;
        ballController.ResetBall(); // ����� ������ ��� ������ ��������� ����
    }

    public void ExitMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        BallController.score = 0;
    }


}