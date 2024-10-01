using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System.Collections;

public class VRObjectVelocityDisplay : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // LeftHand o RightHand
    public SteamVR_Behaviour_Pose controllerPose; // Componente del controlador
    public SteamVR_Action_Boolean grabAction; // Acción de agarre para detectar si un objeto es agarrado

    public Text velocityText; // Texto para mostrar la velocidad

    private Interactable currentInteractable; // Objeto interactivo actual
    private Rigidbody objectRigidbody; // Rigidbody del objeto para calcular la velocidad
    private Vector3 previousPosition; // Posición del objeto en el frame anterior
    private bool isObjectThrown = false; // Indica si el objeto fue lanzado
    private bool isObjectGrabbed = false; // Indica si el objeto está siendo agarrado

    void Update()
    {
        // Verificar si se está agarrando un objeto
        if (grabAction.GetState(handType))
        {
            // Obtener el objeto interactivo que está siendo agarrado
            currentInteractable = FindObjectOfType<Hand>().currentAttachedObject?.GetComponent<Interactable>();

            if (currentInteractable != null && !isObjectGrabbed)
            {
                // Obtenemos el Rigidbody del objeto
                objectRigidbody = currentInteractable.GetComponent<Rigidbody>();

                if (objectRigidbody != null)
                {
                    // Inicializar la velocidad y posición cuando el objeto es agarrado
                    previousPosition = objectRigidbody.position;
                    velocityText.text = "Velocidad de lanzamiento: 0 m/s";
                    velocityText.gameObject.SetActive(true);
                    isObjectGrabbed = true;
                }
            }
        }
        else if (isObjectGrabbed && !isObjectThrown)
        {
            // Cuando el objeto se suelta, comienza a calcularse la velocidad de lanzamiento
            if (objectRigidbody != null)
            {
                // Calcular la velocidad como la diferencia de posición dividida entre el tiempo transcurrido
                Vector3 velocity = (objectRigidbody.position - previousPosition) / Time.deltaTime;
                float speed = velocity.magnitude;

                // Mostrar la velocidad en el texto
                velocityText.text = "Velocidad de lanzamiento: " + speed.ToString("F2") + " m/s";

                // Marcar que el objeto ha sido lanzado
                isObjectThrown = true;
                isObjectGrabbed = false;

                // Iniciar la corutina para desaparecer el texto después de 5 segundos
                StartCoroutine(HideVelocityTextAfterDelay(5));
            }
        }

        // Actualizar la posición anterior para calcular la velocidad en el siguiente frame
        if (objectRigidbody != null && isObjectGrabbed)
        {
            previousPosition = objectRigidbody.position;
        }
    }

    // Corutina para ocultar el texto después de un retraso
    IEnumerator HideVelocityTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        velocityText.gameObject.SetActive(false);
        isObjectThrown = false; // Reiniciar la variable para el próximo lanzamiento
    }
}

