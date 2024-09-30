using UnityEngine;
using Valve.VR;

public class HandCollision : MonoBehaviour
{
    public SteamVR_Action_Boolean grabAction; // Acción de SteamVR configurada para agarrar (Primary button o trigger)
    public SteamVR_Input_Sources handType;    // Tipo de mano (LeftHand o RightHand)
    public Transform handTransform;           // Asigna aquí el transform de la mano

    private bool isHoldingObject = false;
    private GameObject heldObject;
    private FixedJoint joint; // Añadir el FixedJoint para manejar las físicas

    void Awake()
    {
        // Añadir el FixedJoint a la mano (puedes ajustarlo según necesites)
        joint = handTransform.gameObject.AddComponent<FixedJoint>();
        joint.breakForce = 2000;  // Fuerza máxima antes de que se rompa el agarre
        joint.breakTorque = 2000;
        joint.connectedBody = null; // Sin conexión al iniciar
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("La mano ha colisionado con: " + collision.gameObject.name);

        // Si la mano colisiona con un objeto grabbable y no estamos sosteniendo nada
        if (!isHoldingObject && collision.gameObject.CompareTag("Grabbable"))
        {
            // Verificar si el botón de agarre está siendo presionado
            if (grabAction.GetState(handType))
            {
                Debug.Log("Botón de agarre presionado."); // Añadido para verificar el botón
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    AttachObject(rb);  // Usar FixedJoint para agarrar el objeto
                }
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("La mano sigue colisionando con: " + collision.gameObject.name);

        // Verificamos si se debe soltar el objeto durante la colisión
        if (isHoldingObject && grabAction.GetStateUp(handType))
        {
            ReleaseObject();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("La mano ha dejado de colisionar con: " + collision.gameObject.name);
    }

    private void AttachObject(Rigidbody rb)
    {
        // Conectar el objeto al FixedJoint
        heldObject = rb.gameObject;
        joint.connectedBody = rb;
        Debug.Log("Objeto conectado: " + heldObject.name);
        isHoldingObject = true;
    }

    private void ReleaseObject()
    {
        if (heldObject != null)
        {
            // Desconectar el FixedJoint y restaurar las físicas
            joint.connectedBody = null;

            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Opcional: ajustar velocidad angular o lineal del objeto al soltarlo
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            heldObject = null;
            isHoldingObject = false;
        }
    }
}