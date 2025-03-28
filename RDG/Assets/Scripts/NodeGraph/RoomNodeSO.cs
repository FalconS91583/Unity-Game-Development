using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEditor;

public class RoomNodeSO : ScriptableObject
{
    #region Odcinek 9
    public string id;
    public List<string> parentRoomNodeIDList = new List<string>();
    public List<string> childRoomNodeIDList = new List<string>();
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
    public RoomNodeTypeSO roomNodeType;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;

#if UNITY_EDITOR
    [HideInInspector] public Rect rect;
    [HideInInspector] public bool isLeftClickDragging = false;
    [HideInInspector] public bool isSelected = false;

    public void Initialise(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.id = Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.roomNodeGraph = nodeGraph;
        this.roomNodeType = roomNodeType;

        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    public void Draw(GUIStyle nodeStyle)
    {
        GUILayout.BeginArea(rect, nodeStyle);

        EditorGUI.BeginChangeCheck();

        if (parentRoomNodeIDList.Count > 0 || roomNodeType.isEntrance)
        {
            EditorGUILayout.LabelField(roomNodeType.roomNodeTypeName);
        }
        else
        {
            int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);

            int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());

            roomNodeType = roomNodeTypeList.list[selection];

            if (roomNodeTypeList.list[selected].isCorridor && !roomNodeTypeList.list[selection].isCorridor
                || !roomNodeTypeList.list[selected].isCorridor && roomNodeTypeList.list[selection].isCorridor 
                || !roomNodeTypeList.list[selected].isBossRoom && roomNodeTypeList.list[selection].isBossRoom)
            {
                if (childRoomNodeIDList.Count > 0)
                {
                    for (int i = childRoomNodeIDList.Count - 1; i >= 0; i--)
                    {
                        RoomNodeSO childRoomNode = roomNodeGraph.GetRoomNode(childRoomNodeIDList[i]);

                        if (childRoomNode != null)
                        {
                            RemoveChildRoomNodeIDFromNode(childRoomNode.id);

                            childRoomNode.RemoveParentRoomNodeIDFromNode(id);
                        }
                    }
                }
            }
        }

        if(EditorGUI.EndChangeCheck()) 
            EditorUtility.SetDirty(this);

        GUILayout.EndArea();
    }
    public string[] GetRoomNodeTypesToDisplay()
    {
        string[] roomArray = new string[roomNodeTypeList.list.Count];

        for (int i = 0; i < roomNodeTypeList.list.Count; i++)
        {
            if (roomNodeTypeList.list[i].displayInNodeGraphEditor)
            {
                roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
            }
        }
        return roomArray;
    }

    public void ProcessEvent(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent); 
                break;

            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;

            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;

            default:
                break;
        }
    }

    private void ProcessMouseDownEvent(Event currentEvent)
    {
        if (currentEvent.button == 0)
        {
            ProcessLeftClickDownEvent();
        }

        else if(currentEvent.button == 1)
        {
            ProcessRightClickDownEvent(currentEvent);
        }
    }

    private void ProcessLeftClickDownEvent()
    {
        Selection.activeObject = this;

        if(isSelected == true)
        {
            isSelected = false;
        }
        else
        {
            isSelected = true;
        }
    }
    private void ProcessRightClickDownEvent(Event currentEvent)
    {
        roomNodeGraph.SetNodeToDrawConnectionLineFrom(this, currentEvent.mousePosition);
    }
    private void ProcessMouseUpEvent(Event currentEvent) 
    {
        if(currentEvent.button == 0)
        {
            ProcessLeftClickUpEvent();
        }
    }
    private void ProcessLeftClickUpEvent()
    {
        if (isLeftClickDragging)
        {
            isLeftClickDragging = false;
        }
    }
    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if(currentEvent.button == 0)
        {
            ProcessLeftMouseDragEvent(currentEvent);
        }
    }
    private void ProcessLeftMouseDragEvent(Event currEvent)
    {
        isLeftClickDragging = true;

        DragNode(currEvent.delta);
        GUI.changed = true;
    }
    public void DragNode(Vector2 delta)
    {
        rect.position += delta;
        EditorUtility.SetDirty(this);
    }
    public bool AddCheildRoomNodeIDToRoomNode(string childID)
    {
        if (IsChildRoomValid(childID))
        {
            childRoomNodeIDList.Add(childID);
            return true;
        }
        return false;
    }
    public bool IsChildRoomValid(string childID)
    {
        bool isConnectedBossNodeArleady = false;

        foreach (RoomNodeSO roomNode in roomNodeGraph.roomNodeList)
        {
            if(roomNode.roomNodeType.isBossRoom && roomNode.parentRoomNodeIDList.Count > 0 )
                isConnectedBossNodeArleady = true;
        }
        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isBossRoom && isConnectedBossNodeArleady)
            return false;

        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isNone)
            return false;

        if(childRoomNodeIDList.Contains(childID))
            return false;

        if(id == childID)
            return false;

        if(parentRoomNodeIDList.Contains(childID))
            return false;

        if(roomNodeGraph.GetRoomNode(childID).parentRoomNodeIDList.Count > 0)
            return false;

        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && roomNodeType.isCorridor)
            return false;

        if(!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && !roomNodeType.isCorridor)
            return false;

        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor  && childRoomNodeIDList.Count >= Settings.maxChildCorridors)
            return false;

        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isEntrance)
            return false;

        if(!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count >0)
            return false;

        return true;
    }
    public bool AddPArentRoomNodeIDToRoomNode(string parentID)
    {
        parentRoomNodeIDList.Add(parentID); 
        return true;
    }

    public bool RemoveChildRoomNodeIDFromNode(string childID)
    {
        if (childRoomNodeIDList.Contains(childID))
        {
            childRoomNodeIDList.Remove(childID);
            return true;
        }
        return false;
    }
    public bool RemoveParentRoomNodeIDFromNode(string parentID)
    {
        if (parentRoomNodeIDList.Contains(parentID))
        {
            parentRoomNodeIDList.Remove(parentID);
            return true;
        }
        return false;
    }
#endif
    #endregion
}
