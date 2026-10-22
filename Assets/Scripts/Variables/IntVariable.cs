using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "ScriptableObjects/IntVariable")]
public class IntVariable : ScriptableObject {
    [SerializeField] private int value;
    [SerializeField] private bool persistInPlayerPrefs = false;

    public int Value => value;

    public void SetValue(int val) {
        value = val;
        if (persistInPlayerPrefs) {
            PlayerPrefs.SetInt(name, value);
        }
    }

    public void ApplyChange(int amount) {
        SetValue(value + amount);
    }

    public void ResetValue() {
        value = 0;
        if (persistInPlayerPrefs) {
            PlayerPrefs.DeleteKey(name);
        }
    }

    private void OnEnable() {
        if (persistInPlayerPrefs) {
            value = PlayerPrefs.GetInt(name, value);
        }
    }
}