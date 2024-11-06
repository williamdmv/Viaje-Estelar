using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public QuestionnaireManager questionnaireManager; // Referencia a QuestionnaireManager
    public GameObject coinCanvas; // Canvas para mostrar las monedas
    public Text coinText; // Texto para mostrar la cantidad de monedas
    private int coins = 0; // Contador de monedas
    private bool finalMessageShown = false; // Para asegurar que solo se ejecute una vez

    void Start()
    {
        coinCanvas.SetActive(false); // Desactiva el canvas de monedas al inicio
    }

    void Update()
    {
        // Detecta si el mensaje final de QuestionnaireManager está activo y si aún no se ha mostrado el mensaje final
        if (questionnaireManager.finalMessageText.gameObject.activeSelf && !finalMessageShown)
        {
            finalMessageShown = true;
            AwardCoins();
        }
    }

    // Método para calcular las monedas y activar el canvas
    private void AwardCoins()
    {
        int correctAnswers = questionnaireManager.GetCorrectAnswerCount();

        // Calcula las monedas (2 monedas por respuesta correcta)
        coins += correctAnswers * 2;
        coinText.text = "Monedas: " + coins;

        // Activa el canvas de monedas y lo desactiva después de 5 segundos
        StartCoroutine(ShowCoinCanvas());
    }

    private IEnumerator ShowCoinCanvas()
    {
        coinCanvas.SetActive(true);
        yield return new WaitForSeconds(5);
        coinCanvas.SetActive(false);
    }
}
