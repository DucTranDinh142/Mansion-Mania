using UnityEngine;
using UnityEngine.UI;

public enum NodeDirectionType
{
    None,
    Up,
    UpLeft,
    UpRight,
    Left,
    Right,
    Down,
    DownLeft,
    DownRight,
}
public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPoint;
    [SerializeField] private RectTransform connectionLength;
    [SerializeField] private RectTransform childNodeConnectionPoint;

    public void DirectConnection(NodeDirectionType direction, float length, float offset)
    {
        bool shoulBeActive = direction != NodeDirectionType.None;
        float finalLength = shoulBeActive ? length : 0;
        float angle = GetDirectionAngle(direction);

        rotationPoint.localRotation = Quaternion.Euler(0, 0, angle + offset);
        connectionLength.sizeDelta = new Vector2(finalLength, connectionLength.sizeDelta.y);
    }
    public Image GetConnectionImage() => connectionLength.GetComponent<Image>();
    public Vector2 GetConnetionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                rect.parent as RectTransform,
                childNodeConnectionPoint.position,
                null,
                out var localPosition
            );
        return localPosition;
    }
    private float GetDirectionAngle(NodeDirectionType direction)
    {
        switch (direction)
        {
            case NodeDirectionType.UpLeft: return 135f;
            case NodeDirectionType.Up: return 90f;
            case NodeDirectionType.UpRight: return 45f;
            case NodeDirectionType.Left: return 180f;
            case NodeDirectionType.Right: return 0f;
            case NodeDirectionType.DownLeft: return -135f;
            case NodeDirectionType.Down: return -90f;
            case NodeDirectionType.DownRight: return -45f;
            default: return 0f;
        }

    }
}

