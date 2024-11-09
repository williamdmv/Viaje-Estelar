using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public QuestionnaireManager questionnaireManager; // Referencia a QuestionnaireManager
    public GameObject coinCanvas; // Canvas para mostrar las monedas
    public Text coinText; // Texto para mostrar la cantidad de monedas
    private int coins = 0; // Contador de monedas
    private bool finalMessageShown = false; // Para asegurar que solo se ejecute una vez

    private string filePath;

    void Start()
    {
        coinCanvas.SetActive(false); // Desactiva el canvas de monedas al inicio

        // Define la ruta del archivo JSON en Assets/Monedas
        filePath = Application.dataPath + "/Monedas/coins.json";

        // Cargar las monedas del archivo JSON si existe
        LoadCoins();
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
        coins = correctAnswers * 2;
        coinText.text = "Monedas: " + coins;

        // Guardar las monedas en el archivo JSON
        SaveCoins();

        // Activa el canvas de monedas y lo desactiva después de 5 segundos
        StartCoroutine(ShowCoinCanvas());
    }

    private IEnumerator ShowCoinCanvas()
    {
        coinCanvas.SetActive(true);
        yield return new WaitForSeconds(5);
        coinCanvas.SetActive(false);
    }

    // Método para reiniciar la visualización de monedas cuando se reinicia el quiz
    public void ResetFinalMessage()
    {
        finalMessageShown = false; // Permite que se muestren las monedas después de un reinicio
    }

    // Método para guardar las monedas en un archivo JSON
    private void SaveCoins()
    {
        // Crea la carpeta si no existe
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        // Guarda las monedas en formato JSON
        File.WriteAllText(filePath, JsonUtility.ToJson(new CoinData { coins = this.coins }));
    }

    // Método para cargar las monedas desde el archivo JSON
    private void LoadCoins()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            CoinData data = JsonUtility.FromJson<CoinData>(json);
            this.coins = data.coins;
            coinText.text = "Monedas: " + coins;
        }
    }

    // Clase auxiliar para guardar los datos en formato JSON
    [System.Serializable]
    private class CoinData
    {
        public int coins;
    }
}

