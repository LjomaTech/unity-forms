using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor menu for quickly adding LjomaTech Form Management prefabs to the scene.
/// Access via GameObject > LTFormManagement > Prefabs
/// </summary>
public static class LTFormPrefabMenu
{
    private const string ROOT_MENU = "GameObject/LTFormManagement/Prefabs/";
    private const string PREFAB_PATH = "LTPrefabs";

    // ============================================================
    // Basic Inputs
    // ============================================================
    
    [MenuItem(ROOT_MENU + "Input Field", false, 10)]
    private static void AddInputField()
    {
        SpawnPrefab("InputField-FormManagement");
    }

    [MenuItem(ROOT_MENU + "Button", false, 11)]
    private static void AddButton()
    {
        SpawnPrefab("Button-FormManagement");
    }

    [MenuItem(ROOT_MENU + "Toggle", false, 12)]
    private static void AddToggle()
    {
        SpawnPrefab("Toggle-FormManagement");
    }

    [MenuItem(ROOT_MENU + "Dropdown", false, 13)]
    private static void AddDropdown()
    {
        SpawnPrefab("DropDown-FormManagement");
    }

    // ============================================================
    // Sliders
    // ============================================================
    
    [MenuItem(ROOT_MENU + "Sliders/Slider", false, 20)]
    private static void AddSlider()
    {
        SpawnPrefab("Slider-FormManagement");
    }

    [MenuItem(ROOT_MENU + "Sliders/Range (Double Slider)", false, 21)]
    private static void AddRangeSlider()
    {
        SpawnPrefab("RangeInput-FormManagement");
    }

    // ============================================================
    // Date & Time
    // ============================================================
    
    [MenuItem(ROOT_MENU + "Date & Time/Date Input", false, 30)]
    private static void AddDateInput()
    {
        SpawnPrefab("DateInput/DateInput-FormManagement");
    }

    [MenuItem(ROOT_MENU + "Date & Time/DateTime (12hr)", false, 31)]
    private static void AddDateTimeInput()
    {
        SpawnPrefab("DateInput/DatetimeInput-FormManagement");
    }

    [MenuItem(ROOT_MENU + "Date & Time/DateTime (24hr)", false, 32)]
    private static void AddDateTime24Input()
    {
        SpawnPrefab("DateInput/DatetimeInput-24-FormManagement");
    }

    // ============================================================
    // Selection
    // ============================================================
    
    [MenuItem(ROOT_MENU + "Selection/Radiobox (Single Select)", false, 40)]
    private static void AddRadiobox()
    {
        SpawnPrefab("Radiobox/Radiobox");
    }

    [MenuItem(ROOT_MENU + "Selection/Radiobox (Multi Select)", false, 41)]
    private static void AddRadioboxMulti()
    {
        SpawnPrefab("Radiobox/Radiobox-MultiSelect");
    }

    [MenuItem(ROOT_MENU + "Selection/Radiobox Toggle", false, 42)]
    private static void AddRadioboxToggle()
    {
        SpawnPrefab("Radiobox/Radiobox-Toggle");
    }

    // ============================================================
    // Specialized Inputs
    // ============================================================
    
    [MenuItem(ROOT_MENU + "Specialized/Phone Number", false, 50)]
    private static void AddPhoneNumber()
    {
        SpawnPrefab("PhoneNumber-FormManagement");
    }

    [MenuItem(ROOT_MENU + "Specialized/Color Picker", false, 51)]
    private static void AddColorPicker()
    {
        SpawnPrefab("ColorPicker-FormManagement");
    }

    // ============================================================
    // Helper
    // ============================================================
    
    private static void SpawnPrefab(string prefabName)
    {
        GameObject prefab = Resources.Load<GameObject>($"{PREFAB_PATH}/{prefabName}");

        if (prefab == null)
        {
            Debug.LogError($"[LTFormManagement] Prefab not found: Resources/{PREFAB_PATH}/{prefabName}");
            return;
        }

        // Get parent - use selection if it's a Canvas or has a Canvas parent
        Transform parent = Selection.activeTransform;
        
        // If nothing selected, try to find a Canvas in the scene
        if (parent == null)
        {
            Canvas canvas = Object.FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                parent = canvas.transform;
            }
        }

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
        
        if (instance != null)
        {
            // Position at center if RectTransform
            RectTransform rectTransform = instance.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = Vector2.zero;
            }

            Undo.RegisterCreatedObjectUndo(instance, $"Create {prefabName}");
            Selection.activeGameObject = instance;
            
            // Focus on the new object in hierarchy
            EditorGUIUtility.PingObject(instance);
        }
    }
}