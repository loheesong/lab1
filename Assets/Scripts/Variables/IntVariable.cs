using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "ScriptableObjects/IntVariable")]
public class IntVariable : ScriptableObject {
    [SerializeField] private int value;
    [SerializeField] private int initialValue = 0;
    [SerializeField] private bool resetOnPlay = false;
    [SerializeField] private bool persistInPlayerPrefs = false;

    public int Value => value;

    public void SetValue(int val) {
        value = val;
        if (persistInPlayerPrefs) PlayerPrefs.SetInt(name, value);
    }

    public void ApplyChange(int amount) => SetValue(value + amount);

    public void ResetValue() {
        value = initialValue;
        if (persistInPlayerPrefs) PlayerPrefs.DeleteKey(name);
    }

    private void OnEnable() {
        if (persistInPlayerPrefs) value = PlayerPrefs.GetInt(name, value);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChanged;
#endif
    }

#if UNITY_EDITOR
    private void OnDisable() {
        UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeChanged;
    }

    private void OnPlayModeChanged(UnityEditor.PlayModeStateChange state) {
        if (state == UnityEditor.PlayModeStateChange.ExitingEditMode && resetOnPlay) value = initialValue;
    }
#endif
}