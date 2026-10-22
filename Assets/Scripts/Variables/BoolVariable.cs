using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariable", menuName = "ScriptableObjects/Variables/BoolVariable")]
public class BoolVariable : ScriptableObject {
    [SerializeField] private bool value;
    public bool Value => value;

    public void SetValue(bool val) {
        value = val;
    }

    public void Toggle() {
        value = !value;
    }
}