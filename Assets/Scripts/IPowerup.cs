using UnityEngine;

public enum PowerupType {
    Coin = 0,
    MagicMushroom = 1,
    OneUpMushroom = 2,
    StarMan = 3,
    FireFlower = 4,
    Damage = 99,
    Default = -1
}

public interface IPowerup {
    void SpawnPowerup();
    void ApplyPowerup(GameObject target);
    void DestroyPowerup();
}