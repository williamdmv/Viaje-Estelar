using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageAudioSequence : MonoBehaviour
{
    [System.Serializable]
    public class ImageAudio
    {
        public Sprite image;       // Imagen a mostrar
        public AudioClip audioClip; // Audio asociado a la imagen
    }

    public Image imageDisplay;       // Referencia al componente Image en el Canvas
    public AudioSource audioSource;  // Referencia al AudioSource para reproducir el audio
    public ImageAudio[] sequence;    // Lista de imágenes y audios para la secuencia
    public float timeBetweenImages = 1f; // Tiempo entre imágenes (opcional)
    public Canvas canvasToDisable;   // Referencia al Canvas que se desactivará al final
    public Canvas canvasToEnable;    // Referencia al Canvas que se activará después

    private int currentIndex = 0;

    void Start()
    {
        // Iniciar la secuencia
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        while (currentIndex < sequence.Length)
        {
            // Mostrar la imagen actual
            imageDisplay.sprite = sequence[currentIndex].image;

            // Reproducir el audio actual
            audioSource.clip = sequence[currentIndex].audioClip;
            audioSource.Play();

            // Esperar hasta que el audio termine de reproducirse
            yield return new WaitWhile(() => audioSource.isPlaying);

            // Esperar un tiempo opcional entre imágenes
            yield return new WaitForSeconds(timeBetweenImages);

            // Pasar a la siguiente imagen en la secuencia
            currentIndex++;
        }

        // Desactivar el Canvas al finalizar la secuencia
        canvasToDisable.gameObject.SetActive(false);

        // Activar el otro Canvas
        canvasToEnable.gameObject.SetActive(true);
    }
}


