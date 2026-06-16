using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Targeting")]
    private Transform target; 
    
    [Header("Bullet Settings")]
    public float speed = 3f;         // Kecepatan Peluru
    public float rotateSpeed = 150f;  // Kecepatan Belok (jangan terlalu tinggi biar gak jago banget)
    public float lifeTime = 2f;      // Waktu hidup peluru

    [Header("Dumb Homing Logic")]
    public float homingDuration = 0.2f;// CUMA NGEJAR SELAMA 0.5 DETIK
    private float timer = 0f;

    public void SetTarget(Transform playerTransform)
    {
        target = playerTransform;
    }

    void Update()
    {
        // 1. Selalu maju ke depan (sesuai arah hidung peluru)
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // 2. Hitung Waktu
        timer += Time.deltaTime;

        // 3. Logika Belok (Homing)
        // Syarat: Punya target DAN waktunya masih di bawah homingDuration
        if (target != null && timer < homingDuration)
        {
            Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
            direction.Normalize();
            
            // Hitung belokan
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            transform.Rotate(0, 0, -rotateAmount * rotateSpeed * Time.deltaTime);
        }

        // 4. Hapus jika sudah tua
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerShipTag"))
        {
            // Tambahkan logika mengurangi nyawa player di sini jika ada
            Destroy(gameObject);
        }
    }
}