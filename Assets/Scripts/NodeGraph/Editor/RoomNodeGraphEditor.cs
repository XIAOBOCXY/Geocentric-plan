using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;//EditorWindow
public class RoomNodeGraphEditor : EditorWindow //继承EditorWindow类
{
    private GUIStyle roomNodeStyle;
    private const float nodeWidth = 160f;//结点宽
    private const float nodeHeight = 75f;//结点高
    private const int nodePadding = 25;//结点内边距
    private const int nodeBorder = 12;//结点边界
    [MenuItem("Room Node Graph Editor",menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]//添加菜单项
    private static void openWindow()//创建函数打开编辑器窗口
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");//返回窗口
    }
    private void OnEnable()
    {
        roomNodeStyle = new GUIStyle();//GUIStyle可以基于已经存在的实例new一个新的实例,只需对原有的效果中不符合自己需求的进行修改
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;//加载一个内置资源，样式为node1
        roomNodeStyle.normal.textColor = Color.white;//结点字体颜色为白色
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);//内边距
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);//边界
    }
    private void OnGUI()
    {
        Debug.Log("OnGUI has been called");//调试日志

        //结点1
        GUILayout.BeginArea(new Rect(new Vector2(100f, 100f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);//位置，大小，样式
        EditorGUILayout.LabelField("Node 1");//文本
        GUILayout.EndArea();//结束区域

        //结点2
        GUILayout.BeginArea(new Rect(new Vector2(300f, 300f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);//位置，大小，样式
        EditorGUILayout.LabelField("Node 2");//文本
        GUILayout.EndArea();//结束区域
    }
}