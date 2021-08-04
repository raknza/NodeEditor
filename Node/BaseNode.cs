using System;
using System.Xml.Serialization;

public abstract class BaseNode
{
    public string title;
    public CompositeData Compositer;
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    public BaseImp Imp
    {
        get;
        protected set;
    }

    public BaseNode() { }

    abstract public Type getType();

}
