//房间节点类型列表脚本化对象类
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeTypeListSO", menuName = "Scriptable Object/Dungeon/Room Node Type List")] //添加Assets菜单按钮
public class RoomNodeTypeListSO : ScriptableObject  //继承ScriptableObject类
{
    #region Header ROOM NODE TYPE LIST
    [Space(10)]
    [Header("房间节点类型列表")]
    #endregion
    #region Tooltip
    [Tooltip("此列表应填充所有的游戏房间节点类型可编写脚本的对象")]
    #endregion
    public List<RoomNodeTypeSO> list;

    #region Validation 
#if UNITY_EDITOR //平台判断，只有在unity编辑器中执行，才会执行以下代码
    private void OnValidate() //用来验证一些数据，脚本加载或Inspector中的任何值被修改时会调用
    {
        HelperUtlities.ValidateCheckEnumerableValues(this, nameof(list), list); //判断房间节点类型列表是否为空字符串
    }
#endif
    #endregion
}
