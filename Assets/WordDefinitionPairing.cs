using UnityEngine;
using UnityEngine.UI;

public class WordDefinitionPairing : MonoBehaviour
{
    public Button[] wordButtons; // Array de botones para las palabras
    public Button[] definitionButtons; // Array de botones para las definiciones

    private Button selectedWord = null; // La palabra seleccionada
    private Button selectedDefinition = null; // La definición seleccionada

    public Color selectedColor = Color.yellow; // Color para botón seleccionado
    public Color correctPairColor = Color.green; // Color cuando el emparejamiento es correcto
    public Color incorrectPairColor = Color.red; // Color cuando el emparejamiento es incorrecto

    void Start()
    {
        // Asignar eventos de clic a los botones de palabras
        foreach (Button wordButton in wordButtons)
        {
            wordButton.onClick.AddListener(() => SelectWord(wordButton));
        }

        // Asignar eventos de clic a los botones de definiciones
        foreach (Button definitionButton in definitionButtons)
        {
            definitionButton.onClick.AddListener(() => SelectDefinition(definitionButton));
        }
    }

    // Método que se llama cuando se selecciona una palabra
    void SelectWord(Button wordButton)
    {
        // Resetear la selección previa
        if (selectedWord != null)
        {
            selectedWord.GetComponent<Image>().color = Color.white;
        }

        // Seleccionar la nueva palabra
        selectedWord = wordButton;
        wordButton.GetComponent<Image>().color = selectedColor;

        // Intentar emparejar si ya hay una definición seleccionada
        TryPairing();
    }

    // Método que se llama cuando se selecciona una definición
    void SelectDefinition(Button definitionButton)
    {
        // Resetear la selección previa
        if (selectedDefinition != null)
        {
            selectedDefinition.GetComponent<Image>().color = Color.white;
        }

        // Seleccionar la nueva definición
        selectedDefinition = definitionButton;
        definitionButton.GetComponent<Image>().color = selectedColor;

        // Intentar emparejar si ya hay una palabra seleccionada
        TryPairing();
    }

    // Intentar emparejar una palabra con una definición
    void TryPairing()
    {
        if (selectedWord != null && selectedDefinition != null)
        {
            // Validar si el emparejamiento es correcto
            if (ValidatePair(selectedWord, selectedDefinition))
            {
                // Cambiar el color a correcto
                selectedWord.GetComponent<Image>().color = correctPairColor;
                selectedDefinition.GetComponent<Image>().color = correctPairColor;

                // Desactivar los botones para que no se puedan seleccionar nuevamente
                selectedWord.interactable = false;
                selectedDefinition.interactable = false;
            }
            else
            {
                // Cambiar el color a incorrecto
                selectedWord.GetComponent<Image>().color = incorrectPairColor;
                selectedDefinition.GetComponent<Image>().color = incorrectPairColor;

                // Resetear después de un breve retraso
                Invoke(nameof(ResetSelections), 1.5f);
            }

            // Resetear las selecciones
            selectedWord = null;
            selectedDefinition = null;
        }
    }

    // Validar el emparejamiento
    bool ValidatePair(Button word, Button definition)
    {
        // Aquí puedes usar cualquier lógica para validar los pares
        // Por ejemplo, comparar los textos en los botones
        return word.GetComponentInChildren<Text>().text == definition.GetComponentInChildren<Text>().text;
    }

    // Resetear las selecciones cuando el emparejamiento es incorrecto
    void ResetSelections()
    {
        if (selectedWord != null) selectedWord.GetComponent<Image>().color = Color.white;
        if (selectedDefinition != null) selectedDefinition.GetComponent<Image>().color = Color.white;
    }
}

