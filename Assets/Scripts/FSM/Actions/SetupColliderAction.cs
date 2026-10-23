using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/SetupCollider")]
public class SetupColliderAction : Action {
    public Vector2 size = new Vector2(0.82589954f, 1.0215895f);
    public Vector2 offset = new Vector2(0f, 0.00828445f);

    public override void Act(StateController controller) {
        BoxCollider2D col = controller.GetComponent<BoxCollider2D>();
        if (col == null) {
            Debug.LogError($"[SetupColliderAction] Could not find a BoxCollider2D on {controller.gameObject.name}.", controller);
            return;
        }

        // Sprites are centre-pivoted, so a taller collider grows downward; lift Mario to keep his feet on the same spot
        float oldBottom = col.offset.y - col.size.y / 2f;
        float newBottom = offset.y - size.y / 2f;
        controller.transform.position += Vector3.up * (oldBottom - newBottom) * controller.transform.lossyScale.y;

        col.size = size;
        col.offset = offset;
    }
}
