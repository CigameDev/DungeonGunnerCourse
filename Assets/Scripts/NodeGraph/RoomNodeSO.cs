using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNode", menuName = "ScriptableObjects/Dugeon/Room Node ")]
public class RoomNodeSO : ScriptableObject
{
    
    public string id;
    public List<string> parentRoomNodeIDList = new List<string>();
    public List<string> childRoomNodeIDList =  new List<string> ();
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
    public RoomNodeTypeSO roomNodeType;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
    #region Editor Code
    //the following code should only be run in the Unity Editor
#if UNITY_EDITOR
    [HideInInspector] public Rect rect;
    [HideInInspector] public bool isLeftClickDragging = false;
    [HideInInspector] public bool isSelected = false;

    public void Initialise(Rect rect,RoomNodeGraphSO nodeGraph,RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.id = Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.roomNodeGraph = nodeGraph;
        this.roomNodeType = roomNodeType;

        //Load room node type list
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }    

    /// <summary>
    /// Draw node with node stype
    /// </summary>
    public void Draw(GUIStyle nodeStyle)
    {
        //Draw node box uing Begin Area
        GUILayout.BeginArea(rect,nodeStyle);

        //Start Region To Detect Popup Selection Changes
        EditorGUI.BeginChangeCheck();

        //Display a popup using the RoomNodeType name values that can be selected from (default to the currently set roomNodeType)

        int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);

        //EditorGUILayout.Popup tao ra 1 hop thoai do xuong,chua cac lua chon cho phep nguoi dung lua chon
        int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());

        roomNodeType = roomNodeTypeList.list[selection];

        if(EditorGUI.EndChangeCheck())//neu co bat ky su thay doi nao trong inspector ,hàm SetDirty se duoc goi,thong bao cho
            EditorUtility.SetDirty(this);

        GUILayout.EndArea();

    }    

    public string[]GetRoomNodeTypesToDisplay()
    {
        string[]roomArray = new string[roomNodeTypeList.list.Count];
        
        for(int i = 0; i < roomNodeTypeList.list.Count; i++)
        {
            if (roomNodeTypeList.list[i].displayInNodeGraphEditor)//chi co nhung phong duoc show ra (lon ,nho,vua ,hanh lang,boss,chest,none moi nam trong danh sach nay)
            {
                roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
            }    
        }
        return roomArray;
    }
    /// <summary>
    /// Process events for the node
    /// Cac su kien voi moi node ( nhan chuot,nha chuot ,keo )
    /// </summary>
    public void ProcessEvents(Event currentEvent)
    {
        switch(currentEvent.type)
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
            ProcessLeftClickDownEvent(currentEvent);
        }

        if(currentEvent.button == 1)//chuot phai
        {
            ProcessRightClickDownEvent(currentEvent);
        }    
    }

    private void ProcessLeftClickDownEvent(Event currentEvent)
    {
        Selection.activeObject = this;//the hien khi ta chon phong nao thi phong do se hien thi khac di la duoc chon o asset(prefab)

        //toggle node selection - chuyen doi lua chon
        if (isSelected == true)
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
    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if(currentEvent.button ==0)
        {
            ProcessLeftMouseDragEvent(currentEvent);
        }
    }

    private void ProcessLeftMouseDragEvent(Event currentEvent)
    {
        isLeftClickDragging = true;
        DragNode(currentEvent.delta);
        GUI.changed = true;
    }

    private void DragNode(Vector2 delta)//di chuyen cac phong trong editor day
    {
        rect.position += delta;
        EditorUtility.SetDirty(this);
    }    
    private void ProcessMouseUpEvent(Event currentEvent)
    {
        // If left click up
        if(currentEvent.button == 0)
        {
            ProcessLeftClickUpEvent(currentEvent);
        }

    }

    private void ProcessLeftClickUpEvent(Event currentEvent)
    {
        if(isLeftClickDragging == true)
        {
            isLeftClickDragging = false;
        }    
    }

   
#endif

    #endregion Editor Code
}
