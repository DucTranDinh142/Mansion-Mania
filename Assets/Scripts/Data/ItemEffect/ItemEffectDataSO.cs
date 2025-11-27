using UnityEngine;

public class ItemEffectDataSO : ScriptableObject
{
    [TextArea]
    public string effectDescription;
    protected Player player;

    public virtual bool CanBeUsed(Player player)
    {
        return true;
    }
    public virtual void ExecuteEffect() 
    {

    }
    public virtual void Subcribe(Player player)
    {
        this.player = player;
    }
    public virtual void Unsubcribe()
    {

    }
}
