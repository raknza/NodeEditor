using System;
using System.Xml.Serialization;

public enum ConnectionPointType { In, Out }

public abstract class ConnectionPoint
{
    public string id
    {
        get;
        protected set;
    }

    [XmlIgnore] public BaseImp Imp
    {
        get;
        protected set;
    }

    // 連接點類型
    [XmlIgnore] public ConnectionPointType type;

    // 連接節點
    [XmlIgnore] public BaseNode node;

    // 委派點擊連接點的method
    [XmlIgnore] public Action<ConnectionPoint> OnClickConnectionPoint;
    public ConnectionPoint() { }


    public ConnectionPoint(BaseNode node, ConnectionPointType type, Action<ConnectionPoint> OnClickConnectionPoint, string id)
    {
        this.node = node;
        this.type = type;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        this.id = (id == null ? Guid.NewGuid().ToString() : id);

    }

    public abstract Type getType();

}