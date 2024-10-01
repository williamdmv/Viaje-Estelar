using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class VRLaunchAngleDisplay : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // LeftHand o RightHand
    public SteamVR_Behaviour_Pose controllerPose; // Componente para obtener la rotación del controlador
    public Text angleText; // Texto UI para mostrar el ángulo
    public Transform launchPoint; // Punto desde donde se calcula el lanzamiento

    // Acción de agarre para verificar si se está tomando un objeto (debes tener configurado el input de SteamVR)
    public SteamVR_Action_Boolean grabAction; 

    void Update()
    {
        // Verificar si el controlador ha agarrado un objeto
        if (grabAction.GetState(handType))
        {
            // Obtener la dirección a la que está apuntando el controlador (su forward vector)
            Vector3 controllerForward = controllerPose.transform.forward;

            // Calcular el ángulo entre la dirección hacia arriba (Vector3.up) y la dirección del controlador
            float angle = Vector3.Angle(Vector3.up, controllerForward);

            // Convertir el ángulo a entero y mostrar en el texto UI
            angleText.text = "Angle: " + Mathf.RoundToInt(angle) + "°";

            // Asegurarse de que el texto esté activo cuando se agarra un objeto
            angleText.gameObject.SetActive(true);
        }
        else
        {
            // Si no se está agarrando un objeto, ocultar el texto del ángulo
            angleText.gameObject.SetActive(false);
        }

        // Depuración para verificar la dirección del controlador y el ángulo
        Debug.Log("Controller Forward Vector: " + controllerPose.transform.forward);
    }
}


