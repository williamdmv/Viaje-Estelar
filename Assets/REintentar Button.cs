using UnityEngine;
using Valve.VR.InteractionSystem;

public class REintentarButton : MonoBehaviour
{
    public GestorCuestionario gestorCuestionario; // Referencia al GestorCuestionario para reiniciar el cuestionario

    // Este evento se activa cuando la mano toca el botón de reintento
    private void OnHandHoverBegin(Hand hand)
    {
        if (hand != null && gestorCuestionario != null)
        {
            gestorCuestionario.RetryQuiz(); // Llama al método para reiniciar el cuestionario
            //gameObject.SetActive(false); // Oculta el botón de reintento después de tocarlo, si es necesario
        }
    }
}