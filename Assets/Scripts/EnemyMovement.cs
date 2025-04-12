using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private float startSpeed = 2f;

    [SerializeField] private bool moveVertically = false; 
    [SerializeField] private bool startInPositiveDirection = true;



    private Vector2 direction;

    private void Start()
    {
        startSpeed = speed;
        if (moveVertically)
            direction = startInPositiveDirection ? Vector2.up : Vector2.down;
        else
            direction = startInPositiveDirection ? Vector2.right : Vector2.left;
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

            
            GameManager.Instance.ChangeScene("EnemyBattle");
            PlayerController.Instance.canMove = false;

           

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
