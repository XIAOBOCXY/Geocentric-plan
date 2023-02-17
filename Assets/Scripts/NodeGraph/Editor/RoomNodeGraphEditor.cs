using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;//EditorWindow
public class RoomNodeGraphEditor : EditorWindow //�̳�EditorWindow��
{
    private GUIStyle roomNodeStyle;
    private const float nodeWidth = 160f;//����
    private const float nodeHeight = 75f;//����
    private const int nodePadding = 25;//����ڱ߾�
    private const int nodeBorder = 12;//���߽�
    [MenuItem("Room Node Graph Editor",menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]//��Ӳ˵���
    private static void openWindow()//���������򿪱༭������
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");//���ش���
    }
    private void OnEnable()
    {
        roomNodeStyle = new GUIStyle();//GUIStyle���Ի����Ѿ����ڵ�ʵ��newһ���µ�ʵ��,ֻ���ԭ�е�Ч���в������Լ�����Ľ����޸�
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;//����һ��������Դ����ʽΪnode1
        roomNodeStyle.normal.textColor = Color.white;//���������ɫΪ��ɫ
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);//�ڱ߾�
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);//�߽�
    }
    private void OnGUI()
    {
        Debug.Log("OnGUI has been called");//������־

        //���1
        GUILayout.BeginArea(new Rect(new Vector2(100f, 100f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);//λ�ã���С����ʽ
        EditorGUILayout.LabelField("Node 1");//�ı�
        GUILayout.EndArea();//��������

        //���2
        GUILayout.BeginArea(new Rect(new Vector2(300f, 300f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);//λ�ã���С����ʽ
        EditorGUILayout.LabelField("Node 2");//�ı�
        GUILayout.EndArea();//��������
    }
}