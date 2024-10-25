using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Valve.VR.InteractionSystem;

public class SceneChangeOnCollision1mala : MonoBehaviour
{
    public GameObject loadingScreen;  // El Canvas que contiene la pantalla de carga
    public VideoPlayer videoPlayer;   // El Video Player que reproduce el video
    //public string sceneToLoad;        // El nombre de la escena que quieres cargar

    private bool isTriggered = false; // Bandera para prevenir múltiples activaciones

    private void Start()
    {
        // Asegúrate de que el Canvas de la pantalla de carga está desactivado al inicio
        loadingScreen.SetActive(false);
    }

    // Este evento se activa cuando la mano toca el botón
    private void OnHandHoverBegin(Hand hand)
    {
        if (hand != null && !isTriggered)
        {
            isTriggered = true;  // Evitar que se active más de una vez
            StartCoroutine(PlayLoadingScreen());
        }
    }

    private IEnumerator PlayLoadingScreen()
    {
        // Activar el Canvas de la pantalla de carga
        loadingScreen.SetActive(true);

        // Iniciar la reproducción del video
        videoPlayer.Play();

        // Esperar hasta que el video termine de reproducirse
        yield return new WaitForSeconds((float)videoPlayer.clip.length);

        // Cargar la nueva escena
        //SceneManager.LoadScene(sceneToLoad);
    }
}


