using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public float radius = 5.0f; // ������ ��������� ��������
    public float speed = 1.0f; // �������� ��������
    public float linearSpeed = 5.0f; // �������� �������� �������� �� ������

    private Vector3 center; // ����� ��������� ��������
    private float angle; // ������� ����
    private bool isCircularMotion = true; // ���� ��� ����������� ������ ��������

    void Start()
    {
        center = new Vector3(0, 0, 0); // ������������� ����� ����� ��� ����� ��������� ��������
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
        angle += speed * Time.deltaTime; // ��������� ���� � ����������� �� �������� � �������

        float x = center.x + Mathf.Cos(angle) * radius; // ��������� ����� ���������� x
        float y = center.y + Mathf.Sin(angle) * radius; // ��������� ����� ���������� y

        transform.position = new Vector3(x, y, transform.position.z); // ��������� ������� �������
    }

    void LinearMove()
    {
        Vector3 direction = (transform.position - center).normalized; // ����������� �������� �� ������
        transform.position += direction * linearSpeed * Time.deltaTime; // ��������� ������� �������
    }
}