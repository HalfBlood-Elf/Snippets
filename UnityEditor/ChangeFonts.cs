using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class ChangeFonts : EditorWindow
{

    static List<Text> allTexts = new List<Text>();
    static List<Text> targetTexts = new List<Text>();

    static List<UnityEngine.Object> offenders = new List<UnityEngine.Object>();
    static Font targetFindFont;
    static Font targetChangeFont;
    [MenuItem("Tools/ChangeFonts")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ChangeFonts window = (ChangeFonts)EditorWindow.GetWindow(typeof(ChangeFonts));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Search for:");
        targetFindFont = (Font)EditorGUILayout.ObjectField(targetFindFont, typeof(Font),true);
        GUILayout.Label("Change to:");
        targetChangeFont = (Font)EditorGUILayout.ObjectField(targetChangeFont, typeof(Font), true);
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Search"))
        {
            SelectTargets();
        }
        if (GUILayout.Button("Change"))
        {
            if (targetTexts.Count > 0)
            {
                Change();
            }
            else
            {
                Debug.LogError("Nothing to change, search for target Texts first");
            }
        }
        GUILayout.EndHorizontal();
    }
    void Change()
    {
        foreach (var t in targetTexts)
        {
            t.font = null;
            t.font = targetChangeFont;
            EditorUtility.SetDirty(t);
        }
        Debug.Log("Changed " + targetTexts.Count.ToString() + " Fonts");
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
    void SelectTargets()
    {
        allTexts = FindObjectsOfTypeAll<Text>();
        offenders.Clear();
        targetTexts.Clear();
        foreach (var item in allTexts)
        {
            //Debug.Log("checking: " + item.name);
            if (checkObject(item))
            {
                offenders.Add(item.gameObject as UnityEngine.Object);
                targetTexts.Add(item);
            }
            //else 
        }
        Selection.objects = offenders.ToArray();
        Debug.Log("Found " + offenders.Count.ToString() + " objects with target Font");
    }
    static bool checkObject(Text text)
    {
        if (text.font == targetFindFont)
        {
            return true;
        }
        else return false;
    }
    public static List<T> FindObjectsOfTypeAll<T>()
    {
        List<T> results = new List<T>();
        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
        {
            var s = EditorSceneManager.GetSceneAt(i);
            if (s.isLoaded)
            {
                var allGameObjects = s.GetRootGameObjects();
                for (int j = 0; j < allGameObjects.Length; j++)
                {
                    var go = allGameObjects[j];
                    results.AddRange(go.GetComponentsInChildren<T>(true));
                }
            }
        }
        return results;
    }
}