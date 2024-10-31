using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem; 

public class VrButtonSync2 : MonoBehaviour
{
   public int indiceOpcion; // El índice de la opción (sincronizado con el canvas)
    public Button uiButton; // Referencia al botón de la UI del canvas
    public GestorCuestionario gestorcuestionario; // Referencia al script del cuestionario

    private void OnHandHoverBegin(Hand hand) // Este evento se activa cuando la mano toca el botón
    {
        if (hand != null)
        {
            // Simula que el botón UI ha sido presionado
            if (uiButton != null)
            {
                uiButton.onClick.Invoke(); // Activa el evento del botón del canvas
            }

            // Simula la selección de la opción en el cuestionario
            gestorcuestionario.SeleccionarOpcion(indiceOpcion);
        }
    }
}
