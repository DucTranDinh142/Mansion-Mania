using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer enemySprite;
    [SerializeField] private float damagedDuration = .3f;
    public float damagedTimer;
    private void Awake()
    {
        enemySprite = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        damagedTimer -= Time.deltaTime;
        if(damagedTimer <= 0 && enemySprite.color != Color.white)
            enemySprite.color = Color.white;
    }
    public void TakeDamage()
    {
        enemySprite.color = Color.black;
        Cooldown(ref damagedTimer, damagedDuration);
    }

    private void Cooldown(ref float cooldownTimeRemain, float cooldownValue)
    {
       cooldownTimeRemain = cooldownValue;
    }
}
