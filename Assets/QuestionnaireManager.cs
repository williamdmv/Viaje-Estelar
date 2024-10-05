using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class QuizOption
{
    public string optionText; // Texto de la opción
}

[System.Serializable]
public class QuizQuestion
{
    public string questionText; // Texto de la pregunta
    public List<QuizOption> options; // Lista de opciones
    public int correctOptionIndex; // Índice de la respuesta correcta (basado en la lista de opciones)
}

public class QuestionnaireManager : MonoBehaviour
{
    public List<QuizQuestion> questions; // Lista de preguntas que se pueden configurar desde el inspector
    public Text questionTextUI; // El texto que mostrará la pregunta en la UI
    public List<Button> optionButtons; // Lista de botones que representan las opciones
    public float buttonDisableDuration = 1.5f; // Duración del bloqueo de los botones en segundos

    private int currentQuestionIndex = 0; // Índice de la pregunta actual
    private bool hasAnswered = false; // Bandera para controlar si ya se respondió la pregunta

    void Start()
    {
        DisplayQuestion();
    }

    // Método para mostrar la pregunta actual y sus opciones
    public void DisplayQuestion()
    {
        // Restablecemos la bandera de interacción y activamos los botones
        hasAnswered = false;
        EnableButtons();

        if (currentQuestionIndex < questions.Count)
        {
            QuizQuestion currentQuestion = questions[currentQuestionIndex];
            Debug.Log("Mostrando pregunta " + (currentQuestionIndex + 1) + ": " + currentQuestion.questionText);

            questionTextUI.text = currentQuestion.questionText; // Mostrar la pregunta en el Text UI

            // Mostrar las opciones en los botones
            for (int i = 0; i < optionButtons.Count; i++)
            {
                optionButtons[i].onClick.RemoveAllListeners(); // Eliminamos los listeners previos
                
                if (i < currentQuestion.options.Count)
                {
                    optionButtons[i].gameObject.SetActive(true); // Activa el botón si hay opción

                    // Cambia el texto del botón para sistemas legacy
                    Text legacyText = optionButtons[i].GetComponentInChildren<Text>(); // Asegúrate de que el componente Text legacy está en los hijos del botón
                    if (legacyText != null)
                    {
                        legacyText.text = currentQuestion.options[i].optionText;
                    }
                    else
                    {
                        Debug.LogError("No se encontró un componente Text en el botón.");
                    }

                    int index = i; // Captura el índice para evitar problemas de referencia en el closure
                    optionButtons[i].onClick.AddListener(() => SelectOption(index)); // Asigna el índice correcto al botón
                }
                else
                {
                    optionButtons[i].gameObject.SetActive(false); // Oculta los botones no usados
                }
            }
        }
        else
        {
            Debug.Log("No hay más preguntas.");
        }
    }

    // Método para seleccionar una opción
    public void SelectOption(int optionIndex)
    {
        if (!hasAnswered) // Verificamos si ya se respondió la pregunta
        {
            hasAnswered = true; // Marcamos que ya se respondió la pregunta
            QuizQuestion currentQuestion = questions[currentQuestionIndex];
            Debug.Log("Pregunta " + (currentQuestionIndex + 1) + ": opción seleccionada: " + optionIndex);

            // Verifica si la opción seleccionada es la correcta
            if (optionIndex == currentQuestion.correctOptionIndex)
            {
                Debug.Log("¡Respuesta correcta!");
            }
            else
            {
                Debug.Log("Respuesta incorrecta.");
            }

            // Deshabilitamos los botones después de la primera respuesta
            StartCoroutine(DisableButtonsTemporarily());
        }
        else
        {
            Debug.Log("Ya has respondido esta pregunta. Espera para la próxima.");
        }
    }

    // Corrutina para deshabilitar los botones y reactivarlos después de un pequeño tiempo
    private IEnumerator DisableButtonsTemporarily()
    {
        DisableButtons(); // Desactivamos los botones
        yield return new WaitForSeconds(buttonDisableDuration); // Esperamos el tiempo especificado
        currentQuestionIndex++; // Avanzamos a la siguiente pregunta
        DisplayQuestion(); // Mostramos la siguiente pregunta
    }

    // Método para deshabilitar todos los botones
    private void DisableButtons()
    {
        foreach (var button in optionButtons)
        {
            button.interactable = false;
        }
    }

    // Método para habilitar todos los botones
    private void EnableButtons()
    {
        foreach (var button in optionButtons)
        {
            button.interactable = true;
        }
    }
}

