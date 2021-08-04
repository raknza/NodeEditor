using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

public class BehaviorEditor : NodeEditor
{

    [MenuItem("Window/Node Editor/BehaviorEditor")]
    static void ShowEditor()
    {
        NodeEditor editor = GetWindow<BehaviorEditor>();
        editor.titleContent = new GUIContent("Behavior Tree Editor");


    }
    void OnGUI()
    {
        // 載入管理器
        if (Manager == null)
        {
            BackroundTex = EditorGUIUtility.Load("Assets/NodeEditor/image/backround.png") as Texture2D;
            style.fontSize = 24;
            style.normal.textColor = Color.gray;
            
            Manager = new NodeManager(CreateConnection);
            Manager.SetList(node_list, connections);
        }
        GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), BackroundTex);
        GUILayout.Label(" Behavior Tree Editor", style);
        // 滑鼠移動事件
        wantsMouseMove = true;
        BeginWindows();
        //背景繪製
        // 繪製節點
        for (int i = 0; i < node_list.Count; i++)
        {
            node_list[i].Imp.SetRect(GUI.Window(i, node_list[i].Imp.rect, DrawNodeWindow, node_list[i].title));
            node_list[i].Imp.Draw();
        }
        // 繪製曲線
        for (int i = 0; connections != null && i < connections.Count; i++)
        {
            connections[i].Draw();
        }
        // 繪製目前拉出的線條
        DrawLine();
        // 工具列
        ToolBar();
        // 輸入事件
        ProcessEvents(Event.current);
        DrawGrid(20, 0.2f, Color.black);
        DrawGrid(100f, 0.2f, Color.black);
        Repaint();
        EndWindows();
    }
    private void ToolBar()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("load graph", GUILayout.Width(100), GUILayout.Height(20)))
        {

            // load graph
            // 選擇路徑
            OpenDialogDir ofn2 = new OpenDialogDir();
            ofn2.ulFlags = 0x00000010;
            ofn2.pszDisplayName = new string(new char[2000]); ;     // 緩存  
            ofn2.lpszTitle = "Choose Dialouge tree folder to load";
            IntPtr pidlPtr = DllOpenFileDialog.SHBrowseForFolder(ofn2);
            // 轉換選取路徑
            char[] charArray = new char[2000];
            for (int i = 0; i < 2000; i++)
                charArray[i] = '\0';
            DllOpenFileDialog.SHGetPathFromIDList(pidlPtr, charArray);
            string fullDirPath = new String(charArray);
            fullDirPath = fullDirPath.Substring(0, fullDirPath.IndexOf('\0'));
            // 讀取
            if (fullDirPath.Length > 0)
            {
                Directory.CreateDirectory(@fullDirPath + "/types/");
                Load(fullDirPath + "/", fullDirPath + "/types/");
                string[] path = fullDirPath.Split('\\');
                string title = path[path.Length - 1];
                File.WriteAllText("filelog", fullDirPath);
                graph_opened = true;
            }
        }
        if (GUILayout.Button("new graph", GUILayout.Width(100), GUILayout.Height(20)))
        {
            // new graph
            graph_opened = true;
        }
        if (GUILayout.Button("save graph", GUILayout.Width(100), GUILayout.Height(20)))
        {
            // save graph
            // 選擇路徑
            OpenDialogDir ofn2 = new OpenDialogDir();
            ofn2.ulFlags = 0x00000010 | 0x00000040;
            ofn2.pszDisplayName = new string(new char[2000]); ;     // 緩存  
            ofn2.lpszTitle = "Choose Dialouge tree folder to save";
            IntPtr pidlPtr = DllOpenFileDialog.SHBrowseForFolder(ofn2);

            char[] charArray = new char[2000];
            for (int i = 0; i < 2000; i++)
                charArray[i] = '\0';
            DllOpenFileDialog.SHGetPathFromIDList(pidlPtr, charArray);
            string fullDirPath = new String(charArray);
            fullDirPath = fullDirPath.Substring(0, fullDirPath.IndexOf('\0'));
            // 儲存
            if (fullDirPath.Length > 0)
            {
                // 儲存 建立types資料夾
                Directory.CreateDirectory(@fullDirPath + "/types/");
                Manager.Save(fullDirPath + "/", fullDirPath + "/types/");
            }
        }
        if (!graph_opened)
            GUI.enabled = false;
        if (GUILayout.Button("add node", GUILayout.Width(100), GUILayout.Height(20)))
        {
            // add node
            Addnode();

        }
        if (selected_node == null)
            GUI.enabled = false;
        if (GUILayout.Button("delete node", GUILayout.Width(100), GUILayout.Height(20)))
        {
            Deletenode(selected_node);
        }
        GUI.enabled = true; 
        GUILayout.EndHorizontal();
    }
    // 繪製節點視窗內容
    void DrawNodeWindow(int id)
    {

        int max_length = 20;
        int sum_lines = 0;
        // 繪製資料內容
        for (int i = 0; node_list[id].Compositer.GetData(i) != null; i++)
        {

            ExampleData data = (ExampleData)node_list[id].Compositer.Data[i];
            GUILayout.Label("state:");
            data.state = GUILayout.TextField(data.state);
            Rect state = GUILayoutUtility.GetLastRect();
            GUILayout.Label("action (funciton name):");
            data.action = GUILayout.TextField(data.action);
            Rect action = GUILayoutUtility.GetLastRect();
            // 偵測鼠標是否在資料區域內
            Vector2 pos = Event.current.mousePosition;
            if (state.Contains(pos) || action.Contains(pos))
            {
                selected_data = data;
            }
        }

        // 點擊節點視窗的相關處理
        // 對節點按下滑鼠左鍵
        if (Event.current.type == EventType.MouseDown && (Event.current.button == 0))
        {
            Debug.Log("選取節點");
            selected_node = node_list[id];
            // 清除選取
            Manager.ClearConnectionSelection();
            // 搜尋輸入輸出點具有connection者
            for (int i = 0; connections != null && i < connections.Count; i++)
            {
                // 選取設置為true
                if (connections[i].outPoint == selected_node.outPoint)
                {
                    connections[i].selected = true;
                }
                if (connections[i].inPoint == selected_node.inPoint)
                {
                    connections[i].selected = true;
                }
            }
                
        }
        // 對節點右鍵
        if (Event.current.type == EventType.MouseDown && (Event.current.button == 1))
        {
            // 節點右鍵選單
            GenericMenu genericMenu = new GenericMenu();
            if (selected_node != null)
            {
                //genericMenu.AddItem(new GUIContent("Add State"), false, () => AddState());
                genericMenu.AddItem(new GUIContent("Copy and Paste"), false, () => Addnode((UnityNode)selected_node));
                /*if (selected_data!=null)
                {
                    genericMenu.AddItem(new GUIContent("Delete State"), false, () => DeleteState());
                }*/
                genericMenu.AddItem(new GUIContent("Delete node"), false, () => Deletenode(selected_node));
            }
            genericMenu.ShowAsContext();
        }
        // 刪除鍵
        //if (delete_key == true && Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Delete)
            //Deletenode(selected_node);
        // 拖曳
        GUI.DragWindow();
    }

    public override void Addnode()
    {
        BaseNode new_node = new UnityNode(Manager.OnClickInPoint, Manager.OnClickOutPoint);
        node_list.Add(new_node);
        UnityConnectionPoint inPoint = (UnityConnectionPoint)node_list[node_list.Count - 1].inPoint;
        inPoint.SetImp(new AIConnectionPointImp(inPoint, node_list[node_list.Count - 1]));
        UnityConnectionPoint outPoint = (UnityConnectionPoint)node_list[node_list.Count - 1].outPoint;
        outPoint.SetImp(new AIConnectionPointImp(outPoint, node_list[node_list.Count - 1]));
        node_list[node_list.Count - 1].title = "State";
        AddState(node_list[node_list.Count - 1]);
    }
    public override void Addnode(Vector2 pos)
    {
        Rect rect = new Rect(pos.x, pos.y, 150, 100);
        BaseNode new_node = new UnityNode(rect, Manager.OnClickInPoint, Manager.OnClickOutPoint);
        node_list.Add(new_node);
        UnityConnectionPoint inPoint = (UnityConnectionPoint)node_list[node_list.Count - 1].inPoint;
        inPoint.SetImp(new AIConnectionPointImp(inPoint, node_list[node_list.Count - 1]));
        UnityConnectionPoint outPoint = (UnityConnectionPoint)node_list[node_list.Count - 1].outPoint;
        outPoint.SetImp(new AIConnectionPointImp(outPoint, node_list[node_list.Count - 1]));
        node_list[node_list.Count - 1].title = "State";
        AddState(node_list[node_list.Count - 1]);
    }
    public override void Addnode(UnityNode node)
    {
        BaseNode new_node = new UnityNode(node);
        node_list.Add(new_node);
        UnityConnectionPoint inPoint = (UnityConnectionPoint)node_list[node_list.Count - 1].inPoint;
        inPoint.SetImp(new AIConnectionPointImp(inPoint, node_list[node_list.Count - 1]));
        UnityConnectionPoint outPoint = (UnityConnectionPoint)node_list[node_list.Count - 1].outPoint;
        outPoint.SetImp(new AIConnectionPointImp(outPoint, node_list[node_list.Count - 1]));
        node_list[node_list.Count - 1].title = "State";
    }


    void AddState(BaseNode node)
    {

        ExampleData test = new ExampleData();
        test.state = "idle";
        AddData(node, test);

    }

    void DeleteState()
    {
        if (selected_node != null)
        {
            selected_node.Compositer.DeleteData(selected_data);
            selected_data = null;
        }
    }

    // 創造連線 決定實體
    public new void CreateConnection()
    {
        UnityConnection Con = new UnityConnection(Manager.selectedInPoint, Manager.selectedOutPoint, Manager.OnClickRemoveConnection);
        Con.SetImp(new LineConnectionImp(Con));
        connections.Add(Con);
    }


}



