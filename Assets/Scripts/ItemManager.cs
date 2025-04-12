using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ItemManager : MonoBehaviour
{



   

    [SerializeField] private int value = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Key") && !GameManager.Instance.hasKey)
        {
          

            GameManager.Instance.hasKey = true;

            collision.gameObject.SetActive(false);

        }

        if (collision.CompareTag("Coin"))
        {
            MoneyManager.Instance.IncreaseMoney(value);

            Destroy(collision.gameObject);
        }


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {


if (collision.gameObject.CompareTag("Door") && GameManager.Instance.hasKey)
    {
        GameManager.Instance.hasKey = false;

        string doorID = collision.gameObject.name;

        // Registrar puerta abierta
        GameData.Instance.RegisterOpenedDoor(doorID);

        collision.gameObject.SetActive(false);
    }

    }
}
