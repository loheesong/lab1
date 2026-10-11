using UnityEngine;

public enum PowerupType {
    Coin = 0,
    MagicMushroom = 1,
    OneUpMushroom = 2,
    StarMan = 3
}

public interface IPowerup {
    void SpawnPowerup();
    void ApplyPowerup();
    void DestroyPowerup();
}
