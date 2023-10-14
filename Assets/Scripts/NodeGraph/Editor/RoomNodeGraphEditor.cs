using System;
using UnityEditor;
using UnityEditor.Callbacks;// use open asset
using UnityEngine;



public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeStype;
    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeSO currentRoomNode = null;//dung de xac dinh roomnode hien tai de thuc hien keo tha gi do
    private RoomNodeTypeListSO roomNodeTypeList;

    //Node layout values
    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    private float connectingLineWidth = 3f;
    //[MenuItem("Window/Dungeon Editor/Room Node Graph Editor")]
    [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]//ten va duong dan,vao cai duong dan nay se thuc hien cau lenh duoi ,mo ra hop thoai
    private static void OpenWindow()
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");//ten no la Room Node Graph Editor
    }

    private void OnEnable()
    {
        //Define node layout style
        roomNodeStype = new GUIStyle();
        roomNodeStype.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        roomNodeStype.normal.textColor = Color.white;//dat mau chu mau trang
        roomNodeStype.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeStype.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        //Load Room node types
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

  

    /// <summary>
    /// double click vao asset bat ky (hinh anh ,am thanh ,SO ,prefab ...) thi se goi xuong ham ben duoi
    /// Nhung phai click vao RoomNodeGraphSO thi moi Open Window va tra ve true
    /// instanceID chinh la ID cua thuc the ma double click vao
    /// </summary>
    [OnOpenAsset(0)] //Need the namespace UnityEditor.Callbacks,bat cu khi mo asset nao = DoubleClick
    public static bool OnDoubleClickAsset(int instanceID,int line)
    {
        RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;
        if(roomNodeGraph != null)
        {
            //neu la roomNodeGraph thi se cho mo ra
            OpenWindow();

            currentRoomNodeGraph = roomNodeGraph;
            return true;
        }
        //khong thi thoi
        return false;
    }



    /// <summary>
    /// Duoc goi khi trinh chinh sua co su thay doi
    /// Giong nhu ham Update goi lien tuc khi bi tac dong,bat len,thay doi kich thuoc,nhan vao ...
    /// </summary>
    private void OnGUI()
    {
        //GUILayout.BeginArea(new Rect(new Vector2(100f, 100f), new Vector2(nodeWidth, nodeHeight)), roomNodeStype);
        //EditorGUILayout.LabelField("Node 1");
        //GUILayout.EndArea();

        //GUILayout.BeginArea(new Rect(new Vector2(300f, 300f), new Vector2(nodeWidth, nodeHeight)), roomNodeStype);
        //EditorGUILayout.LabelField("Node 2");
        //GUILayout.EndArea();

        // If a scriptable object of type RoomNodeGraphSO has been selected then process
        if (currentRoomNodeGraph != null)
        {
            DrawDraggedLine();

            //Process Events
            ProcessEvents(Event.current);//xu ly cac su kien mousedown,up ,drag

            //Draw Room Nodes
            DrawRoomNodes();

            if (GUI.changed)//kiem tra xem co su thay doi GUI hay khong,neu co thi ve lai cua so
                Repaint();
        }
    }

    private void DrawDraggedLine()
    {
        if(currentRoomNodeGraph.linePosition != Vector2.zero)
        {
            //Draw line from node  to the line position
            Handles.DrawBezier(currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition,
                currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition, Color.white, null, connectingLineWidth);
        }    
    }    
    /// <summary>
    /// Process Room Node Graph Events
    /// </summary>
    private void ProcessEvents(Event currentEvent)
    {
        //Get room node that mouse is over if it's null or not currently being dragged
        if(currentRoomNode == null || currentRoomNode.isLeftClickDragging ==false)
        {
            currentRoomNode = IsMouseOverRoomNode(currentEvent);
        }
        //if mouse isn't over a room node
        if (currentRoomNode == null && currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            ProcessRoomNodeGraphEvents(currentEvent);

        }
        //else process room node events
        else
        {
            // process room node events
            currentRoomNode.ProcessEvents(currentEvent);
        }    
    }


    /// <summary>
    /// Check to see to mouse is over a room node - if so then return the room node else return null
    /// Trong RoomNodeGraph chua nhieu phong(to nho vua) ,ch
    /// </summary>
    private RoomNodeSO IsMouseOverRoomNode(Event currentEvent)
    {
        for(int i = currentRoomNodeGraph.roomNodeList.Count - 1; i >=0;i--)
        {
            if (currentRoomNodeGraph.roomNodeList[i].rect.Contains(currentEvent.mousePosition))
            {
                return currentRoomNodeGraph.roomNodeList[i];
            }    
        }
        return null;
    }

    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch(currentEvent.type)
        {
            // Process Mouse Down Events
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;
            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;
            case EventType.MouseUp:
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Process mouse down events on the room node graph (not over a node)
    /// Xu ly cac su kien nhan chuot tren Room node graph
    /// </summary>
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        //Process right click mouse down on graph event (show context menu)
        if(currentEvent.button == 1)
        {
            ShowContextMenu(currentEvent.mousePosition);
        }
    }
    /// <summary>
    /// Show the context menu
    /// </summary>
    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);

        menu.ShowAsContext();
    }

    /// <summary>
    /// Create a room node at the mouse position
    /// </summary>
    private void CreateRoomNode(object mousePositionObject)
    {
        CreateRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone));
    }
    /// <summary>
    /// Create a room node at the mouse position - overloaded to also pass in RoomNodeType
    /// </summary>
    private void CreateRoomNode(object mousePositionObject, RoomNodeTypeSO roomNodeType)
    {
        Vector2 mousePosition = (Vector2)mousePositionObject;

        //create room node scriptable object asset
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();

        //add room node to current room node graph room node list
        currentRoomNodeGraph.roomNodeList.Add(roomNode);

        //set room node value
        roomNode.Initialise(new Rect(mousePosition,new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph, roomNodeType);

        //add room node to room node graph scriptable object asset database
        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);

        AssetDatabase.SaveAssets();
    }

    private void ProcessMouseUpEvent(Event currentEvent)
    {
        if (currentEvent.button == 1 && currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            ClearLineDrag();
        }    
    }    

    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if(currentEvent.button == 1)
        {
            ProcessRightMouseDragEvent(currentEvent);
        }    
    }

    private void ProcessRightMouseDragEvent(Event currentEvent)
    {
        if(currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            DragConnectingLine(currentEvent.delta);
            GUI.changed = true; 
        }    
    }

    private void DragConnectingLine(Vector2 delta)
    {
        currentRoomNodeGraph.linePosition += delta;
    }


    private void ClearLineDrag()
    {
        currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
        currentRoomNodeGraph.linePosition = Vector2.zero;
        GUI.changed = true;
    }    

    /// <summary>
    /// Draw room nodes in the graph window
    /// </summary>
    private void DrawRoomNodes()
    {
        //Loop through all room nodes and draw themm
        foreach(RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            roomNode.Draw(roomNodeStype);
        }    
        GUI.changed = true;
    }    

    //change
}
