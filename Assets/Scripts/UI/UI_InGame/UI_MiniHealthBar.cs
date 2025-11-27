using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }
    private void OnEnable()
    {
        entity.Onflipped += HandleFlip;
    }
    private void OnDisable()
    {
        entity.Onflipped -= HandleFlip;
    }
    // Update is called once per frame
    private void HandleFlip() => transform.rotation = Quaternion.identity;
}
