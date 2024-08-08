using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public float radius = 5.0f; // Радиус кругового движения
    public float speed = 1.0f; // Скорость движения
    public float linearSpeed = 5.0f; // Линейная скорость движения от центра

    private Vector3 center; // Центр кругового движения
    private float angle; // Текущий угол
    private bool isCircularMotion = true; // Флаг для определения режима движения

    void Start()
    {
        center = new Vector3(0, 0, 0); // Устанавливаем центр сцены как центр кругового движения
    }

    void Update()
    {
        if (isCircularMotion)
        {
            CircularMove();
        }
        else
        {
            LinearMove();
        }

        if (Input.GetMouseButtonDown(0))
        {
            isCircularMotion = false;
        }
    }

    void CircularMove()
    {
        angle += speed * Time.deltaTime; // Обновляем угол в зависимости от скорости и времени

        float x = center.x + Mathf.Cos(angle) * radius; // Вычисляем новую координату x
        float y = center.y + Mathf.Sin(angle) * radius; // Вычисляем новую координату y

        transform.position = new Vector3(x, y, transform.position.z); // Обновляем позицию объекта
    }

    void LinearMove()
    {
        Vector3 direction = (transform.position - center).normalized; // Направление движения от центра
        transform.position += direction * linearSpeed * Time.deltaTime; // Обновляем позицию объекта
    }
}