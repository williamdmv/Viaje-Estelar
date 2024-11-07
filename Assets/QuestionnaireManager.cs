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
    public GameObject objectToActivate; // Objeto que se activará si todas las respuestas son correctas
    public GameObject objectToActivateOnFailure; // Objeto que se activará si al menos una respuesta es incorrecta

    private int currentQuestionIndex = 0; // Índice de la pregunta actual
    private bool hasAnswered = false; // Bandera para controlar si ya se respondió la pregunta
    private bool hasIncorrectAnswer = false; // Bandera para verificar si se ha respondido alguna pregunta incorrectamente
    private List<string> questionResults = new List<string>(); // Lista que almacena los resultados de cada pregunta
    

    void Start()
    {
        resultsText.gameObject.SetActive(false); // Asegúrate de que el texto de resultados esté desactivado al inicio
        finalMessageText.gameObject.SetActive(false); // Asegúrate de que el mensaje final esté desactivado al inicio
        objectToActivate.SetActive(false); // Asegúrate de que el objeto esté desactivado al inicio
        objectToActivateOnFailure.SetActive(false); // Asegúrate de que el objeto de fallo esté desactivado al inicio
        DisplayQuestion(); // Muestra la primera pregunta al inicio
    }

    // Método para mostrar la pregunta actual y sus opciones
    public void DisplayQuestion()
    {
        hasAnswered = false;
        EnableButtons();

        if (currentQuestionIndex < questions.Count)
        {
            QuizQuestion currentQuestion = questions[currentQuestionIndex];
            questionTextUI.text = currentQuestion.questionText; // Mostrar la pregunta en el Text UI

            // Mostrar las opciones en los botones
            for (int i = 0; i < optionButtons.Count; i++)
            {
                optionButtons[i].onClick.RemoveAllListeners(); // Eliminamos los listeners previos

                if (i < currentQuestion.options.Count)
                {
                    optionButtons[i].gameObject.SetActive(true); // Activa el botón si hay opción
                    Text legacyText = optionButtons[i].GetComponentInChildren<Text>();
                    if (legacyText != null)
                    {
                        legacyText.text = currentQuestion.options[i].optionText;
                    }

                    int index = i;
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
            hasAnswered = true;
            QuizQuestion currentQuestion = questions[currentQuestionIndex];

            if (optionIndex == currentQuestion.correctOptionIndex)
            {
                questionResults.Add("Pregunta " + (currentQuestionIndex + 1) + ": Correcta");
            }
            else
            {
                hasIncorrectAnswer = true; // Marcamos que hubo una respuesta incorrecta
                questionResults.Add("Pregunta " + (currentQuestionIndex + 1) + ": Incorrecta");
            }

            StartCoroutine(DisableButtonsTemporarily());
        }
    }

    private IEnumerator DisableButtonsTemporarily()
    {
        DisableButtons();
        yield return new WaitForSeconds(buttonDisableDuration);
        currentQuestionIndex++;
        DisplayQuestion();
    }

    private void DisableButtons()
    {
        foreach (var button in optionButtons)
        {
            button.interactable = false;
        }
    }

    private void EnableButtons()
    {
        foreach (var button in optionButtons)
        {
            button.interactable = true;
        }
    }

    private void ShowResults()
    {
        questionTextUI.gameObject.SetActive(false);
        foreach (var button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }

        resultsText.gameObject.SetActive(true);
        string resultsSummary = "Resultados del Cuestionario:\n";
        foreach (string result in questionResults)
        {
            resultsSummary += result + "\n";
        }

        resultsText.text = resultsSummary;

        StartCoroutine(ShowFinalMessage());
    }

    private IEnumerator ShowFinalMessage()
    {
        yield return new WaitForSeconds(7);

        resultsText.gameObject.SetActive(false);
        finalMessageText.gameObject.SetActive(true);
        finalMessageText.text = finalMessage;

        // Si no hubo ninguna respuesta incorrecta, activamos el primer objeto
        if (!hasIncorrectAnswer)
        {
            objectToActivate.SetActive(true);
        }
        else
        {
            // Si hubo al menos una respuesta incorrecta, activamos el segundo objeto
            objectToActivateOnFailure.SetActive(true);
        }
    }

        public void RetryQuiz()
    {
        currentQuestionIndex = 0;
        hasIncorrectAnswer = false;
        questionResults.Clear();
        resultsText.gameObject.SetActive(false);
        finalMessageText.gameObject.SetActive(false);
        objectToActivate.SetActive(false);
        objectToActivateOnFailure.SetActive(false);
        questionTextUI.gameObject.SetActive(true);

    foreach (var button in optionButtons)
    {
        button.gameObject.SetActive(true);
    }

    DisplayQuestion(); // Muestra la primera pregunta nuevamente
}

// Se Agrega este método en el script para el sistema de monedas (no afecta al resto del codigo, sino al sistema de monedas)
public int GetCorrectAnswerCount()
{
    int correctCount = 0;

    foreach (string result in questionResults)
    {
        if (result.Contains("Correcta"))
        {
            correctCount++;
        }
    }

    return correctCount;
}

}




