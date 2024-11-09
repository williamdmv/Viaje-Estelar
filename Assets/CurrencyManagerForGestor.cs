using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManagerForGestor : MonoBehaviour
{
    public GestorCuestionario gestorCuestionario; // Referencia a GestorCuestionario
    public GameObject coinCanvas; // Canvas para mostrar las monedas
    public Text coinText; // Texto para mostrar la cantidad de monedas ganadas en el último intento
    private int totalCoins = 0; // Contador de monedas acumuladas en total
    private bool finalMessageShown = false; // Control de ejecución única por intento

    private string filePath;

    void Start()
    {
        coinCanvas.SetActive(false); // Desactiva el canvas de monedas al inicio

        // Define la ruta del archivo JSON en Assets/Monedas
        filePath = Application.dataPath + "/Monedas/coins.json";

        // Cargar las monedas acumuladas del archivo JSON si existe
        LoadTotalCoins();
    }

    void Update()
    {
        // Detecta si el mensaje final de GestorCuestionario está activo y si aún no se ha mostrado el mensaje final
        if (gestorCuestionario.textoMensajeFinal.gameObject.activeSelf && !finalMessageShown)
        {
            finalMessageShown = true; // Asegura que AwardCoins solo se llama una vez
            AwardCoins();
        }
    }

    // Método para calcular las monedas y activar el canvas
    public void AwardCoins()
    {
        int correctAnswers = gestorCuestionario.GetCorrectAnswerCount();

        // Calcula las monedas obtenidas en el último intento (2 monedas por respuesta correcta)
        int earnedCoins = correctAnswers * 2;
        
        // Actualiza el texto solo con las monedas ganadas en este intento
        coinText.text = "Haz ganado " + earnedCoins + " monedas";

        // Sumar las monedas ganadas al total acumulado y guardarlas en el archivo JSON
        totalCoins += earnedCoins;
        SaveTotalCoins();

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
        finalMessageShown = false; // Permite que se muestren las monedas nuevamente en el próximo intento
    }

    // Método para guardar el total de monedas acumuladas en un archivo JSON
    private void SaveTotalCoins()
    {
        // Crea la carpeta si no existe
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        // Guarda el total acumulado de monedas en formato JSON
        File.WriteAllText(filePath, JsonUtility.ToJson(new CoinData { coins = this.totalCoins }));
    }

    // Método para cargar el total de monedas acumuladas desde el archivo JSON
    private void LoadTotalCoins()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            CoinData data = JsonUtility.FromJson<CoinData>(json);
            this.totalCoins = data.coins;
        }
    }

    // Clase auxiliar para guardar los datos en formato JSON
    [System.Serializable]
    private class CoinData
    {
        public int coins;
    }
}




