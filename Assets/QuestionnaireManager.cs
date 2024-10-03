using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private int currentQuestionIndex = 0; // Índice de la pregunta actual

    void Start()
    {
        DisplayQuestion();
    }

    // Método para mostrar la pregunta actual y sus opciones
    public void DisplayQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            QuizQuestion currentQuestion = questions[currentQuestionIndex];
            questionTextUI.text = currentQuestion.questionText; // Mostrar la pregunta en el Text UI

            // Mostrar las opciones en los botones
            for (int i = 0; i < optionButtons.Count; i++)
            {
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
                    optionButtons[i].onClick.RemoveAllListeners();
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
        if (currentQuestionIndex < questions.Count)
        {
            QuizQuestion currentQuestion = questions[currentQuestionIndex];

            // Verifica si la opción seleccionada es la correcta
            if (optionIndex == currentQuestion.correctOptionIndex)
            {
                Debug.Log("¡Respuesta correcta!");
            }
            else
            {
                Debug.Log("Respuesta incorrecta.");
            }

            // Avanza a la siguiente pregunta
            currentQuestionIndex++;
            DisplayQuestion();
        }
        else
        {
            Debug.Log("El cuestionario ha terminado.");
        }
    }
}

