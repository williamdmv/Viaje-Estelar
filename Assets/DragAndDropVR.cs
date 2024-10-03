using UnityEngine;
using Valve.VR.Extras;

public class DragAndDropVR : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer; // El puntero láser para interacción
    private GameObject selectedButton = null; // El botón que estamos arrastrando
    private bool isDragging = false; // Controla si estamos arrastrando algo

    void Awake()
    {
        // Suscribirse a los eventos del puntero láser
        laserPointer.PointerClick += OnPointerClick;
        laserPointer.PointerIn += OnPointerIn;
        laserPointer.PointerOut += OnPointerOut;
    }

    void Update()
    {
        // Si estamos arrastrando un botón
        if (isDragging && selectedButton != null)
        {
            // Mueve el botón con el puntero láser
            Ray ray = new Ray(laserPointer.transform.position, laserPointer.transform.forward);
            RaycastHit hit;

            // Detecta si el rayo toca una superficie
            if (Physics.Raycast(ray, out hit))
            {
                // Mueve el botón a la posición del rayo
                selectedButton.transform.position = hit.point;
            }
        }
    }

    private void OnPointerClick(object sender, PointerEventArgs e)
    {
        // Si hacemos clic en un botón, lo seleccionamos para arrastrar
        var button = e.target.GetComponent<UnityEngine.UI.Button>();
        if (button != null)
        {
            if (isDragging)
            {
                // Si ya estamos arrastrando, soltamos el botón
                isDragging = false;
                selectedButton = null;
                Debug.Log("Soltando botón");
            }
            else
            {
                // Si no estamos arrastrando, empezamos a arrastrar el botón
                isDragging = true;
                selectedButton = button.gameObject;
                Debug.Log("Arrastrando botón: " + selectedButton.name);
            }
        }
    }

    private void OnPointerIn(object sender, PointerEventArgs e)
    {
        // Cuando el puntero entra en un botón, puedes cambiar el color como feedback
        var button = e.target.GetComponent<UnityEngine.UI.Image>();
        if (button != null)
        {
            button.color = Color.yellow;
        }
    }

    private void OnPointerOut(object sender, PointerEventArgs e)
    {
        // Cuando el puntero sale de un botón, restablece el color
        var button = e.target.GetComponent<UnityEngine.UI.Image>();
        if (button != null)
        {
            button.color = Color.white;
        }
    }
}

