using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab & spawn settings")]
    public GameObject EnemyGO;                
    public int enemiesCount = 5;

    // SETTING NORMAL (Atas ke Bawah)
    public float spawnYTop = 6f;    // Muncul di atas layar
    public float spawnXMin = -2.5f; // Batas Kiri Sebaran
    public float spawnXMax = 2.5f;  // Batas Kanan Sebaran

    List<GameObject> currentEnemies = new List<GameObject>();

    public void SpawnEnemiesForQuestion(int correctAnswer)
    {
        ClearAllEnemies();

        // 1. Siapkan Angka
        HashSet<int> values = new HashSet<int>();
        values.Add(correctAnswer);

        while (values.Count < enemiesCount)
        {
            int rand = correctAnswer + Random.Range(-10, 11);
            if (rand < 0) continue;
            values.Add(rand);
        }

        List<int> list = new List<int>(values);
        // Acak Posisi Angka dalam List
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            int tmp = list[i]; list[i] = list[r]; list[r] = tmp;
        }

        // 2. Hitung Jarak Horizontal (X)
        float totalWidth = spawnXMax - spawnXMin;
        float spacing = (enemiesCount > 1) ? totalWidth / (enemiesCount - 1) : 0f;

        // 3. Spawn Musuh
        for (int i = 0; i < list.Count; i++)
        {
            int val = list[i];

            // Posisi X Rata
            float baseX = spawnXMin + spacing * i;

            // Sedikit variasi acak biar tidak baris kaku
            float jitterX = Random.Range(-spacing * 0.2f, spacing * 0.2f);
            float x = Mathf.Clamp(baseX + jitterX, spawnXMin, spawnXMax);
            
            // Variasi Y (Biar turunnya tidak barengan satu garis lurus)
            float jitterY = Random.Range(0f, 1.5f); 
            float y = spawnYTop + jitterY;

            Vector3 pos = new Vector3(x, y, 0f);
            GameObject go = Instantiate(EnemyGO, pos, Quaternion.identity);

            EnemyControl ec = go.GetComponent<EnemyControl>();
            if (ec != null) ec.SetValue(val);

            currentEnemies.Add(go);
        }
    }

    public void NotifyEnemyDestroyed(GameObject enemy)
    {
        currentEnemies.Remove(enemy);
        int sisa = GameObject.FindGameObjectsWithTag("EnemyShipTag").Length;

        if (currentEnemies.Count == 0 || sisa <= 1)
        {
            var gm = FindObjectOfType<GameManager>();
            if (gm != null) gm.OnAllEnemiesCleared();
        }
    }

    public void ClearAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyShipTag");
        if (enemies != null)
        {
            foreach (GameObject enemy in enemies) Destroy(enemy);
        }
        currentEnemies.Clear();
    }
    
    // Fungsi tidak terpakai (biarkan kosong/hapus)
    public void ScheduleEnemySpawner() { }
    public void UnscheduleEnemySpawner() { }
}