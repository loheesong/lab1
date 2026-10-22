using System;
using UnityEngine;

[System.Serializable]
public struct TransformMap
{
    public MarioState fromState;
    public PowerupType powerupCollected;
}

[CreateAssetMenu(menuName = "PluggableSM/Decisions/Transform")]
public class TransformDecision : Decision
{
    public TransformMap[] map;

    public override bool Decide(StateController controller)
    {
        if (controller is MarioStateController mario)
        {
            if (map == null || controller.currentState == null) return false;

            string stateName = controller.currentState.name.Replace("(Clone)", "").Trim();
            MarioState currentMarioState = EnumExtension.ParseEnum<MarioState>(stateName);

            for (int i = 0; i < map.Length; i++)
            {
                if ((map[i].fromState == currentMarioState || string.Equals(map[i].fromState.ToString(), stateName, StringComparison.OrdinalIgnoreCase))
                    && map[i].powerupCollected == mario.currentPowerupType)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
