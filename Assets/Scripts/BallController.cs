using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class BallController : MonoBehaviour
{
    public float radius = 1.0f; // ������ ��������� ��������
    public float speed = 2.0f; // �������� ��������
    public float linearSpeed = 5.0f; // �������� �������� �������� �� ������
    public GameObject[] targets; // ������ �������
    public GameObject loseWindow; // ������ LoseWindow
    public ParticleSystem successParticlesPrefab; // ������ ������� ������ ��� ��������� ���������
    public ParticleSystem failParticlesPrefab; // ������ ������� ������ ��� ���������
    public AudioSource hitAudioSource; // AudioSource ��� ����� ���������
    public AudioSource missAudioSource; // AudioSource ��� ����� �������
    public GameObject menuPanel; // ������ �� ������ MenuPanel
    public Text textScore; // ������ �� ��������� ���� ��� ����������� �����
    public Text textRecord; // ������ �� ��������� ���� ��� ����������� �������
    public Text textRecord02; // ������ �� ��������� ���� ��� ����������� �������

    private Vector3 center; // ����� ��������� ��������
    private float angle; // ������� ����
    private bool isCircularMotion = true; // ���� ��� ����������� ������ ��������
    private bool isMoving = true; // ���� ��� ����������� ��������
    public static int score = 0; // ���������� ��� �������� �����
    private int recordScore = 0; // ���������� ��� �������� �������
    private const float maxSpeed = 7.0f; // ������������ �������� �������� ����

    void Start()
    {
        center = new Vector3(0, 0, 0); // ������������� ����� ����� ��� ����� ��������� ��������
        SelectRandomTarget();
        LoadRecordScore();
        UpdateScoreText();
        UpdateRecordText();
    }

    void Update()
    {
        if (isMoving && IsMenuPanelOffScreen())
        {
            if (isCircularMotion)
            {
                CircularMove();
            }
            else
            {
                LinearMove();
            }

            // �������� �������
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                isCircularMotion = false;
            }

            CheckBounds();
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

    void SelectRandomTarget()
    {
        if (targets.Length == 0)
        {
            Debug.LogError("No targets assigned!");
            return;
        }

        // ���������� ���� ���� �������
        foreach (GameObject target in targets)
        {
            SpriteRenderer targetRenderer02 = target.GetComponent<SpriteRenderer>();
            if (targetRenderer02 != null)
            {
                targetRenderer02.color = Color.white; // ���������� ���� ������ � ���������
            }
        }

        // �������� ��������� ������
        int randomIndex = Random.Range(0, targets.Length);
        GameObject selectedTarget = targets[randomIndex];

        // ������ ��������� ������ � ������� ����
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

    void CheckBounds()
    {
        if (Vector3.Distance(transform.position, center) > 7.0f)
        {
            ShowLoseWindow();
            PlayFailParticles(transform.position); // ��������� ����� �������
            if (MusicControl.isOn) PlayMissSound(); // ������������� ���� �������
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Target"))
        {
            SpriteRenderer targetRenderer = other.GetComponent<SpriteRenderer>();
            if (targetRenderer != null && targetRenderer.color == Color.red)
            {
                isCircularMotion = true;
                isMoving = true;
                SelectRandomTarget(); // ������� �������� ������
                PlaySuccessParticles(other.transform.position); // ��������� ������� �������
                if (MusicControl.isOn) PlayHitSound(); // ������������� ���� ���������
                IncreaseScore(); // ����������� ����
            }
            else
            {
                ShowLoseWindow();
                PlayFailParticles(transform.position); // ��������� ����� �������
                if (MusicControl.isOn) PlayMissSound(); // ������������� ���� �������
            }
        }
    }

    void ShowLoseWindow()
    {
        isMoving = false;
        if (loseWindow != null)
        {
            loseWindow.SetActive(true);
        }
        else
        {
            Debug.LogError("LoseWindow is not assigned!");
        }

        CheckAndSaveRecord();
    }

    void PlaySuccessParticles(Vector3 position)
    {
        if (successParticlesPrefab != null)
        {
            ParticleSystem particles = Instantiate(successParticlesPrefab, position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration); // ���������� ������� ����� �� ���������
        }
        else
        {
            Debug.LogError("SuccessParticlesPrefab is not assigned!");
        }
    }

    void PlayFailParticles(Vector3 position)
    {
        if (failParticlesPrefab != null)
        {
            ParticleSystem particles = Instantiate(failParticlesPrefab, position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration); // ���������� ������� ����� �� ���������
        }
        else
        {
            Debug.LogError("FailParticlesPrefab is not assigned!");
        }
    }

    void PlayHitSound()
    {
        if (hitAudioSource != null)
        {
            hitAudioSource.Play();
        }
        else
        {
            Debug.LogError("HitAudioSource is not assigned!");
        }
    }

    void PlayMissSound()
    {
        if (missAudioSource != null)
        {
            missAudioSource.Play();
        }
        else
        {
            Debug.LogError("MissAudioSource is not assigned!");
        }
    }

    bool IsMenuPanelOffScreen()
    {
        if (menuPanel == null)
        {
            Debug.LogError("MenuPanel is not assigned!");
            return false;
        }

        Vector3 screenPoint = Camera.main.WorldToViewportPoint(menuPanel.transform.position);
        return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
    }

    void IncreaseScore()
    {
        score++;
        UpdateScoreText();
        IncreaseSpeed(); // ����������� �������� �������� ����
    }

    void IncreaseSpeed()
    {
        if (speed < maxSpeed)
        {
            speed += 0.1f;
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
        }
    }

    void UpdateScoreText()
    {
        if (textScore != null)
        {
            textScore.text = score.ToString();
        }
        else
        {
            Debug.LogError("TextScore is not assigned!");
        }
    }

    void LoadRecordScore()
    {
        recordScore = PlayerPrefs.GetInt("RecordScore", 0);
        YandexGame.NewLeaderboardScores("LiderBordClicker", recordScore);
    }

    void CheckAndSaveRecord()
    {
        if (score > recordScore)
        {
            recordScore = score;
            PlayerPrefs.SetInt("RecordScore", recordScore);
            YandexGame.savesData.money = recordScore;
            YandexGame.SaveProgress();
            UpdateRecordText();
        }
    }

    void UpdateRecordText()
    {
        if (textRecord != null)
        {
            textRecord.text = recordScore.ToString();
            textRecord02.text = recordScore.ToString();
        }
        else
        {
            Debug.LogError("TextRecord is not assigned!");
        }
    }

    public void ResetBall()
    {
        isCircularMotion = true;
        isMoving = true;
        angle = 0f;
        speed = 2.0f; // ���������� �������� �������� ����
        transform.position = new Vector3(center.x + Mathf.Cos(angle) * radius, center.y + Mathf.Sin(angle) * radius, transform.position.z);
        SelectRandomTarget();
        UpdateScoreText();
    }
}