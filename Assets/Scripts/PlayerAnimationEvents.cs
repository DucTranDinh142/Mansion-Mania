using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Player player;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void DisableMovements() => player.EnableMovements(false);
    private void EnableMovements() => player.EnableMovements(true);

}
