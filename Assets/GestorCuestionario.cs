using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class OpcionPregunta
{
    public string textoOpcion; // Texto de la opción
}

[System.Serializable]
public class PreguntaCuestionario
{
    public string textoPregunta; // Texto de la pregunta
    public List<OpcionPregunta> opciones; // Lista de opciones
    public int indiceOpcionCorrecta; // Índice de la respuesta correcta (basado en la lista de opciones)
}

public class GestorCuestionario : MonoBehaviour
{
    public List<PreguntaCuestionario> listaPreguntas; // Lista de preguntas que se pueden configurar desde el inspector
    public Text textoPreguntaUI; // El texto que mostrará la pregunta en la UI
    public List<Button> botonesOpciones; // Lista de botones que representan las opciones
    public float duracionDeshabilitarBotones = 1.5f; // Duración del bloqueo de los botones en segundos
    public Text textoResultados; // Texto en el mismo canvas donde se mostrarán los resultados
    public Text textoMensajeFinal; // Texto para el mensaje final que se mostrará después de los resultados
    public string mensajeFinal = "Gracias por participar"; // Mensaje que aparecerá al final
    public GameObject objetoActivarExito; // Objeto que se activará si todas las respuestas son correctas
    public GameObject objetoActivarFallo; // Objeto que se activará si al menos una respuesta es incorrecta

    private int indicePreguntaActual = 0; // Índice de la pregunta actual
    private bool respuestaDada = false; // Bandera para controlar si ya se respondió la pregunta
    private bool respuestaIncorrecta = false; // Bandera para verificar si se ha respondido alguna pregunta incorrectamente
    private List<string> resultadosPreguntas = new List<string>(); // Lista que almacena los resultados de cada pregunta
   public CurrencyManagerForGestor currencyManagerforgestor; // Referencia a CurrencyManagerForGestor


    void Start()
    {
        textoResultados.gameObject.SetActive(false); // Asegúrate de que el texto de resultados esté desactivado al inicio
        textoMensajeFinal.gameObject.SetActive(false); // Asegúrate de que el mensaje final esté desactivado al inicio
        objetoActivarExito.SetActive(false); // Asegúrate de que el objeto esté desactivado al inicio
        objetoActivarFallo.SetActive(false); // Asegúrate de que el objeto de fallo esté desactivado al inicio
        MostrarPregunta(); // Muestra la primera pregunta al inicio
    }

    // Método para mostrar la pregunta actual y sus opciones
    public void MostrarPregunta()
    {
        respuestaDada = false;
        HabilitarBotones();

        if (indicePreguntaActual < listaPreguntas.Count)
        {
            PreguntaCuestionario preguntaActual = listaPreguntas[indicePreguntaActual];
            textoPreguntaUI.text = preguntaActual.textoPregunta; // Mostrar la pregunta en el Text UI

            // Mostrar las opciones en los botones
            for (int i = 0; i < botonesOpciones.Count; i++)
            {
                botonesOpciones[i].onClick.RemoveAllListeners(); // Eliminamos los listeners previos

                if (i < preguntaActual.opciones.Count)
                {
                    botonesOpciones[i].gameObject.SetActive(true); // Activa el botón si hay opción
                    Text textoBoton = botonesOpciones[i].GetComponentInChildren<Text>();
                    if (textoBoton != null)
                    {
                        textoBoton.text = preguntaActual.opciones[i].textoOpcion;
                    }

                    int indice = i;
                    botonesOpciones[i].onClick.AddListener(() => SeleccionarOpcion(indice)); // Asigna el índice correcto al botón
                }
                else
                {
                    botonesOpciones[i].gameObject.SetActive(false); // Oculta los botones no usados
                }
            }
        }
        else
        {
            MostrarResultados(); // Mostrar la pantalla de resultados al final del cuestionario
        }
    }

