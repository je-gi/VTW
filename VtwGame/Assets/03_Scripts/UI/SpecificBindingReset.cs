using UnityEngine;
using UnityEngine.InputSystem;

public class SpecificBindingReset : MonoBehaviour
{
    public InputActionReference actionToReset;

    public void ResetBinding()
    {
        var currentBindingPaths = GetCurrentBindingPaths(actionToReset.action);

        actionToReset.action.RemoveAllBindingOverrides();

        foreach (var action in actionToReset.action.actionMap.actions)
        {
            if (action == actionToReset.action) continue;

            var actionBindingPaths = GetCurrentBindingPaths(action);
            foreach (var path in currentBindingPaths)
            {
                if (actionBindingPaths.Contains(path))
                {
                    action.RemoveAllBindingOverrides();
                    Debug.Log($"Duplikate Bindung gefunden und zurückgesetzt: {action.name} für Pfad {path}");
                }
            }
        }
    }

    private System.Collections.Generic.List<string> GetCurrentBindingPaths(InputAction action)
    {
        var paths = new System.Collections.Generic.List<string>();
        foreach (var binding in action.bindings)
        {
            if (!string.IsNullOrEmpty(binding.effectivePath))
            {
                paths.Add(binding.effectivePath);
            }
        }
        return paths;
    }
}
