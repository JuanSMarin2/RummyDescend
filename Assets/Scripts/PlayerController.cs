using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField]
    private float moveSpeed = 5f;  // Velocidad de movimiento
    private Vector2 movement;  // Movimiento del jugador

    private bool touchingWallLeft = false;
    private bool touchingWallRight = false;
    private bool touchingWallUp = false;
    private bool touchingWallDown = false;

    public bool canMove;

    private Rigidbody2D rb;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }
        void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Obtener el componente Rigidbody2D

        canMove = true;
    }

    void Update()
    {
        moveSpeed = canMove ? 5 : 0; 


            // Obtener la entrada del jugador para moverlo
            movement.x = Input.GetAxisRaw("Horizontal");  // A/D o flechas izquierda/derecha
            movement.y = Input.GetAxisRaw("Vertical");    // W/S o flechas arriba/abajo

        

        // Detectar las paredes al hacer contacto con ellas
        touchingWallLeft = false;
        touchingWallRight = false;
        touchingWallUp = false;
        touchingWallDown = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Door")) // Si el objeto con el que colisionamos es una pared
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.x > 0) // Pared a la izquierda
                {
                    touchingWallLeft = true;
                }
                else if (contact.normal.x < 0) // Pared a la derecha
                {
                    touchingWallRight = true;
                }
                else if (contact.normal.y > 0) // Pared arriba
                {
                    touchingWallUp = true;
                }
                else if (contact.normal.y < 0) // Pared abajo
                {
                    touchingWallDown = true;
                }
            }
        }
    }

    void FixedUpdate()
    {


            // Calcular la velocidad de movimiento
            Vector2 moveDirection = movement.normalized;  // Normalizamos el movimiento para que el jugador se mueva a la misma velocidad en cualquier dirección

       

        float moveAmountX = movement.x * moveSpeed;
        float moveAmountY = movement.y * moveSpeed;

        // Bloquear el movimiento si está tocando una pared en la dirección en la que se mueve
        if ((touchingWallRight && moveAmountX > 0) || (touchingWallLeft && moveAmountX < 0))
        {
            moveAmountX = 0;
        }

        // Bloquear el movimiento en la dirección vertical si está tocando una pared arriba o abajo
        if ((touchingWallUp && moveAmountY > 0) || (touchingWallDown && moveAmountY < 0))
        {
            moveAmountY = 0;
        }

        // Aplicar el movimiento al Rigidbody2D
        rb.velocity = new Vector2(moveAmountX, moveAmountY);  // Movimiento en ambos ejes
    }
}
