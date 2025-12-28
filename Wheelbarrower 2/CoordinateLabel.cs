using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabel : MonoBehaviour
{
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color blockedColor = Color.gray;
    [SerializeField] private Color explodeColor = Color.yellow;
    [SerializeField] private Color pathColor = new Color(1f,0.5f, 0f);

    [SerializeField] TextMeshPro labels;
    private Vector2Int coortidantes = new Vector2Int();

    private GridManager gridManager;
    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        labels = GetComponent<TextMeshPro>();
        labels.enabled = false;
        DisplayCoordinates();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
            labels.enabled = true;
        }

        ColorCoordinates();
        ToggleLabels();

    }

    private void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            labels.enabled = !labels.IsActive();
        }
    }

    private void ColorCoordinates()
    {
        if (gridManager == null) return;

        NodeClass node = gridManager.GetNode(coortidantes);

        if (node == null) return;
        
        if(!node.isWalkable)
        {
            labels.color = blockedColor;
        }
        else if(node.isPath)
        {
            labels.color = pathColor;
        }
        else if (node.isExplode)
        {
            labels.color = explodeColor;
        }
        else
        {
            labels.color = defaultColor;
        }
    }

    void DisplayCoordinates()
    {
        if (gridManager == null) return;

        coortidantes.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.WorldGridSize);
        coortidantes.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.WorldGridSize);

        labels.text = coortidantes.x + "," + coortidantes.y;
    }

    private void UpdateObjectName()
    {
        transform.parent.name = coortidantes.ToString();
    }
}
