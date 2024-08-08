using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTargetSelector : MonoBehaviour
{
    public GameObject[] targets; // Массив мишеней

    void Start()
    {
        SelectRandomTarget();
    }

    void SelectRandomTarget()
    {
        if (targets.Length == 0)
        {
            Debug.LogError("No targets assigned!");
            return;
        }

        // Выбираем случайную мишень
        int randomIndex = Random.Range(0, targets.Length);
        GameObject selectedTarget = targets[randomIndex];

        // Красим выбранную мишень в красный цвет
        SpriteRenderer targetRenderer = selectedTarget.GetComponent<SpriteRenderer>();
        if (targetRenderer != null)
        {
            targetRenderer.color = Color.red;
        }
        else
        {
            Debug.LogError("Selected target does not have a SpriteRenderer component!");
        }
    }
}