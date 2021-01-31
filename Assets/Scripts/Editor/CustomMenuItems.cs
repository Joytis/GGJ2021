using System.Linq;
using UnityEditor;
using UnityEngine;

public static class CustomMenuItems
{
        // Add a menu item named "Do Something with a Shortcut Key" to MyMenu in the menu bar
    // and give it a shortcut (ctrl-g on Windows, cmd-g on macOS).
    [MenuItem("MyMenu/Copy Components I Care About %&d")]
    static void DoSomethingWithAShortcutKey()
    {
        var gameObjects = Selection.objects.Where(x => x is GameObject).Select(x => (GameObject)x).ToArray();
        if(gameObjects.Length != 2) return;

        UnityEditorInternal.ComponentUtility.CopyComponent(gameObjects[0].GetComponent<Rigidbody>());
        UnityEditorInternal.ComponentUtility.PasteComponentAsNew(gameObjects[1]);

        UnityEditorInternal.ComponentUtility.CopyComponent(gameObjects[0].GetComponent<ConfigurableJoint>());
        UnityEditorInternal.ComponentUtility.PasteComponentAsNew(gameObjects[1]);
    }
}