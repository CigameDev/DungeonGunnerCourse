using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNode", menuName = "ScriptableObjects/Dugeon/Room Node ")]
public class RoomNodeSO : ScriptableObject
{
    
    [HideInInspector] public string id;
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>();
    [HideInInspector] public List<string> childRoomNodeIDList =  new List<string> ();
    [HideInInspector] public RoomNodeGraphSO roomNodeGraphSO;
    public RoomNodeTypeSO roomNodeType;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
}