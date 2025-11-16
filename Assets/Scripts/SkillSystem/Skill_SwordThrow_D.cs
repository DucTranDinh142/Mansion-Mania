using UnityEngine;

public class Skill_SwordThrow_D : Skill_Base
{
    private SkillObject_Sword currentSword;
    private float currentThrowPower;

    [Header("Sword Upgrade")]
    [SerializeField] private GameObject swordPrefab;
    [Range(0f, 10f)]
    [SerializeField] private float swordThrowPower = 3;

    [Header("Sprear Upgrade")]
    [SerializeField] private GameObject spearPrefab;
    public int amountToPierce = 2;
    [Range(0f, 10f)]
    [SerializeField] private float spearThrowPower = 5;

    [Header("Saw Upgrade")]
    [SerializeField] private GameObject sawPrefab;
    public int maxDistance = 5;
    public float attackPerSecond = 1;
    public float maxSpinDuration = 3;
    [Range(0f, 10f)]
    [SerializeField] private float sawThrowPower = 2.5f;

    [Header("Grenade Upgrade")]
    [SerializeField] private GameObject grenadePrefab;
    public int bounceCount;
    public float bounceSpeed = 16;
    [Range(0f, 10f)]
    [SerializeField] private float grenadeThrowPower = 2.5f;

    [Header("Trajectory prediction")]
    [SerializeField] private GameObject predictionDot;
    [SerializeField] private int numberOfDots = 21;
    [SerializeField] private float spaceBetweenDots = .09f;
    [SerializeField] private float swordGravity = 3.5f;
    private Transform[] dots;
    private Vector2 confirmedDirection;

    private void UpdateThrowPower()
    {
        switch (upgradeType)
        {
            case SkillUpgradeType.SwordThrow_D:
                currentThrowPower = swordThrowPower;
                break;

            case SkillUpgradeType.SwordThrow_Pierces_D1:
                currentThrowPower = spearThrowPower;
                break;

            case SkillUpgradeType.SwordThrow_Spin_D2:
                currentThrowPower = sawThrowPower;
                break;

            case SkillUpgradeType.SwordThrow_Bounce_D3:
                currentThrowPower = grenadeThrowPower;
                break;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        swordGravity = swordPrefab.GetComponent<Rigidbody2D>().gravityScale;
        dots = GenerateDots();
    }

    public override bool CanUseSkill()
    {
        UpdateThrowPower();

        if (currentSword != null)
        {
            currentSword.GetSwordBackToPlayer();
            SetSkillOnCoolDown();
            return false;
        }

        return base.CanUseSkill();
    }
    public void ThrowSword()
    {
        GameObject choicePrefab = GetSwordPrefab();
        GameObject newSword = Instantiate(choicePrefab, dots[1].position, Quaternion.identity);

        currentSword = newSword.GetComponent<SkillObject_Sword>();
        currentSword.SetUpSword(this, GetThrowPower());
    }
    private GameObject GetSwordPrefab()
    {
        if (Unlock(SkillUpgradeType.SwordThrow_D))
            return swordPrefab;
        if (Unlock(SkillUpgradeType.SwordThrow_Pierces_D1))
            return spearPrefab;
        if (Unlock(SkillUpgradeType.SwordThrow_Spin_D2))
            return sawPrefab;
        if (Unlock(SkillUpgradeType.SwordThrow_Bounce_D3))
            return grenadePrefab;
        return swordPrefab;

    }
    private Vector2 GetThrowPower() => confirmedDirection * (currentThrowPower * 10);
    public void PredictTrajectory(Vector2 direction)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].position = GetTrajectoryPoint(direction, i * spaceBetweenDots);
        }
    }
    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        //s = v0*t + 1/2(g*t^2)
        float scaledThrowPower = swordThrowPower * 10;
        Vector2 initialVelocity = direction * scaledThrowPower; // v0

        Vector2 gravityEffect = .5f * Physics2D.gravity * swordGravity * (t * t); // gravity = 1/2(g*t^2)
        Vector2 predictedPoint = (initialVelocity * t) + gravityEffect;

        Vector2 playerPosition = transform.root.position;

        return playerPosition + predictedPoint;
    }
    public void ConfirmTrajectory(Vector2 direction) => confirmedDirection = direction;
    public void EnableDots(bool enable)
    {
        foreach (Transform t in dots)
            t.gameObject.SetActive(enable);
    }
    private Transform[] GenerateDots()
    {
        Transform[] newDots = new Transform[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            newDots[i] = Instantiate(predictionDot, transform.position, Quaternion.identity, transform).transform;
            newDots[i].gameObject.SetActive(false);
        }

        return newDots;
    }

}
