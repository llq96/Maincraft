using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


// =====
// ПРОСТО НАЖМИТЕ (CTRL + W)  В ЮНИТИ И ВЫ ВСЁ ПОЙМЁТЕ;)
// =====
public class BestSceneLoader {
    public static GenericMenu menu;

    //Help https://docs.unity3d.com/ScriptReference/MenuItem.html
    [MenuItem("GameObject/Do Something with a Shortcut Key %w")]
    public static void GenerateGenericMenu() {
        //Созадние меню
        menu = new GenericMenu();
        menu.allowDuplicateNames = false;

        //Заполнение меню
        AddScenes_FromBuildSettings();

        menu.AddSeparator(""); 
        menu.AddSeparator("");
        menu.AddSeparator("");

        AddScenes_Other();

        //Показ меню в месте курсора мыши
        ShowGenericMenuAtCursorPosition();
    }

    #region Addiing Items
    static void AddScenes_FromBuildSettings() {
        EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
        string[] paths = new string[buildScenes.Length];
        for (int i = 0; i < buildScenes.Length; i++) {
            paths[i] = buildScenes[i].path;
        }
        AddMenuItems(paths);
    }

    static void AddScenes_Other() {
        string[] guids = AssetDatabase.FindAssets("t:Scene");
        string[] paths = new string[guids.Length];
        for (int i = 0; i < guids.Length; i++) {
            paths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
        }
        AddMenuItems(paths);
    }


    static void AddMenuItems(string[] paths) {
        string path;
        for (int i = 0; i < paths.Length; i++) {
            path = paths[i];
            AddMenuItem($"{path.Substring(path.LastIndexOf("/") + 1)}", path);
        }
    }


    static void AddMenuItem(string _menuName, string _scenePath) {
        menu.AddItem(new GUIContent(_menuName), false, OnClicked, _scenePath);
    }

    #endregion

    static void ShowGenericMenuAtCursorPosition() {
        Vector2 pos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        EditorWindow focused = EditorWindow.focusedWindow;
        pos.x += focused.position.x;
        pos.y += focused.position.y;

        menu.DropDown(new Rect(pos.x, pos.y, 0f, 0f));
    }

    static void OnClicked(object _scenePath) {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene((string)_scenePath, OpenSceneMode.Single);
    }

}