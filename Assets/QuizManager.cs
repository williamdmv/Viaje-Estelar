using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Asegurarse de que el espacio de nombres UI está importado.

public class QuizManager : MonoBehaviour
{
   [System.Serializable]
   public class QuestionAndAnswer
   {
      public string Question;
      public string[] Answers;
      public int CorrectAnswer;
   }

   public List<QuestionAndAnswer> QnA;  // Lista de preguntas y respuestas.
   public GameObject[] options;         // Array de opciones (botones).
   public Text QuestionTxt;             // Texto de la pregunta.

   private int currentQuestion;

   private void Start()
   {
      generateQuestion();  // Genera la primera pregunta al inicio.
   }

   // Método que se ejecuta cuando se responde correctamente
   public void correct()
   {
      if (QnA.Count > 0)  // Verifica que aún haya preguntas.
      {
         QnA.RemoveAt(currentQuestion);  // Elimina la pregunta actual.
         generateQuestion();             // Genera una nueva pregunta.
      }
      else
      {
         Debug.Log("No more questions available.");
      }
   }

   // Método para configurar las respuestas en los botones.
   void SetAnswers()
   {
      for (int i = 0; i < options.Length; i++)
      {
         // Verifica si el índice de la respuesta es válido.
         if (i < QnA[currentQuestion].Answers.Length)
         {
            // Limpia la configuración anterior de la respuesta correcta.
            options[i].GetComponent<AnswerScript>().isCorrect = false;

            // Intenta obtener el componente Text del botón.
            Text answerText = options[i].GetComponentInChildren<Text>();

            if (answerText != null)  // Verifica si encontró el componente Text.
            {
               answerText.text = QnA[currentQuestion].Answers[i];  // Asigna el texto de la respuesta.
            }
            else
            {
               Debug.LogError("No se encontró el componente Text en el botón " + i);
            }

            // Marca la respuesta correcta.
            if (QnA[currentQuestion].CorrectAnswer == i)
            {
               options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
         }
         else
         {
            Debug.LogError("El índice de la respuesta " + i + " excede el tamaño de la lista de respuestas.");
         }
      }
   }

   // Método para generar una nueva pregunta.
   void generateQuestion()
   {
      if (QnA.Count > 0)
      {
         currentQuestion = Random.Range(0, QnA.Count);  // Selecciona una pregunta aleatoriamente.
         QuestionTxt.text = QnA[currentQuestion].Question;  // Asigna el texto de la pregunta.
         SetAnswers();  // Configura las respuestas.
      }
      else
      {
         Debug.Log("No more questions to display.");
      }
   }
}



