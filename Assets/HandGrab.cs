using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;  // Necesario para SteamVR

public class HandGrab : MonoBehaviour
{
    public SteamVR_Action_Boolean grabAction; // Acción de agarrar definida en SteamVR
    public SteamVR_Input_Sources handType;    // La mano (izquierda o derecha)

    public SteamVR_Behaviour_Pose pose; // Componente para obtener la velocidad y rotación

    private GameObject collidingObject;  // El objeto con el que estamos en contacto
    private GameObject objectInHand;     // El objeto que estamos sosteniendo

    void Update()
    {
        // Si estamos presionando el botón de agarre
        if (grabAction.GetStateDown(handType))
        {
            if (collidingObject)
            {
                GrabObject();
            }
        }

        // Si soltamos el botón de agarre
        if (grabAction.GetStateUp(handType))
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto tiene un rigidbody y no estamos sosteniendo nada
        if (!other.GetComponent<Rigidbody>() || objectInHand)
        {
            return;
        }
        collidingObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        // Si salimos del área de colisión del objeto
        if (!collidingObject)
        {
            return;
        }
        collidingObject = null;
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            // Liberar el objeto con la fuerza del movimiento de la mano
            objectInHand.GetComponent<Rigidbody>().velocity = pose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = pose.GetAngularVelocity();
        }
        objectInHand = null;
    }
}

