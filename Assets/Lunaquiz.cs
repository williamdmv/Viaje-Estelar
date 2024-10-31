using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Asegurarse de que el espacio de nombres UI está importado.

public class Lunaquiz : MonoBehaviour
{
   [System.Serializable]
   public class PreguntaYRespuesta
   {
      public string Pregunta;
      public string[] Respuestas;
      public int RespuestaCorrecta;
   }

   public List<PreguntaYRespuesta> ListaPreguntas;  // Lista de preguntas y respuestas.
   public Text[] Opciones;                          // Array de textos de opciones (Text de cada botón).
   public Text TextoPregunta;                       // Texto de la pregunta.

   private int preguntaActual;

   private void Start()
   {
      GenerarPregunta();  // Genera la primera pregunta al inicio.
   }

   // Método que se ejecuta cuando se responde correctamente
   public void RespuestaCorrecta()
   {
      if (ListaPreguntas.Count > 0)  // Verifica que aún haya preguntas.
      {
         ListaPreguntas.RemoveAt(preguntaActual);  // Elimina la pregunta actual.
         GenerarPregunta();                        // Genera una nueva pregunta.
      }
      else
      {
         Debug.Log("No hay más preguntas disponibles.");
      }
   }

   // Método para configurar las respuestas en los textos de los botones.
   void ConfigurarRespuestas()
   {
      for (int i = 0; i < Opciones.Length; i++)
      {
         // Verifica si el índice de la respuesta es válido.
         if (i < ListaPreguntas[preguntaActual].Respuestas.Length)
         {
            // Asigna el texto de la respuesta.
            Opciones[i].text = ListaPreguntas[preguntaActual].Respuestas[i];
            
            // Configura la respuesta correcta en el script del botón.
            Opciones[i].GetComponentInParent<AnswerScript>().isCorrect = 
                (ListaPreguntas[preguntaActual].RespuestaCorrecta == i);
         }
         else
         {
            Debug.LogError("El índice de la respuesta " + i + " excede el tamaño de la lista de respuestas.");
         }
      }
   }

   // Método para generar una nueva pregunta.
   void GenerarPregunta()
   {
      if (ListaPreguntas.Count > 0)
      {
         preguntaActual = Random.Range(0, ListaPreguntas.Count);  // Selecciona una pregunta aleatoriamente.
         TextoPregunta.text = ListaPreguntas[preguntaActual].Pregunta;  // Asigna el texto de la pregunta.
         ConfigurarRespuestas();  // Configura las respuestas.
      }
      else
      {
         Debug.Log("No hay más preguntas para mostrar.");
      }
   }
}


