using UnityEngine;
using Valve.VR.Extras;

public class LaserPointerTest : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer; // Asigna el puntero láser desde el inspector

    void Awake()
    {
        // Suscribirnos al evento del puntero láser
        laserPointer.PointerClick += OnPointerClick;
        laserPointer.PointerIn += OnPointerIn;
        laserPointer.PointerOut += OnPointerOut;
    }

    private void OnPointerClick(object sender, PointerEventArgs e)
    {
        // Detectar si el objeto clicado es un botón
        var button = e.target.GetComponent<UnityEngine.UI.Button>();
        if (button != null)
        {
            button.onClick.Invoke(); // Ejecutar el evento onClick del botón
            Debug.Log("Botón clicado: " + e.target.name);
        }
    }

    private void OnPointerIn(object sender, PointerEventArgs e)
    {
        // Cuando el puntero entra en un botón, cambia su color a amarillo
        var button = e.target.GetComponent<UnityEngine.UI.Image>();
        if (button != null)
        {
            button.color = Color.yellow;
            Debug.Log("Puntero sobre: " + e.target.name);
        }
    }

    private void OnPointerOut(object sender, PointerEventArgs e)
    {
        // Cuando el puntero sale de un botón, restablece su color
        var button = e.target.GetComponent<UnityEngine.UI.Image>();
        if (button != null)
        {
            button.color = Color.white;
        }
    }
}
