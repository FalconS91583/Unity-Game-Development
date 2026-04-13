using System.Collections;
using UnityEngine;

public class Boss_TeleportDasher : Enemy_Boss
{
    [Header("Teleport Settings")]
    [SerializeField] private float fadeOutDuration = 0.5f; 
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float stayInvisibleDuration = 0.2f; 

    [Header("Dash Settings")]
    [SerializeField] private float windUpTime = 0.4f; 
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float recoveryTime = 1f; 

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    protected override void Start()
    {
        base.Start(); 

        
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }
    protected override bool Move()
    {
        if (player == null) return false;

        if (base.Move())
        {
            isDashing = false; 
            return true;
        }


        if (isDashing) return false;

        if (!isBusy)
        {
            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, dir * moveSpeed, acceleration * Time.fixedDeltaTime);
            RotateTowards(dir);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        return false;
    }
    private bool isDashing = false;

    protected override void UseSpeciality()
    {
        if (!isBusy)
        {
            StartCoroutine(TeleportDashSequence());
        }
    }

    private IEnumerator TeleportDashSequence()
    {
        isBusy = true;
        isDashing = false;

        rb.linearVelocity = Vector2.zero;
        col.enabled = false;

        yield return StartCoroutine(FadeAlphaTo(0f, fadeOutDuration));
        yield return new WaitForSeconds(stayInvisibleDuration);

        transform.position = new Vector2(
            Random.Range(minBounds.x, maxBounds.x),
            Random.Range(minBounds.y, maxBounds.y)
        );

        yield return StartCoroutine(FadeAlphaTo(1f, fadeInDuration));
        col.enabled = true;

        float elapsed = 0f;
        while (elapsed < windUpTime)
        {
            if (player != null)
            {
                Vector2 dirToPlayer = (player.position - transform.position).normalized;
                RotateTowards(dirToPlayer);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }


        if (player != null && !IsPlayerCamouflaged())
        {
            isDashing = true;

            Vector2 dashDir = (player.position - transform.position).normalized;

            float dashTimer = 0f;
            while (dashTimer < dashDuration)
            {
                
                rb.linearVelocity = dashDir * dashSpeed;
                dashTimer += Time.deltaTime;
                yield return null;
            }
            isDashing = false;
        }

        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(recoveryTime);
        isBusy = false;
    }
    private IEnumerator FadeAlphaTo(float targetAlpha, float duration)
    {
        if (spriteRenderer == null) yield break;

        float startAlpha = spriteRenderer.color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);

            Color newColor = originalColor;
            newColor.a = newAlpha;
            spriteRenderer.color = newColor;

            yield return null;
        }
        Color finalColor = originalColor;
        finalColor.a = targetAlpha;
        spriteRenderer.color = finalColor;
    }

    private void RotateTowards(Vector2 direction)
    {
        if (direction == Vector2.zero) return;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
    }
}