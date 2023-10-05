using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//loai phong boss,nho,loi di....
[CreateAssetMenu(fileName = "RoomNodeType", menuName = "ScriptableObjects/Dugeon/Room Node Type")]
public class RoomNodeTypeSO : ScriptableObject
{
    public string roomNodeTypeName;

    #region Header
    [Header("Only flag the RoomNodeTypes that should be visible in the editor")]//chi gan co cac RoomNodeType co hien thi trinh chinh sua
    #endregion Header
    public bool displayInNodeGraphEditor = true;
    #region Header
    [Header("One Type Should Be A Corridor")] //co phai hanh lang hay khong
    #endregion Header
    public bool isCorridor;
    #region Header
    [Header("One Type Should Be A CorridorNS")] //NS
    #endregion Header
    public bool isCorridorNS;
    #region Header
    [Header("One Type Should Be A CorridorEW")] //EW
    #endregion Header
    public bool isCorridorEW;
    #region Header
    [Header("One Type Should Be An Entrance")] // cong vao
    #endregion Header
    public bool isEntrance;
    #region Header
    [Header("One Type Should Be A Boss Room")]
    #endregion Header
    public bool isBossRoom;
    #region Header
    [Header("One Type Should Be None (Unassigned)")]
    #endregion Header
    public bool isNone;

    //tham dinh
    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName);
    }
#endif
    #endregion
}
