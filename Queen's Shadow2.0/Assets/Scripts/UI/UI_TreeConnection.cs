using UnityEngine;
using UnityEngine.UI;

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPoint;
    [SerializeField] private RectTransform connectionLenght;

    [SerializeField] private RectTransform childNodeConnectionPoint;

    public void DirectConnection(NodeDirectionType direction, float length, float offset)
    {
        bool shouldBeActive = direction != NodeDirectionType.None;
        float finalLength = shouldBeActive ? length : 0;    
        float angle = GetDirectionAngle(direction);

        rotationPoint.localRotation = Quaternion.Euler(0,0,angle + offset);
        connectionLenght.sizeDelta = new Vector2(finalLength, connectionLenght.sizeDelta.y);
    }
    //lololofki dla mojego kochanego Kapiego, bo ja go lololofkam

    public Image GetConnectionImage() => connectionLenght.GetComponent<Image>();

    //Convert worldPosition of the object to localPosition
    public Vector2 GetConnectionPoint(RectTransform rect)
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
    private float GetDirectionAngle(NodeDirectionType type)
    {
        switch (type)
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

public enum NodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Left,
    Right,
    DownLeft,
    Down,
    DownRight,
}
