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

    
    // Pastikan kedua ini sudah diisi di Inspector!
    public GameObject QuestionGeneratorGO;
    public GameObject questionUIControllerGO;

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

    void UpdateGameManagerState()
    {
        switch (GMState)
        {
            case GameManagerState.Opening:
                GameOverGO.SetActive(false);
                playButton.SetActive(true);
                GameTitleGO.SetActive(true);
                
                // Matikan spawner saat opening
                enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
                enemySpawner.GetComponent<EnemySpawner>().ClearAllEnemies();
                
                if(questionUIControllerGO != null)
                    questionUIControllerGO.GetComponent<QuestionUIController>().HideQuestion();

                break;

            case GameManagerState.Gameplay:
                scoreUITextGO.GetComponent<GameScore>().Score = 0;
                playButton.SetActive(false);
                GameTitleGO.SetActive(false);


                playerShip.GetComponent<PlayerControl>().Init();
                TimeCounterGO.GetComponent<TimeCounter>().StartTimeCounter();


                // --- BAGIAN PERBAIKAN PENTING ---
                if (QuestionGeneratorGO != null)
                {
                    // 1. Ambil script dari objek yang ditunjuk di Inspector (BUKAN FindObjectOfType)
                    var qg = QuestionGeneratorGO.GetComponent<QuestionGenerator>();
                    
                    // 2. Ambil Soal & Jawaban dari sumber yang SAMA
                    string teksSoal = qg.currentQuestion;
                    int jawabanBenar = qg.currentAnswer;

                    // DEBUG: Cek Console Unity jika angka masih salah
                    Debug.Log($"[GameManager] Soal: '{teksSoal}' || Jawaban Seharusnya: {jawabanBenar}");

                    // 3. Tampilkan Soal
                    if(questionUIControllerGO != null)
                        questionUIControllerGO.GetComponent<QuestionUIController>().ShowQuestion(teksSoal);
                    
                    // 4. Spawn Musuh (Matikan scheduler random dulu agar tidak bentrok)
                    var spawner = enemySpawner.GetComponent<EnemySpawner>();
                    spawner.UnscheduleEnemySpawner(); // Stop spawn random
                    spawner.SpawnEnemiesForQuestion(jawabanBenar); // Spawn jawaban
                }
                else
                {
                    Debug.LogError("ERROR FATAL: Slot 'Question Generator GO' di GameManager Inspector masih KOSONG!");
                }
                // --------------------------------
                break;

            case GameManagerState.GameOver:
                TimeCounterGO.GetComponent<TimeCounter>().StopTimeCounter();
                GameOverGO.SetActive(true);
                enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
                Invoke("ChangeToOpeningState", 5f);
                
                if(questionUIControllerGO != null)
                    questionUIControllerGO.GetComponent<QuestionUIController>().HideQuestion();
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
            scoreUITextGO.GetComponent<GameScore>().Score += 100;

        // Generate soal baru
        if (QuestionGeneratorGO != null)
        {
            var qg = QuestionGeneratorGO.GetComponent<QuestionGenerator>();
            var sp = enemySpawner.GetComponent<EnemySpawner>();
            
            qg.NewQuestion(); // Bikin soal baru
            
            // Log untuk debug
            Debug.Log($"[CorrectHit] Soal Baru: {qg.currentQuestion} | Jawaban: {qg.currentAnswer}");
            
            sp.SpawnEnemiesForQuestion(qg.currentAnswer); // Spawn musuh baru
        }
    }

    // Dipanggil ketika jawaban SALAH ditembak
    public void WrongHit()
    {
        if (playerShip != null)
        {
            var pc = playerShip.GetComponent<PlayerControl>();
            if (pc != null) pc.TakeDamage();
        }
    }

   public void OnAllEnemiesCleared()
    {
        Debug.Log("Waktu habis! Musuh sudah lewat semua.");

        // OPSI A: HUKUMAN (Pemain kehilangan nyawa karena telat jawab)
        // WrongHit(); // <--- Uncomment baris ini jika ingin mengurangi nyawa pemain

        // OPSI B: REFESH (Langsung kasih soal baru biar game lanjut)
        if (QuestionGeneratorGO != null && enemySpawner != null)
        {
            var qg = QuestionGeneratorGO.GetComponent<QuestionGenerator>();
            var sp = enemySpawner.GetComponent<EnemySpawner>();

            if (qg != null && sp != null)
            {
                // Buat soal baru
                qg.NewQuestion(); 
                
                // Munculkan musuh baru
                sp.SpawnEnemiesForQuestion(qg.currentAnswer);
            }
        }
    }
}