using UnityEngine;
using TMPro;

public class EnemyControl : MonoBehaviour
{
    public int value = 0;
    public TMP_Text numberText;

    [Header("Movement")]
    public float speed = 1.5f;
    public float waveFrequency = 1.0f;
    public float waveMagnitude = 1.5f;

    [Header("SHOOTING SETTINGS")]
    [Tooltip("Drag objek EnemyGunGO (anak pesawat) ke sini")]
    public Transform gunPoint; 

    [Tooltip("Garis Pemicu: Atur di Inspector biar pas di bawah tempat muncul")]
    public float shootingYLine = 4.0f; // SAYA NAIKKAN BIAR CEPAT KETEMU GARIS
    
    [Tooltip("Set 0 agar LANGSUNG nembak pas kena garis")]
    public float startDelay = 0.0f;    
    
    public GameObject enemyBulletPrefab;
    public float fireRate = 6f;//Jeda antar peluru berikutnya

    // Status Internal
    private float nextFireTime = 0f;
    private bool hasCrossedLine = false;

    [Header("Effects")]
    public GameObject ExplosionGO;
    private GameManager gameManagerRef;
    private bool hasBeenHit = false;
    private float startX;

    public void SetValue(int v)
    {
        value = v;
        if (numberText != null) numberText.text = value.ToString();
    }

    void Start()
    {
        if (numberText != null) numberText.text = value.ToString();
        gameManagerRef = FindObjectOfType<GameManager>();
        startX = transform.position.x;
    }

    void Update()
    {
        // 1. GERAKAN
        Vector3 pos = transform.position;
        pos.y -= speed * Time.deltaTime;
        pos.x = startX + Mathf.Sin(Time.time * waveFrequency) * waveMagnitude;
        transform.position = pos;

        // 2. LOGIKA GARIS PEMICU
        if (!hasCrossedLine)
        {
            // Jika musuh sudah turun MELEWATI garis
            if (transform.position.y <= shootingYLine)
            {
                hasCrossedLine = true;
                // Langsung set waktu nembak = Waktu Sekarang + Delay (0)
                nextFireTime = Time.time + startDelay; 
            }
        }

        // 3. MENEMBAK
        if (hasCrossedLine && Time.time > nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireRate;
        }

        // 4. HAPUS JIKA KELUAR LAYAR BAWAH
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y - 1f)
        {
            EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
            if (spawner != null) spawner.NotifyEnemyDestroyed(gameObject);
            Destroy(gameObject);
        }
    }

    void FireBullet()
    {
        if (enemyBulletPrefab != null)
        {
            // Tentukan posisi spawn
            Vector3 spawnPos = (gunPoint != null) ? gunPoint.position : transform.position;

            // --- HITUNG ROTASI AWAL AGAR MENGHADAP PLAYER ---
            GameObject player = GameObject.FindGameObjectWithTag("PlayerShipTag");
            Quaternion initRotation = Quaternion.identity;

            if (player != null)
            {
                Vector3 direction = player.transform.position - spawnPos;
                // Hitung sudut (dikurangi 90 karena sprite peluru biasanya menghadap atas)
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                initRotation = Quaternion.Euler(0, 0, angle);
            }

            // Instantiate dengan rotasi yang sudah benar
            GameObject bulletObj = Instantiate(enemyBulletPrefab, spawnPos, initRotation);
            
            // Set Target untuk script Homing
            EnemyBullet bulletScript = bulletObj.GetComponent<EnemyBullet>();
            if (bulletScript != null && player != null)
            {
                bulletScript.SetTarget(player.transform);
            }
        }
    }

    // --- VISUALISASI GARIS HIJAU ---
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-10, shootingYLine, 0), new Vector3(10, shootingYLine, 0));
        
        if (gunPoint != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(gunPoint.position, 0.15f);
        }
    }
    
    // --- TABRAKAN ---
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerBulletTag"))
        {
            if (hasBeenHit) { Destroy(col.gameObject); return; }
            hasBeenHit = true;
            Destroy(col.gameObject); 

            if (gameManagerRef == null) gameManagerRef = FindObjectOfType<GameManager>();

            int jawabanBenarServer = -999; 
            if (gameManagerRef != null && gameManagerRef.QuestionGeneratorGO != null)
            {
                var qg = gameManagerRef.QuestionGeneratorGO.GetComponent<QuestionGenerator>();
                jawabanBenarServer = qg.currentAnswer;
            }

            if (value == jawabanBenarServer)
            {
                PlayExplosion();
                if (gameManagerRef != null) gameManagerRef.CorrectHit();
                Destroy(gameObject);
            }
            else
            {
                PlayExplosion();
                if (gameManagerRef != null) gameManagerRef.WrongHit();
                Destroy(gameObject);
            }
        }
    }

    void PlayExplosion()
    {
        if (ExplosionGO != null)
        {
            GameObject ex = Instantiate(ExplosionGO, transform.position, Quaternion.identity);
            Destroy(ex, 1.5f);
        }
    }
}