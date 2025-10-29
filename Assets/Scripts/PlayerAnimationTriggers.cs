using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void CurrentStateTrigger()
    {
        player.CallAnimationTrigger();
    }
}
