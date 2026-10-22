using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/FireAttack")]
public class FireAttackAction : Action {
    [Header("Fireball Settings")]
    [Tooltip("Maximum number of active fireballs allowed on screen at once.")]
    public int maxPrefabInScene = 3;

    [Tooltip("Initial impulse force applied to the fireball.")]
    public float impulseForce = 8f;

    [Tooltip("Angle in degrees upwards at which the fireball is launched.")]
    public float degree = 30f;

    [Tooltip("The Fireball prefab to instantiate.")]
    public GameObject attackPrefab;

    [Header("Facing Direction Reference")]
    [Tooltip("BoolVariable updated by PlayerMovement tracking Mario's horizontal facing.")]
    public BoolVariable marioFaceRight;

    public override void Act(StateController controller) {
        if (attackPrefab == null) {
            Debug.LogWarning("[FireAttackAction] Attack Prefab is not assigned!", this);
            return;
        }

        // Limit the number of active fireballs in the scene
        string targetTag = string.IsNullOrEmpty(attackPrefab.tag) || attackPrefab.CompareTag("Untagged")
            ? "Untagged"
            : attackPrefab.tag;

        GameObject[] activePrefabs = targetTag != "Untagged"
            ? GameObject.FindGameObjectsWithTag(targetTag)
            : Object.FindObjectsByType<FireballController>(FindObjectsSortMode.None) is FireballController[] fbs
                ? System.Array.ConvertAll(fbs, fb => fb.gameObject)
                : new GameObject[0];

        if (activePrefabs.Length < maxPrefabInScene) {
            bool isFacingRight = marioFaceRight == null || marioFaceRight.Value;

            // Spawn slightly in front of Mario so it does not collide immediately with Mario's collider
            Vector3 spawnOffset = new Vector3(isFacingRight ? 0.6f : -0.6f, 0.2f, 0f);
            Vector3 spawnPosition = controller.transform.position + spawnOffset;

            GameObject fireball = Instantiate(attackPrefab, spawnPosition, Quaternion.identity);

            // Apply impulse force in the facing direction
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null) {
                Vector2 direction = CalculateDirection(degree, isFacingRight);
                rb.AddForce(direction * impulseForce, ForceMode2D.Impulse);
            }
        }
    }

    public Vector2 CalculateDirection(float degrees, bool isFacingRight) {
        float radians = degrees * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);

        if (!isFacingRight) {
            x = -x;
        }

        return new Vector2(x, y).normalized;
    }
}