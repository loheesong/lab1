using UnityEngine;

[System.Serializable]
public struct Void { }

[CreateAssetMenu(fileName = "NewSimpleGameEvent", menuName = "ScriptableObjects/Events/SimpleGameEvent")]
public class SimpleGameEvent : GameEvent<Void>
{
    public void Raise() => Raise(new Void());
}