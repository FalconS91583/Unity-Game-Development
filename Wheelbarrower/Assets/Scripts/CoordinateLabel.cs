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

    [SerializeField] TextMeshPro labels;
    private Vector2Int coortidantes = new Vector2Int();

    private WayPoint waypoint;

    void Awake()
    {
        labels = GetComponent<TextMeshPro>();
        labels.enabled = false;
        waypoint = GetComponentInParent<WayPoint>();
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
        if (waypoint.isPlaceable)
        {
            labels.color = defaultColor;
        }
        else
        {
            labels.color = blockedColor;
        }
    }

    void DisplayCoordinates()
    {
        coortidantes.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coortidantes.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

        labels.text = coortidantes.x + "," + coortidantes.y;
    }

    private void UpdateObjectName()
    {
        transform.parent.name = coortidantes.ToString();
    }
}
