using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Player player;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    public void DamageEnemies() => player.DamageEnemies();
    private void DisableMovements() => player.EnableMovements(false);
    private void EnableMovements() => player.EnableMovements(true);

}
