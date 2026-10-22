using System;
using System.Collections;
using UnityEngine;

public class MarioStateController : StateController {
    [Header("Mario State Data")]
    public PowerupType currentPowerupType = PowerupType.Default;
    public float flickerInterval = 0.1f;

    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;

    public override void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        base.Start();
        GameRestart();
    }

    // Called on start or when OnGameRestart event is raised
    public void GameRestart() {
        currentPowerupType = PowerupType.Default;
        StopAllCoroutines();
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        TransitionToState(startState);
    }

    public void SetPowerup(PowerupType type) {
        currentPowerupType = type;
    }

    public void Fire() {
        if (currentState != null) {
            currentState.DoEventTriggeredActions(this, ActionType.Attack);
        }
    }

    public void SetRendererToFlicker() {
        StopCoroutine(nameof(BlinkSpriteRenderer));
        StartCoroutine(nameof(BlinkSpriteRenderer));
    }

    private IEnumerator BlinkSpriteRenderer() {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        while (currentState != null && string.Equals(currentState.name, "InvincibleSmallMario", StringComparison.OrdinalIgnoreCase)) {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flickerInterval);
        }

        spriteRenderer.enabled = true;
    }
}