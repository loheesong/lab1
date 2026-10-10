using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "ScriptableObjects/IntVariable")]
public class IntVariable : ScriptableObject {
    [SerializeField] private int value;
    public int Value => value;

    public void SetValue(int val) {
        value = val;
        PlayerPrefs.SetInt(name, value);
    }

    public void ResetValue() {
        value = 0;
        PlayerPrefs.DeleteKey(name);
    }

    private void OnEnable() {
        value = PlayerPrefs.GetInt(name, value);
    }
}
