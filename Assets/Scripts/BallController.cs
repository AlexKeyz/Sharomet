using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class BallController : MonoBehaviour
{
    public float radius = 1.0f; // Радиус кругового движения
    public float speed = 2.0f; // Скорость движения
    public float linearSpeed = 5.0f; // Линейная скорость движения от центра
    public GameObject[] targets; // Массив мишеней
    public GameObject loseWindow; // Панель LoseWindow
    public ParticleSystem successParticlesPrefab; // Префаб системы частиц для успешного попадания
    public ParticleSystem failParticlesPrefab; // Префаб системы частиц для поражения
    public AudioSource hitAudioSource; // AudioSource для звука попадания
    public AudioSource missAudioSource; // AudioSource для звука промаха
    public GameObject menuPanel; // Ссылка на объект MenuPanel
    public Text textScore; // Ссылка на текстовый блок для отображения очков
    public Text textRecord; // Ссылка на текстовый блок для отображения рекорда
    public Text textRecord02; // Ссылка на текстовый блок для отображения рекорда

    private Vector3 center; // Центр кругового движения
    private float angle; // Текущий угол
    private bool isCircularMotion = true; // Флаг для определения режима движения
    private bool isMoving = true; // Флаг для определения движения
    public static int score = 0; // Переменная для хранения очков
    private int recordScore = 0; // Переменная для хранения рекорда
    private const float maxSpeed = 7.0f; // Максимальная скорость вращения мяча

    void Start()
    {
        center = new Vector3(0, 0, 0); // Устанавливаем центр сцены как центр кругового движения
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

            // Проверка нажатий
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                isCircularMotion = false;
            }

            CheckBounds();
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

    void SelectRandomTarget()
    {
        if (targets.Length == 0)
        {
            Debug.LogError("No targets assigned!");
            return;
        }

        // Сбрасываем цвет всех мишеней
        foreach (GameObject target in targets)
        {
            SpriteRenderer targetRenderer02 = target.GetComponent<SpriteRenderer>();
            if (targetRenderer02 != null)
            {
                targetRenderer02.color = Color.white; // Возвращаем цвет мишени к исходному
            }
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

    void CheckBounds()
    {
        if (Vector3.Distance(transform.position, center) > 7.0f)
        {
            ShowLoseWindow();
            PlayFailParticles(transform.position); // Запускаем белые частицы
            if (MusicControl.isOn) PlayMissSound(); // Воспроизводим звук промаха
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
                SelectRandomTarget(); // Сменяем активную мишень
                PlaySuccessParticles(other.transform.position); // Запускаем красные частицы
                if (MusicControl.isOn) PlayHitSound(); // Воспроизводим звук попадания
                IncreaseScore(); // Увеличиваем очки
            }
            else
            {
                ShowLoseWindow();
                PlayFailParticles(transform.position); // Запускаем белые частицы
                if (MusicControl.isOn) PlayMissSound(); // Воспроизводим звук промаха
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
            Destroy(particles.gameObject, particles.main.duration); // Уничтожаем частицы после их окончания
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
            Destroy(particles.gameObject, particles.main.duration); // Уничтожаем частицы после их окончания
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
        IncreaseSpeed(); // Увеличиваем скорость вращения мяча
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
        speed = 2.0f; // Сбрасываем скорость вращения мяча
        transform.position = new Vector3(center.x + Mathf.Cos(angle) * radius, center.y + Mathf.Sin(angle) * radius, transform.position.z);
        SelectRandomTarget();
        UpdateScoreText();
    }
}