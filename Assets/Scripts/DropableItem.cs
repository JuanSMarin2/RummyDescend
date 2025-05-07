using UnityEngine;
using System.Collections;

public class DropableItem : MonoBehaviour
{
    [SerializeField] private float disableDuration = 0.5f;
    private BoxCollider2D boxCollider;
    private Coroutine disableCoroutine;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
     
    }

    private void OnEnable()
    {
        // Iniciar lógica cuando el objeto se activa
        StartCoroutine(InitializeCollider());

        // Suscribirse al evento de cambio de escena
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        // Desuscribirse del evento cuando el objeto se desactiva
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnSceneChanged;

        // Detener cualquier corrutina en curso
        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
        }
    }

    private IEnumerator InitializeCollider()
    {
        // Desactivar collider al inicio
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
            yield return new WaitForSeconds(disableDuration);
            boxCollider.enabled = true;
        }
    }

    private void OnSceneChanged(UnityEngine.SceneManagement.Scene current, UnityEngine.SceneManagement.Scene next)
    {
        // Reiniciar el proceso de desactivación temporal
        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
        }
        disableCoroutine = StartCoroutine(DisableColliderTemporarily());
    }

    private IEnumerator DisableColliderTemporarily()
    {
        if (boxCollider != null)
        {
            // Desactivar y esperar
            boxCollider.enabled = false;
            yield return new WaitForSeconds(disableDuration);

            // Reactivar solo si el objeto sigue activo
            if (gameObject.activeInHierarchy)
            {
                boxCollider.enabled = true;
            }
        }
    }
}