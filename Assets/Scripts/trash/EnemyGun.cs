using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject EnemyBulletGO;
    private bool hasFired = false; // Penanda agar cuma nembak sekali

    void Update()
    {
        // Cek: Apakah saya belum menembak?
        if (!hasFired)
        {
            // Cek Posisi: Apakah pesawat sudah turun dan masuk layar?
            // (Asumsi batas atas layar adalah Y = 5, jadi kita pakai 4.5 biar aman)
            if (transform.position.y < 4.5f) 
            {
                FireEnemyBullet();
                hasFired = true; // Tandai sudah nembak
            }
        }
    }

    void FireEnemyBullet() 
    {
        GameObject playerShip = GameObject.Find("PlayerGO");

        if (playerShip != null)
        {
            // Spawn peluru
            GameObject bullet = Instantiate(EnemyBulletGO, transform.position, transform.rotation);

            // Arahkan moncong peluru ke pemain secara visual
            Vector3 direction = playerShip.transform.position - bullet.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // --- PERBAIKAN DI SINI ---
            // Gunakan SetTarget (karena Anda pakai script Homing), BUKAN SetDirection
            EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
            if (bulletScript != null)
            {
                // Kirim Transform pemain agar peluru bisa mengejar terus
                bulletScript.SetTarget(playerShip.transform);
            }
        }
    }
}