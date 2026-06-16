using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerControl : MonoBehaviour
{
    public float speed;    // Tambahkan speed
    public GameObject GameManagerGO;
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;
    public GameObject PlayerBulletGO;
    public GameObject ExplosionGO;

    public TMP_Text LivesUIText;

    const int MaxLives = 3;
    int lives;

    // player terkena bullet
    public void TakeDamage()
    {
        lives--;
        LivesUIText.text = lives.ToString();

        PlayExplosion(); // optional visual feedback

        if (lives <= 0)
        {
            // akhir permainan: beritahu GameManager
            if (GameManagerGO != null)
            {
                var gm = GameManagerGO.GetComponent<GameManager>();
                if (gm != null)
                {
                    gm.SetGameManagerState(GameManager.GameManagerState.GameOver);
                }
            }

            gameObject.SetActive(false);
        }
    }

    // set awal
    public void Init()
    {
        lives = MaxLives;
        LivesUIText.text = lives.ToString();
        transform.position = new Vector2(0,0);
        gameObject.SetActive(true);
    }

    // pergerakan player
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            GetComponent<AudioSource>().Play();
            GameObject bullet01 = (GameObject)Instantiate (PlayerBulletGO);
            bullet01.transform.position = bulletPosition01.transform.position;

            GameObject bullet02 = (GameObject)Instantiate (PlayerBulletGO);
            bullet02.transform.position = bulletPosition02.transform.position;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(x, y).normalized;

        Move(direction);
    }

    // pergerakan player
    void Move(Vector2 direction)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        max.x = max.x - 0.225f;
        min.x = min.x + 0.225f; 

        max.y = max.y - 0.285f;
        min.y = min.y + 0.285f;

        Vector2 pos = transform.position;
        pos += direction * speed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    // player menabrak enemy
    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.tag == "EnemyShipTag") || (col.tag == "EnemyBulletTag"))
        {
            PlayExplosion();
            
            lives--;
            LivesUIText.text = lives.ToString();

            if (lives == 0)
            {
                GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
                gameObject.SetActive(false);
                
            }
            
        }
    }

    // animasi ledakan player
    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(ExplosionGO);

        explosion.transform.position = transform.position;
    }
}
