using UnityEngine;
using Valve.VR.InteractionSystem;

public class RetryButtonHandler : MonoBehaviour
{
    public QuestionnaireManager questionnaireManager; // Referencia al QuestionnaireManager para reiniciar el quiz

    // Este evento se activa cuando la mano toca el botón de reintento
    private void OnHandHoverBegin(Hand hand)
    {
        if (hand != null && questionnaireManager != null)
        {
            questionnaireManager.RetryQuiz(); // Llama al método para reiniciar el quiz
            //gameObject.SetActive(false); // Oculta el botón de reintento después de tocarlo
        }
    }
}
