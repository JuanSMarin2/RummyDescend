using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private float startSpeed = 2f;

    [SerializeField] private bool moveVertically = false; 
    [SerializeField] private bool startInPositiveDirection = true;

    [SerializeField] private GameObject keyPrefab; // Asigna el prefab en el inspector
    [SerializeField] private GameObject coinPrefab; // Asigna el prefab en el inspector


    [SerializeField] private Vector2 dropOffset = new Vector2(0, 0.5f); // opcional, para que no aparezca encima del enemigo

    [SerializeField] private bool isWarden;

    [SerializeField, Range(0f, 1f)] private float dropChance = 0.5f;



    private Vector2 direction;

    private void Start()
    {
        startSpeed = speed;
        if (moveVertically)
            direction = startInPositiveDirection ? Vector2.up : Vector2.down;
        else
            direction = startInPositiveDirection ? Vector2.right : Vector2.left;

        if (GameData.Instance != null && GameData.Instance.defeatedEnemies.Contains(gameObject.name))
        {
            // Ya fue derrotado antes: desactivar sin soltar nada
            Disable();
        }
    }

    private void Update()
    {
       

        transform.Translate(direction * speed * Time.deltaTime);
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

            // Guardar posición del jugador y el ID del enemigo
            GameData.Instance.lastPlayerPosition = collision.transform.position;
            GameData.Instance.RegisterDefeatedEnemy(gameObject.name);


            GameData.Instance.lastEnemyID = gameObject.name;
            GameManager.Instance.ChangeScene("EnemyBattle");
            PlayerController.Instance.canMove = false;

            OnDefeated();

           

        }

    }

    private void FlipDirection()
    {
        direction *= -1;

   
        if (!moveVertically)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
        }
    }

    public void OnDefeated()
    {
        GameData.Instance.RegisterDefeatedEnemy(gameObject.name);

        if (isWarden && keyPrefab != null)
        {
            Instantiate(keyPrefab, transform.position + (Vector3)dropOffset, Quaternion.identity);
        }

        if (!isWarden && coinPrefab != null && Random.value <= dropChance)
        {
            Instantiate(coinPrefab, transform.position + (Vector3)dropOffset, Quaternion.identity);
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
}
