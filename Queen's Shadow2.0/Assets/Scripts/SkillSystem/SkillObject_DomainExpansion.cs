using UnityEngine;

public class SkillObject_DomainExpansion : SkillObject_Base
{
    private Skill_DomainExpansion domainManager;

    private float expandSpeed = 2;
    private float duration;
    private float slowDownPrecent = 0.9f;

    private Vector3 targetScale;
    private bool isShrinking;

    public void SetupDomain(Skill_DomainExpansion domainManager)
    {
        this.domainManager = domainManager;

        float maxSize = domainManager.maxDomainSize;
        duration = domainManager.GetDomainDuration();
        slowDownPrecent =  domainManager.GetSlowPrecent();
        expandSpeed = domainManager.expandSpeed;

        targetScale = Vector3.one * maxSize;

        Invoke(nameof(ShrinkDomain), duration);
    }

    private void Update()
    {
        HandleScaling();
    }

    private void HandleScaling()
    {
        float sizeDiffrence = Mathf.Abs(transform.localScale.x - targetScale.x);
        bool shouldChangeScale = sizeDiffrence > 0.1f;

        if (shouldChangeScale) 
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, expandSpeed * Time.deltaTime);

        if (isShrinking && sizeDiffrence < 0.1f)
        {
            TerminateDomain();
        }
    }

    private void TerminateDomain()
    {
        domainManager.ClearTargets();
        Destroy(gameObject);
    }

    private void ShrinkDomain()
    {
        targetScale = Vector3.zero;
        isShrinking = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null)
            return;

        domainManager.AddTarget(enemy);
        enemy.SlowDownEntityBy(duration, slowDownPrecent, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null)
            return;

        enemy.StopSlowDown();
    }
}
