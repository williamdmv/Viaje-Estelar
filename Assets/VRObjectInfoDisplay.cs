using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VRObjectInfoDisplay : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // LeftHand o RightHand
    public SteamVR_Behaviour_Pose controllerPose; // Componente del controlador
    public SteamVR_Action_Boolean grabAction; // Acción de agarre para detectar si un objeto es agarrado

    // Textos UI para mostrar masa, peso y gravedad
    public Text massText;
    public Text weightText;
    public Text gravityText;

    private Interactable currentInteractable; // Objeto interactivo actual
    private ObjectProperties currentObjectProperties; // Propiedades del objeto actual

    void Update()
    {
        // Verificar si se está agarrando un objeto
        if (grabAction.GetState(handType))
        {
            // Obtener el objeto interactivo que está siendo agarrado
            currentInteractable = FindObjectOfType<Hand>().currentAttachedObject?.GetComponent<Interactable>();

            if (currentInteractable != null)
            {
                // Obtener las propiedades del objeto agarrado
                currentObjectProperties = currentInteractable.GetComponent<ObjectProperties>();

                if (currentObjectProperties != null)
                {
                    // Mostrar los datos del objeto en los textos con dos decimales
                    massText.text = "Masa: " + currentObjectProperties.mass.ToString("F2") + " gramos";
                    weightText.text = "Peso: " + currentObjectProperties.weight.ToString("F2") + " newtons";
                    gravityText.text = "Gravedad: " + currentObjectProperties.gravity.ToString("F2") + " m/s²";

                    // Activar los textos
                    massText.gameObject.SetActive(true);
                    weightText.gameObject.SetActive(true);
                    gravityText.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            // Si no se está agarrando ningún objeto, desactivar los textos
            massText.gameObject.SetActive(false);
            weightText.gameObject.SetActive(false);
            gravityText.gameObject.SetActive(false);

            // Limpiar la referencia al objeto interactivo
            currentObjectProperties = null;
        }
    }
}
