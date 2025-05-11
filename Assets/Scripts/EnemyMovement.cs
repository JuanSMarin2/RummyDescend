using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private float speed = 2f;
    private float startSpeed = 2f;
    [SerializeField] private bool moveVertically = false;
    [SerializeField] private bool startInPositiveDirection = true;
    [SerializeField] private GameObject keyPrefab; // Prefab de la llave (para Wardens)
    [SerializeField] private GameObject coinPrefab; // Prefab de la moneda (70%)
    [SerializeField] private GameObject specialCardPickupPrefab; // Prefab de la carta especial (30%)
    [SerializeField] private Vector2 dropOffset = new Vector2(0, 0.5f); // Ajuste de posición del drop
    [SerializeField] private bool isWarden; // ¿Es un Warden? (dropea llave)
    [SerializeField] private bool isBoss;
    [SerializeField] private GameObject flagPrefab;

    private Vector2 direction;

    private void Start()
    {
        startSpeed = speed;

        // Jefes no se mueven
        if (isBoss)
        {
            speed = 0;

            isWarden = false;
            return;
        }

        

        direction = startInPositiveDirection ?
            (moveVertically ? Vector2.up : Vector2.right) :
            (moveVertically ? Vector2.down : Vector2.left);

        if (GameData.Instance != null && GameData.Instance.defeatedEnemies.Contains(gameObject.name))
        {
            Disable();
        }
    }

    private void Update()
    {
        if (!isBoss) // Solo mover si no es jefe
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyLimit") || collision.CompareTag("Wall"))
        {
            StartCoroutine(WaitTimerToFlip());
        }

        if (collision.CompareTag("Player"))
        {
            speed = 0;
            GameData.Instance.lastPlayerPosition = collision.transform.position;
            GameData.Instance.RegisterDefeatedEnemy(gameObject.name);
            GameData.Instance.lastEnemyID = gameObject.name;
            GameData.Instance.lastEnemyData = enemyData;
            GameManager.Instance.ChangeScene("EnemyBattle");
            PlayerController.Instance.canMove = false;
            StartCoroutine(TimerToDefeat());


           
        }
    }

    private void FlipDirection()
    {
        direction *= -1;
        Vector3 scale = transform.localScale;

        if (!moveVertically) scale.x *= -1;
        else scale.y *= -1;

        transform.localScale = scale;
    }

    public void OnDefeated()
    {
        GameData.Instance.RegisterDefeatedEnemy(gameObject.name);

        if (isBoss)
        {
            // Jefes dropean bandera
            if (flagPrefab != null)
            {
                Instantiate(flagPrefab, transform.position + (Vector3)dropOffset, Quaternion.identity);
            }
        }
        else if (isWarden)
        {
            // Wardens dropean llave
            if (keyPrefab != null)
            {
                Instantiate(keyPrefab, transform.position + (Vector3)dropOffset, Quaternion.identity);
            }
        }
        else
        {
            // Enemigos normales: 70% moneda, 30% carta especial
            float dropRoll = Random.value;
            if (dropRoll <= 0.7f && coinPrefab != null)
            {
                Instantiate(coinPrefab, transform.position + (Vector3)dropOffset, Quaternion.identity);
            }
            else if (specialCardPickupPrefab != null)
            {
                Instantiate(specialCardPickupPrefab, transform.position + (Vector3)dropOffset, Quaternion.identity);
            }
        }

        gameObject.SetActive(false);
    }


    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator WaitTimerToFlip()
    {
        speed = 0;
        yield return new WaitForSeconds(0.5f);
        speed = startSpeed;
        FlipDirection();
    }

    private IEnumerator TimerToDefeat()
    {
        yield return new WaitForSeconds(0.9f);
        OnDefeated();


    }
}