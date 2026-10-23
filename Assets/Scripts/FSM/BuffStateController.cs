using System.Collections;
using UnityEngine;

public class BuffStateController : StateController {
    [Header("Active Buff Data")]
    public PowerupType currentBuffType = PowerupType.Default;
    public bool isInvincible = false;

    [Header("Starman Flash")]
    [SerializeField] private Material flashMaterial;
    public float flashInterval = 0.1f;

    [Header("Broadcast Audio Events")]
    [SerializeField] private SimpleGameEvent onStarmanStart;
    [SerializeField] private SimpleGameEvent onStarmanExit;

    private SpriteRenderer spriteRenderer;
    private Material defaultMaterial;
    private Animator animator;
    private RuntimeAnimatorController preStarmanAnimator;
    private RuntimeAnimatorController activeStarmanAnimator;

    public override void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        defaultMaterial = spriteRenderer.sharedMaterial;
        base.Start();
        GameRestart();
    }

    public void GameRestart() {
        currentBuffType = PowerupType.Default;
        isInvincible = false;
        StopFlash();
        RestoreAnimator();
        onStarmanExit?.Raise();
        TransitionToState(startState);
    }

    public void SetBuff(PowerupType buff) {
        currentBuffType = buff;
    }

    public void StartStarman(RuntimeAnimatorController starmanAnimator) {
        isInvincible = true;
        SwapAnimator(starmanAnimator);
        onStarmanStart?.Raise();
        StopCoroutine(nameof(FlashWhite));
        StartCoroutine(nameof(FlashWhite));
    }

    public void ExitStarman() {
        isInvincible = false;
        StopFlash();
        RestoreAnimator();
        onStarmanExit?.Raise();
    }

    private void SwapAnimator(RuntimeAnimatorController starmanAnimator) {
        if (animator == null || starmanAnimator == null) return;
        if (activeStarmanAnimator == null) preStarmanAnimator = animator.runtimeAnimatorController;
        activeStarmanAnimator = starmanAnimator;
        animator.runtimeAnimatorController = starmanAnimator;
    }

    // Skip restore if a form change (powerup, damage, death) replaced the controller mid-Starman
    private void RestoreAnimator() {
        if (animator != null && activeStarmanAnimator != null && animator.runtimeAnimatorController == activeStarmanAnimator) {
            animator.runtimeAnimatorController = preStarmanAnimator;
        }
        activeStarmanAnimator = null;
        preStarmanAnimator = null;
    }

    private void StopFlash() {
        StopCoroutine(nameof(FlashWhite));
        if (spriteRenderer != null) spriteRenderer.sharedMaterial = defaultMaterial;
    }

    private IEnumerator FlashWhite() {
        if (flashMaterial == null) yield break;

        while (isInvincible) {
            spriteRenderer.sharedMaterial = spriteRenderer.sharedMaterial == flashMaterial ? defaultMaterial : flashMaterial;
            yield return new WaitForSeconds(flashInterval);
        }

        spriteRenderer.sharedMaterial = defaultMaterial;
    }
}
