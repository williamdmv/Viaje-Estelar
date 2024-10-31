using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravedarlunar : MonoBehaviour
{
     private Vector3 originalGravity;
    public float lunarGravity = 1.62f; // Gravedad de la Luna

    void OnEnable()
    {
        // Guardar la gravedad original y establecer la gravedad lunar
        originalGravity = Physics.gravity;
        Physics.gravity = Vector3.down * lunarGravity;
    }

    void OnDisable()
    {
        // Restaurar la gravedad original al salir de la escena
        Physics.gravity = originalGravity;
    }
}
