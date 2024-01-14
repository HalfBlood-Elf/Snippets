using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OpenPersistentDataPath
{
    [MenuItem("Tools/Open Persistent Data Path Folder")]
    public static void OpenPersistentDataPathFolder()
    {
        Application.OpenURL(Application.persistentDataPath);
    }
}
