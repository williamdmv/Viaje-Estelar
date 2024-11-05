using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ReturnToPosition : MonoBehaviour
{
    public Transform referencePosition; // Objeto de referencia para la posición a la que debe volver
    public float returnDelay = 3.0f; // Tiempo de espera antes de regresar
    private Vector3 initialPosition; // Posición inicial de la pelota
    private bool isHeld = false; // Verifica si el objeto está siendo agarrado
    private Rigidbody rb;

    void Start()
    {
        // Guarda la posición inicial de la pelota y obtiene el componente Rigidbody
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();

        // Agrega eventos de SteamVR para cuando el objeto es agarrado y soltado
        var interactable = GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.onAttachedToHand += OnGrabbed;
            interactable.onDetachedFromHand += OnReleased;
        }
    }

    private void OnGrabbed(Hand hand)
    {
        isHeld = true; // Marca el objeto como agarrado
        StopAllCoroutines(); // Detiene cualquier retorno en progreso
    }

    private void OnReleased(Hand hand)
    {
        isHeld = false; // Marca el objeto como soltado
        StartCoroutine(ReturnAfterDelay()); // Inicia el retorno después del tiempo estipulado
    }

    private IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSeconds(returnDelay);

        if (!isHeld)
        {
            // Desactiva la física temporalmente para evitar movimientos indeseados
            rb.isKinematic = true;

            // Retorna a la posición de referencia y restablece la rotación
            transform.position = referencePosition.position;
            transform.rotation = referencePosition.rotation;

            // Reactiva la física y limpia la velocidad y rotación del Rigidbody
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Actualiza la posición inicial
            initialPosition = transform.position;
        }
    }
}