    // Método para seleccionar una opción
    public void SeleccionarOpcion(int indiceOpcion)
    {
        if (!respuestaDada) // Verificamos si ya se respondió la pregunta
        {
            respuestaDada = true;
            PreguntaCuestionario preguntaActual = listaPreguntas[indicePreguntaActual];

            if (indiceOpcion == preguntaActual.indiceOpcionCorrecta)
            {
                resultadosPreguntas.Add("Pregunta " + (indicePreguntaActual + 1) + ": Correcta");
            }
            else
            {
                respuestaIncorrecta = true; // Marcamos que hubo una respuesta incorrecta
                resultadosPreguntas.Add("Pregunta " + (indicePreguntaActual + 1) + ": Incorrecta");
            }

            StartCoroutine(DeshabilitarBotonesTemporalmente());
        }
    }

    private IEnumerator DeshabilitarBotonesTemporalmente()
    {
        DeshabilitarBotones();
        yield return new WaitForSeconds(duracionDeshabilitarBotones);
        indicePreguntaActual++;
        MostrarPregunta();
    }

    private void DeshabilitarBotones()
    {
        foreach (var boton in botonesOpciones)
        {
            boton.interactable = false;
        }
    }

    private void HabilitarBotones()
    {
        foreach (var boton in botonesOpciones)
        {
            boton.interactable = true;
        }
    }

    private void MostrarResultados()
    {
        textoPreguntaUI.gameObject.SetActive(false);
        foreach (var boton in botonesOpciones)
        {
            boton.gameObject.SetActive(false);
        }

        textoResultados.gameObject.SetActive(true);
        string resumenResultados = "Resultados del Cuestionario:\n";
        foreach (string resultado in resultadosPreguntas)
        {
            resumenResultados += resultado + "\n";
        }

        textoResultados.text = resumenResultados;

        StartCoroutine(MostrarMensajeFinal());
    }

     private IEnumerator MostrarMensajeFinal()
    {
        yield return new WaitForSeconds(7);

    textoResultados.gameObject.SetActive(false);
    textoMensajeFinal.gameObject.SetActive(true);

    // Mostrar mensaje final si no está vacío
    if (!string.IsNullOrEmpty(mensajeFinal))
    {
        textoMensajeFinal.text = mensajeFinal;
    }
    else
    {
        textoMensajeFinal.text = ""; // Si está vacío, dejar sin texto
    }

    // Mostrar monedas
    if (currencyManagerforgestor != null)
    {
        currencyManagerforgestor.AwardCoins();
    }

    // Activar objeto de éxito o fallo
    if (!respuestaIncorrecta)
    {
        objetoActivarExito.SetActive(true);
    }
    else
    {
        objetoActivarFallo.SetActive(true);
    }

    // Esperar 7 segundos y mostrar el siguiente mensaje
    yield return new WaitForSeconds(10);
    textoMensajeFinal.text = "¡Fin del cuestionario, astronauta. Haz llegado al final de la aventura!"; // Cambia al segundo mensaje
    }

    public void RetryQuiz()
{
    indicePreguntaActual = 0;
    respuestaIncorrecta = false;
    resultadosPreguntas.Clear();
    textoResultados.gameObject.SetActive(false);
    textoMensajeFinal.gameObject.SetActive(false);
    objetoActivarExito.SetActive(false);
    objetoActivarFallo.SetActive(false);
    textoPreguntaUI.gameObject.SetActive(true);

    foreach (var boton in botonesOpciones)
    {
        boton.gameObject.SetActive(true);
    }

     if (currencyManagerforgestor != null)
    {
        currencyManagerforgestor.ResetFinalMessage();
    }

    MostrarPregunta(); 
}

public int GetCorrectAnswerCount()
{
    int correctCount = 0;

    foreach (string resultado in resultadosPreguntas)
    {
        if (resultado.Contains("Correcta"))
        {
            correctCount++;
        }
    }

    return correctCount;
}


}
