using UnityEngine;

public class SkillObject_Ultimate : SkillObject_Base
{
    private Skill_Ultimate_E ultimateManager;

    private float expandSpeed;
    private float duration;
    private float slowDownPercent;

    private Vector3 targetScale;
    private bool isStrinking;
    public void SetupDomain(Skill_Ultimate_E ultimateManager)
    {
        this.ultimateManager = ultimateManager;

        duration = ultimateManager.GetDomainDuration();
        float maxSize = ultimateManager.maxDomainSize;
        slowDownPercent = ultimateManager.GetSlowPercent();
        expandSpeed = ultimateManager.expandSpeed;

        targetScale = Vector3.one*maxSize;
        Invoke(nameof(StrinkDomain), duration);
    }
    private void Update()
    {
        HandleScaling();
    }
    private void HandleScaling()
    {
        float sizeDifference = Mathf.Abs(transform.localScale.x);
        bool shouldChangeScale = sizeDifference > .1f;

        if (shouldChangeScale)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, expandSpeed*Time.deltaTime);
        if(isStrinking && sizeDifference < .1f)
        {
            ultimateManager.ClearTargets();
            Destroy(gameObject);  
        }
    }
    private void StrinkDomain()
    {
        targetScale = Vector3.zero;
        isStrinking = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null) return;

        ultimateManager.AddTarget(enemy);
        enemy.SlowDownEntity(duration, slowDownPercent, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null) return;

        enemy.StopSlowDown();
    }
}