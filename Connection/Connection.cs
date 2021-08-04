using System;
using System.Xml.Serialization;

public abstract class Connection
{
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    [XmlIgnore] public Action<Connection> OnClickRemoveConnection;
    [XmlIgnore] public bool selected = false;

    [XmlIgnore] public BaseImp Imp
    {
        get;
        protected set;
    }

    public Connection() { }

    public void Draw()
    {
        Imp.Draw();
    }
    public abstract Type getType();
}
