using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playButton;
    public GameObject playerShip;
    public GameObject enemySpawner;
    public GameObject GameOverGO;
    public GameObject scoreUITextGO;
    public GameObject TimeCounterGO;
    public GameObject GameTitleGO;

    // Untuk menyembunyikan/menampilkan joystick tanpa SetActive(false)
    public CanvasGroup joystickCanvasGroup;

    // Pastikan kedua ini sudah diisi di Inspector
    public GameObject QuestionGeneratorGO;
    public GameObject questionUIControllerGO;
    public GameObject ShootButton;

    public enum GameManagerState
    {
        Opening,
        Gameplay,
        GameOver,
    }

    GameManagerState GMState;

    void Start()
    {
        GMState = GameManagerState.Opening;
        UpdateGameManagerState();
    }

    void SetJoystickVisible(bool visible)
    {
        if (joystickCanvasGroup != null)
        {
            joystickCanvasGroup.alpha = visible ? 1f : 0f;
            joystickCanvasGroup.interactable = visible;
            joystickCanvasGroup.blocksRaycasts = visible;
        }
    }

    void UpdateGameManagerState()
    {
        switch (GMState)
        {
            case GameManagerState.Opening:
                GameOverGO.SetActive(false);
                playButton.SetActive(true);
                GameTitleGO.SetActive(true);

                // Joystick disembunyikan saat awal game
                SetJoystickVisible(false);

                // Matikan spawner saat opening
                enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
                enemySpawner.GetComponent<EnemySpawner>().ClearAllEnemies();

                if (questionUIControllerGO != null)
                {
                    questionUIControllerGO.GetComponent<QuestionUIController>().HideQuestion();
                }

                break;

            case GameManagerState.Gameplay:
                scoreUITextGO.GetComponent<GameScore>().Score = 0;
                playButton.SetActive(false);
                GameTitleGO.SetActive(false);
                ShootButton.SetActive(true);

                // Joystick muncul saat game dimulai
                SetJoystickVisible(true);

                playerShip.GetComponent<PlayerControl>().Init();
                TimeCounterGO.GetComponent<TimeCounter>().StartTimeCounter();

                if (QuestionGeneratorGO != null)
                {
                    var qg = QuestionGeneratorGO.GetComponent<QuestionGenerator>();

                    if (qg != null)
                    {
                        string teksSoal = qg.currentQuestion;
                        int jawabanBenar = qg.currentAnswer;

                        Debug.Log($"[GameManager] Soal: '{teksSoal}' || Jawaban Seharusnya: {jawabanBenar}");

                        if (questionUIControllerGO != null)
                        {
                            questionUIControllerGO.GetComponent<QuestionUIController>().ShowQuestion(teksSoal);
                        }

                        var spawner = enemySpawner.GetComponent<EnemySpawner>();

                        if (spawner != null)
                        {
                            spawner.UnscheduleEnemySpawner();
                            spawner.SpawnEnemiesForQuestion(jawabanBenar);
                        }
                    }
                    else
                    {
                        Debug.LogError("ERROR: Object QuestionGeneratorGO tidak memiliki script QuestionGenerator!");
                    }
                }
                else
                {
                    Debug.LogError("ERROR FATAL: Slot 'Question Generator GO' di GameManager Inspector masih KOSONG!");
                }

                break;

            case GameManagerState.GameOver:
                TimeCounterGO.GetComponent<TimeCounter>().StopTimeCounter();
                GameOverGO.SetActive(true);
                SetJoystickVisible(false);
                ShootButton.SetActive(false);

                enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();

                Invoke("ChangeToOpeningState", 5f);

                if (questionUIControllerGO != null)
                {
                    questionUIControllerGO.GetComponent<QuestionUIController>().HideQuestion();
                }

                break;
        }
    }

    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    public void StartgamePlay()
    {
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
    }

    public void ChangeToOpeningState()
    {
        SetGameManagerState(GameManagerState.Opening);
    }

    // Dipanggil ketika jawaban BENAR ditembak
    public void CorrectHit()
    {
        if (scoreUITextGO != null)
        {
            scoreUITextGO.GetComponent<GameScore>().Score += 100;
        }

        // Generate soal baru
        if (QuestionGeneratorGO != null)
        {
            var qg = QuestionGeneratorGO.GetComponent<QuestionGenerator>();
            var sp = enemySpawner.GetComponent<EnemySpawner>();

            if (qg != null && sp != null)
            {
                qg.NewQuestion();

                Debug.Log($"[CorrectHit] Soal Baru: {qg.currentQuestion} | Jawaban: {qg.currentAnswer}");

                sp.SpawnEnemiesForQuestion(qg.currentAnswer);
            }
        }
    }

    // Dipanggil ketika jawaban SALAH ditembak
    public void WrongHit()
    {
        if (playerShip != null)
        {
            var pc = playerShip.GetComponent<PlayerControl>();

            if (pc != null)
            {
                pc.TakeDamage();
            }
        }
    }

    public void OnAllEnemiesCleared()
    {
        Debug.Log("Waktu habis! Musuh sudah lewat semua.");

        if (QuestionGeneratorGO != null && enemySpawner != null)
        {
            var qg = QuestionGeneratorGO.GetComponent<QuestionGenerator>();
            var sp = enemySpawner.GetComponent<EnemySpawner>();

            if (qg != null && sp != null)
            {
                qg.NewQuestion();
                sp.SpawnEnemiesForQuestion(qg.currentAnswer);
            }
        }
    }
}