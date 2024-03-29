using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeTypeListSO", menuName = "ScriptableObjects/Dugeon/Room Node Type List")]
public class RoomNodeTypeListSO : ScriptableObject
{
    #region Header ROOM NODE TYPE LIST
    [Space(10)]
    [Header("ROOM NODE TYPE LIST")]
    #endregion
    #region Tooltip
    [Tooltip("Danh sach nay chua tat ca cac RoomNodeTypeSO trong game - su dung thay vi 1 enum")]
    #endregion
    public List<RoomNodeTypeSO> list;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(list), list);
    }
#endif
    #endregion
}
