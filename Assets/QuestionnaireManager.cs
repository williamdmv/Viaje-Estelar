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
    public Text resultsText; // Texto en el mismo canvas donde se mostrarán los resultados
    public Text finalMessageText; // Texto para el mensaje final que se mostrará después de los resultados
    public string finalMessage = "Gracias por participar"; // Mensaje que aparecerá al final

    private int currentQuestionIndex = 0; // Índice de la pregunta actual
    private bool hasAnswered = false; // Bandera para controlar si ya se respondió la pregunta
    private List<string> questionResults = new List<string>(); // Lista que almacena los resultados de cada pregunta

    void Start()
    {
        resultsText.gameObject.SetActive(false); // Asegúrate de que el texto de resultados esté desactivado al inicio
        finalMessageText.gameObject.SetActive(false); // Asegúrate de que el mensaje final esté desactivado al inicio
        DisplayQuestion(); // Muestra la primera pregunta al inicio
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

                    // Cambia el texto del botón
                    Text legacyText = optionButtons[i].GetComponentInChildren<Text>();
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
            ShowResults(); // Mostrar la pantalla de resultados al final del cuestionario
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
                questionResults.Add("Pregunta " + (currentQuestionIndex + 1) + ": Correcta");
            }
            else
            {
                Debug.Log("Respuesta incorrecta.");
                questionResults.Add("Pregunta " + (currentQuestionIndex + 1) + ": Incorrecta");
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

    // Método para mostrar la pantalla de resultados
    private void ShowResults()
    {
        // Desactivar los botones de opciones y el texto de la pregunta
        questionTextUI.gameObject.SetActive(false);
        foreach (var button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Activar el texto de resultados
        resultsText.gameObject.SetActive(true);

        // Construimos el texto con los resultados de cada pregunta
        string resultsSummary = "Resultados del Cuestionario:\n";
        foreach (string result in questionResults)
        {
            resultsSummary += result + "\n";
        }

        resultsText.text = resultsSummary; // Mostramos los resultados en el Text UI del canvas

        // Iniciar la corrutina para mostrar el mensaje final después de 7 segundos
        StartCoroutine(ShowFinalMessage());
    }

    // Corrutina para mostrar el mensaje final después de 7 segundos
    private IEnumerator ShowFinalMessage()
    {
        yield return new WaitForSeconds(7); // Esperamos 7 segundos

        // Desactivar el texto de resultados
        resultsText.gameObject.SetActive(false);

        // Activar el texto del mensaje final, que se puede configurar desde el inspector
        finalMessageText.gameObject.SetActive(true);
        finalMessageText.text = finalMessage; // Mostrar el mensaje configurado desde el Inspector
    }
}



