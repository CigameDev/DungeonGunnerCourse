using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;



public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeType;
    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeTypeListSO roomNodeTypeList;

    //Node layout values
    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    [MenuItem("Room Node Graph Editor",menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]//ten va duong dan

    private static void OpenWindow()
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
    }

    private void OnEnable()
    {
        //Define node layout style
        roomNodeType = new GUIStyle();
        roomNodeType.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        roomNodeType.normal.textColor = Color.white;
        roomNodeType.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeType.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        //Load Room node types
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    [OnOpenAsset(0)] //Need the namespace UnityEditor.Callbacks
    public static bool OnDoubleClickAsset(int instanceID,int line)
    {
        RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;
        if(roomNodeGraph != null)
        {
            OpenWindow();

            currentRoomNodeGraph = roomNodeGraph;

            return true;
        }   
        return false;
    }    
    
    /// <summary>
    /// Draw Editor Gui
    /// Giong nhu ham Update goi lien tuc
    /// </summary>
    private void OnGUI()
    {
        //GUILayout.BeginArea(new Rect(new Vector2(100f,100f),new Vector2(nodeWidth,nodeHeight)),roomNodeStype);
        //EditorGUILayout.LabelField("Node 1");
        //GUILayout.EndArea();

        //GUILayout.BeginArea(new Rect(new Vector2(300f, 300f), new Vector2(nodeWidth, nodeHeight)), roomNodeStype);
        //EditorGUILayout.LabelField("Node 2");
        //GUILayout.EndArea();

        // If a scriptable object of type RoomNodeGraphSO has been selected then process
        if(currentRoomNodeGraph != null)
        {
            //Process Events
            ProcessEvents(Event.current);

            //Draw Room Nodes
            DrawRoomNodes();

            if (GUI.changed)
                Repaint();
        }    
    }

    /// <summary>
    /// Process Room Node Graph Events
    /// </summary>
    /// <param name="currentEvent"></param>
    private void ProcessEvents(Event currentEvent)
    {
        ProcessRoomNodeGraphEvents(currentEvent);
    }

    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch(currentEvent.type)
        {
            // Process Mouse Down Events
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

            default:
                break;
        }
    }
    /// <summary>
    /// Process mouse down events on the room node graph (not over a node)
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
    private void CreateRoomNode(object mousePositionObject, RoomNodeTypeSO roomNodeTypeSO)
    {
        Vector2 mousePosition = (Vector2)mousePositionObject;

        //create room node scriptable object asset
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();

        //add room node to current room node graph room node list
        currentRoomNodeGraph.roomNodeList.Add(roomNode);

        //set room node value
        //ve mo cai ben duoi ra xem video 10 luc 36 phut
        //roomNode.Initialise(new Rect(mousePosition,new Vector2(nodeWidth,nodeHeight)),currentRoomNodeGraph,roomNodeType);

        //add room node to room node graph scriptable object asset database
        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);

        AssetDatabase.SaveAssets();
    }

    private void DrawRoomNodes()
    {

    }    
}
