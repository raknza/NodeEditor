using System;
using System.Collections.Generic;

public class NodeManager {
    public List<BaseNode> node_list
    {
        get;
        protected set;
    }
    public List<Connection> connections
    {
        get;
        protected set;
    }

    // 創造連線委派
    protected Action CreateConnection;

    // 選取之輸入輸出點
    public ConnectionPoint selectedInPoint {
        get;
        protected set;
    }
    public ConnectionPoint selectedOutPoint {
        get;
        protected set;
    }

    // 實體物件管理由其他類別自行完成 自行決定實體的node connection等等的類別

    public NodeManager(Action CreateConnection) {
        this.CreateConnection = CreateConnection;
    }

    public void SetList(List<BaseNode> node_list, List<Connection> connections)
    {
        this.node_list = node_list;
        this.connections = connections;
    }

    //選取輸入點
    public void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;
        
        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }
    // 選取輸出點
    public void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;
        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }
    // 新增connection

    // 清除連接選取
    public void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
        // 搜尋輸入點有connection的
        for (int i = 0; connections != null && i < connections.Count; i++)
        {
            connections[i].selected = false;
        }
    }
    // 點擊刪除連線
    public void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }

    // 儲存節點列表 連線列表 各類型node,data,connectionpoint,connection,imp
    public void Save(string path1, string path2)
    {
        // 抓取subclass之各type
        List<Type> extraDataTypes = new List<Type>();
        List<Type> extraNodeTypes = new List<Type>();
        List<Type> extraImpTypes = new List<Type>();
        List<Type> extraConnectionTypes = new List<Type>();
        List<Type> extraConnectionPointTypes = new List<Type>();
        for (int j = 0; j < node_list.Count; j++)
        {
            BaseNode node = node_list[j];
            int k = 0;
            // 抓取所有節點內所有的Data type
            for (int i = 0; i < node.Compositer.Data.Count; i++)
            {

                for (k = 0; k < extraDataTypes.Count; k++)
                {
                    if (extraDataTypes[k] == node.Compositer.Data[i].getType())
                        break;
                }
                // 加入 type
                if (k == extraDataTypes.Count)
                {
                    extraDataTypes.Add(node.Compositer.Data[i].getType());

                }

            }
            // 抓取Node type
            for (k = 0; k < extraNodeTypes.Count; k++)
            {
                if (extraNodeTypes[k] == node.getType())
                    break;
            }
            if (k == extraNodeTypes.Count)
            {
                extraNodeTypes.Add(node.getType());
            }

            // 抓取 Node Imp type
            for (k = 0; k < extraImpTypes.Count; k++)
            {
                if (extraImpTypes[k] == node.Imp.getType())
                    break;
            }
            if (k == extraImpTypes.Count)
            {
                extraImpTypes.Add(node.Imp.getType());
            }
            // 抓取ConnectionPoint 與 ConnectionPointImp
            for (k = 0; k < extraConnectionPointTypes.Count; k++)
            {
                if (extraConnectionPointTypes[k] == node.inPoint.getType())
                    break;
            }
            if (k == extraConnectionPointTypes.Count)
            {
                extraConnectionPointTypes.Add(node.inPoint.getType());
            }
            for (k = 0; k < extraImpTypes.Count; k++)
            {
                if (extraImpTypes[k] == node.inPoint.Imp.getType())
                    break;
            }
            if (k == extraImpTypes.Count)
            {
                extraImpTypes.Add(node.inPoint.Imp.getType());
                //Debug.Log(node.Imp.getType());
            }

        }

        // 抓取Connection 與 ConnectionImp

        for (int k = 0; k < connections.Count; k++)
        {
            int t;
            Connection connection = connections[k];
            // 抓取 Connection Type
            for (t = 0; t < extraConnectionTypes.Count; t++)
            {
                if (extraConnectionTypes[t] == connection.getType())
                    break;
            }
            if (t == extraConnectionTypes.Count)
            {
                extraConnectionTypes.Add(connection.getType());
                //Debug.Log(node.Imp.getType());
            }
            // 抓取ConnectionImp
            for (t = 0; t < extraImpTypes.Count; t++)
            {
                if (extraImpTypes[t] == connection.Imp.getType())
                    break;
            }
            if (t == extraImpTypes.Count)
            {
                extraImpTypes.Add(connection.Imp.getType());
                //Debug.Log(node.Imp.getType());
            }
        }

        // 儲存DataType列表
        List<string> DataTypesName = new List<string>();
        for (int i = 0; i < extraDataTypes.Count; i++)
        {
            DataTypesName.Add(extraDataTypes[i].Name);
        }

        // 儲存NodeType列表
        List<string> NodeTypesName = new List<string>();
        for (int i = 0; i < extraNodeTypes.Count; i++)
        {
            NodeTypesName.Add(extraNodeTypes[i].Name);
        }

        // 儲存ImpType列表
        List<string> ImpTypesName = new List<string>();
        for (int i = 0; i < extraImpTypes.Count; i++)
        {
            ImpTypesName.Add(extraImpTypes[i].Name);
        }

        // 儲存Connection Type列表
        List<string> ConnectionTypesName = new List<string>();
        for (int i = 0; i < extraConnectionTypes.Count; i++)
        {
            ConnectionTypesName.Add(extraConnectionTypes[i].Name);
        }

        // 儲存ConnectionPointType列表
        List<string> ConnectionPointTypesName = new List<string>();
        for (int i = 0; i < extraConnectionPointTypes.Count; i++)
        {
            ConnectionPointTypesName.Add(extraConnectionPointTypes[i].Name);
        }

        // 合併Types列表
        List<Type> extraTypes = new List<Type>();
        extraTypes.AddRange(extraDataTypes);
        extraTypes.AddRange(extraNodeTypes);
        extraTypes.AddRange(extraConnectionTypes);
        extraTypes.AddRange(extraImpTypes);
        extraTypes.AddRange(extraConnectionPointTypes);

        XMLOP.Serialize(node_list, path1 + "Nodes.xml", path2, extraTypes, DataTypesName, NodeTypesName, ImpTypesName, ConnectionPointTypesName);
        XMLOP.Serialize(connections, path1 + "Connections.xml", path2, extraTypes, ConnectionTypesName);
    }

    public List<BaseNode> LoadNodes(string NodesPath, string TypesPath)
    {
        List<BaseNode> load_list = XMLOP.Deserialize<List<BaseNode>>(NodesPath, TypesPath);
        return load_list;
    }
    public List<Connection> LoadConnections(string ConnectionsPath, string TypesPath)
    {
        List<Connection> load_connection = XMLOP.Deserialize<List<Connection>>(ConnectionsPath+"Connections.xml", TypesPath);
        return load_connection;
    }
}