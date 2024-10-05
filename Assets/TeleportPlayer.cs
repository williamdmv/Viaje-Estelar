using UnityEngine;
using Valve.VR.InteractionSystem; // Para los eventos de interacción de SteamVR

public class TeleportPlayer : MonoBehaviour
{
    public Vector3 targetPosition;  // Coordenadas de posición objetivo
    public Vector3 targetRotation;  // Coordenadas de rotación objetivo
    public GameObject playerPrefab;  // Referencia al Prefab del Player
    private GameObject playerInstance; // Instancia del Player en la escena

    private Interactable interactable; // Referencia al componente Interactable

    private void Awake()
    {
        interactable = GetComponent<Interactable>();

        // Asegurarse de que haya una instancia del Player en la escena
        if (playerPrefab != null && playerInstance == null)
        {
            // Buscar la instancia del prefab del player en la escena
            playerInstance = GameObject.FindWithTag("Player"); 

            if (playerInstance == null)
            {
                // Si no existe, instanciamos el prefab en la escena
                playerInstance = Instantiate(playerPrefab);
                Debug.Log("Player instanciado desde el prefab.");
            }
            else
            {
                Debug.Log("Player encontrado en la escena.");
            }
        }
        else
        {
            Debug.LogError("Prefab del Player no asignado.");
        }
    }

    // Este método se ejecuta cuando la mano del jugador comienza a pasar sobre el botón 3D
    private void HandHoverBegin(Hand hand)
    {
        Debug.Log("Mano detectada, interactuando con: " + gameObject.name);

        // Teletransporta al jugador a la posición y rotación asignada manualmente
        TeleportTo();
    }

    // Método que mueve al jugador a la posición y rotación objetivo
    private void TeleportTo()
    {
        if (playerInstance != null)
        {
            Debug.Log("Teletransportando jugador a la posición: " + targetPosition + " y rotación: " + targetRotation);
            playerInstance.transform.position = targetPosition; // Asigna la posición del Player
            playerInstance.transform.rotation = Quaternion.Euler(targetRotation); // Asigna la rotación del Player
        }
        else
        {
            Debug.LogError("No se encontró la instancia del Player en la escena.");
        }
    }
}




