using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectedHandler childNode;
    public NodeDirectionType direction;
    [Range(100f, 350f)]
    public float lenght;
    [Range(-50f, 50f)]
    public float rotation;
}

public class UI_TreeConnectedHandler : MonoBehaviour
{
    private RectTransform myRect => GetComponent<RectTransform>();
    [SerializeField] private UI_TreeConnectDetails[] details;
    [SerializeField] private UI_TreeConnection[] connections;

    private Image connectionimage;
    private Color originalColor;

    private void Awake()
    {
        if(connectionimage != null)
            originalColor = connectionimage.color;
    }

    public UI_TreeNode[] GetChildNodes()
    {
        List<UI_TreeNode> childrenToReturn  = new List<UI_TreeNode>();

        foreach (var node in details)
        {
            if(node.childNode != null)
            {
                childrenToReturn.Add(node.childNode.GetComponent<UI_TreeNode>());
            }
        }

        return childrenToReturn.ToArray();
    }

    private void OnValidate()
    {
        if(details.Length <= 0)
            return;

        if(details.Length != connections.Length)
        {
            Debug.Log("Not the same amout of details and connections");
            return;
        }

        UpdateConnection();
    }

    public void UpdateConnection()
    {
        for (int i = 0; i < details.Length; i++)
        {
            var detail = details[i];
            var connection = connections[i];

            Vector2 targetePosition = connection.GetConnectionPoint(myRect);
            Image connectionImage = connection.GetConnectionImage();

            connection.DirectConnection(detail.direction, detail.lenght, detail.rotation);

            if (detail.childNode == null)
                continue;

            detail.childNode?.SetPosition(targetePosition);
            detail.childNode?.SetConnectionImage(connectionImage);
            detail.childNode.transform.SetAsLastSibling();
        }
    }

    public void UpdateAllConnections()
    {
        UpdateConnection();

        foreach (var node in details)
        {
            if(node.childNode ==  null) continue;   
            node.childNode.UpdateConnection();
        }
    }


    public void ConnectionImageUnlocked(bool unlocked)
    {
        if(connectionimage == null)
            return;

        connectionimage.color = unlocked ? Color.white : originalColor;
    }

    public void SetConnectionImage(Image img) => connectionimage = img;

    public void SetPosition(Vector2 position) => myRect.anchoredPosition = position;
}
