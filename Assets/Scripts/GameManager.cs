using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject loseWindow;
    public BallController ballController; // Ссылка на экземпляр BallController
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
        ballController.ResetBall(); // Вызов метода для сброса состояния мяча
    }

    public void ExitMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        BallController.score = 0;
    }


}