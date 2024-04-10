using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AllBindingsReset : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    public void ResetAllBindings()
    {
        // Zugriff auf die aktuelle Action-Map des Spielers über die PlayerInput-Komponente
        InputActionMap playerActionMap = _playerInput.currentActionMap;

        // Durchlaufen Sie alle Aktionen in der Player-Action-Map
        foreach (InputAction action in playerActionMap)
        {
            // Entfernen Sie alle Bindungsüberschreibungen für jede Aktion
            action.RemoveAllBindingOverrides();
        }
    }

    public void AssignResetFunctionToButton(Button button)
    {
        // Weisen Sie die ResetAllBindings-Methode dem Button-Click-Event zu
        button.onClick.AddListener(ResetAllBindings);
    }
}
