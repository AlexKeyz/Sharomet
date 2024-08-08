using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeField] public GameObject StartPanel;
    public int SpeedOffset;
    public int Distance;

    private bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 endPosition;

    void Start()
    {
        startPosition = StartPanel.transform.position;
        endPosition = startPosition - new Vector3(0, Distance, 0);
    }

    void Update()
    {
        if (isMoving)
        {
            StartPanel.transform.position = Vector3.MoveTowards(StartPanel.transform.position, endPosition, SpeedOffset * Time.deltaTime);
            if (StartPanel.transform.position == endPosition)
            {
                isMoving = false;
            }
        }
    }

    public void StartGame()
    {
        isMoving = true;
        StartCoroutine(HideStartPanelAfterDelay());
    }

    IEnumerator HideStartPanelAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Ожидаем 1 секунду
        StartPanel.SetActive(false); // Делаем StartPanel невидимой
    }
}