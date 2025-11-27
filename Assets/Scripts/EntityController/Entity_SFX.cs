using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private bool showGizmo;
    [SerializeField] private float soundDistance;
    [Header("SFX Names")]
    [SerializeField] private string attackHit;
    [SerializeField] private string attackMiss;
    [SerializeField] private string counterAttack;
    [SerializeField] private string move;
    [SerializeField] private string jump;
    [SerializeField] private string landing;
    [SerializeField] private string dash;
    [SerializeField] private string skill;
    [SerializeField] private string ulti;
    [SerializeField] private string death;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }
    public void AttackHit()
    {
        AudioManager.instance.PlaySFX(attackHit, audioSource, soundDistance);
    }

    public void AttackMiss()
    {
        AudioManager.instance.PlaySFX(attackMiss, audioSource, soundDistance);
    }
    public void CounterAttack()
    {
        AudioManager.instance.PlaySFX(counterAttack, audioSource, soundDistance);
    }
    public void Moving()
    {
        AudioManager.instance.PlaySFX(move, audioSource, soundDistance, true);
    }
    public void Jumping()
    {
        AudioManager.instance.PlaySFX(jump, audioSource, soundDistance);
    }
    public void Landing()
    {
        AudioManager.instance.PlaySFX(landing, audioSource, soundDistance);
    }
    public void Dashing()
    {
        AudioManager.instance.PlaySFX(dash, audioSource, soundDistance);
    }
    public void Skilling()
    {
        AudioManager.instance.PlaySFX(skill, audioSource, soundDistance);
    }
    public void Ultimate()
    {
        AudioManager.instance.PlaySFX(ulti, audioSource, soundDistance);
    }
    public void Dying()
    {
        AudioManager.instance.PlaySFX(death, audioSource, soundDistance);
    }
    public void StopVFX()
    {
        audioSource.Stop();
    }
    private void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, soundDistance);
        }
    }
}
