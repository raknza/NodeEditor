using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public abstract class NodeEditor : EditorWindow
{

    // node list
    protected List<BaseNode> node_list = new List<BaseNode>();
    // connection list
    protected List<Connection> connections = new List<Connection>();
    // node manager
    protected NodeManager Manager ; 
    // 是否開啟圖
    protected bool graph_opened = false;
    // 是否選取節點
    protected BaseNode selected_node = null;
    // 是否選取資料
    protected BaseData selected_data = null;

    protected Vector2 mouse_pos_start;

    // GUI風格
    protected GUIStyle style = new GUIStyle();

    protected  Texture2D BackroundTex;
    [MenuItem("Window/Node Editor")]
    static void ShowEditor()
    {
        NodeEditor editor = GetWindow<NodeEditor>();
        editor.titleContent = new GUIContent("BaseNode Editor");


    }
    protected void DrawLine()
    {
        // 若目前選取任一輸入點或輸出點則繪製一條由該節點 選取之點至滑鼠的線
        if (Manager.selectedOutPoint != null || Manager.selectedInPoint != null)
        {
            Handles.DrawBezier(
                Manager.selectedInPoint != null? Manager.selectedInPoint.Imp.rect.center: Manager.selectedOutPoint.Imp.rect.center,
                Event.current.mousePosition,
                Event.current.mousePosition + Vector2.left * 50f,
                Manager.selectedInPoint != null ? Manager.selectedInPoint.Imp.rect.center : Manager.selectedOutPoint.Imp.rect.center + - Vector2.left * 50f,
                Color.gray,
                null,
                3f
            );
            
        }
       }
    public void Load(string path1, string path2)
    {
        List<BaseNode> load_nodes = Manager.LoadNodes(path1 + "nodes.xml", path2);
        List<Connection> load_connections = Manager.LoadConnections(path1, path2);

        // 將節點加入節點list
        for (int i = 0; i < load_nodes.Count; i++)
        {

            // 讀取節點之 rect 及 inPoint(id)

            node_list.Add(new UnityNode(
                load_nodes[i].Imp.rect,
                Manager.OnClickInPoint,
                Manager.OnClickOutPoint,
                load_nodes[i].inPoint.id,
                load_nodes[i].outPoint.id
                )
            );
            node_list[i].Compositer = load_nodes[i].Compositer;

        }
        // 讀入 connection 依照id建立連結
        for (int i = 0; i < load_connections.Count; i++)
        {

            ConnectionPoint inPoint = null;
            ConnectionPoint outPoint = null;
            for (int j = 0; j < node_list.Count; j++)
            {
                if (node_list[j].inPoint.id == load_connections[i].inPoint.id)
                {
                    inPoint = node_list[j].inPoint;

                }
                if (node_list[j].outPoint.id == load_connections[i].outPoint.id)
                {
                    outPoint = node_list[j].outPoint;
                }
                if (inPoint != null && outPoint != null)
                    break;

            }
            connections.Add(new UnityConnection(inPoint, outPoint, Manager.OnClickRemoveConnection));
        }

    }


    // 新增UnityNode節點 
    public virtual void Addnode()
    {
        BaseNode new_node = new UnityNode(Manager.OnClickInPoint, Manager.OnClickOutPoint);
        node_list.Add(new_node);
    }
    public virtual void Addnode(Vector2 pos)
    {
        Rect rect = new Rect(pos.x, pos.y, 150, 100);
        BaseNode new_node = new UnityNode(rect,Manager.OnClickInPoint, Manager.OnClickOutPoint);

        node_list.Add(new_node);
    }
    public virtual void Addnode(UnityNode node)
    {
        BaseNode new_node = new UnityNode(node);
        node_list.Add(new_node);
    }

    public void AddData(BaseNode node, BaseData data)
    {
        node.Compositer.AddData(data);
    }

    public void Deletenode(BaseNode selected_node)
    {
        if (connections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            // 搜尋輸入點有connection的
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].inPoint == selected_node.inPoint || connections[i].outPoint == selected_node.outPoint)
                {
                    connectionsToRemove.Add(connections[i]);
                }
            }
            // 輸出點
            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                connections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }
        if (selected_node != null)
            node_list.Remove(selected_node);
        selected_node = null;
    }

    // 創造連線 決定實體
    public void CreateConnection()
    {
        connections.Add(new UnityConnection(Manager.selectedInPoint, Manager.selectedOutPoint, Manager.OnClickRemoveConnection));
    }

    // 輸入
    protected void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                // 在背景開啟選單
                if (e.button == 1) // 右鍵
                {
                    ProcessContextMenu(e.mousePosition);
                }
                if(e.button == 0) // 左鍵(取消選取)
                {
                    selected_node = null;
                    // 清除選取
                    Manager.ClearConnectionSelection();
                    mouse_pos_start = new Vector2(e.mousePosition.x, e.mousePosition.y);
                }
                break;
            case EventType.MouseDrag:
                Vector2 newPos = new Vector2(e.mousePosition.x - mouse_pos_start.x, e.mousePosition.y - mouse_pos_start.y);
                newPos *= 0.4f;
                for (int i = 0; i < node_list.Count; i++)
                {
                    
                    Rect rect = node_list[i].Imp.rect;
                    rect.x += newPos.x ;
                    rect.y += newPos.y;
                    node_list[i].Imp.SetRect(rect);
                }
                mouse_pos_start = new Vector2(e.mousePosition.x, e.mousePosition.y);
                offset = offset + newPos;
                break;


        }
    }
    // 背景下右鍵選單
    protected void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        if (!graph_opened)
        {
            genericMenu.AddDisabledItem((new GUIContent("Add node")));
        }
        else
        {
            genericMenu.AddItem(new GUIContent("Add node"), false, () => Addnode(mousePosition));
        }
        genericMenu.ShowAsContext();
    }

    protected Vector2 offset = new Vector2(0,0);
    protected void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.black;
        Handles.EndGUI();
    }

}



