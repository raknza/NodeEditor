using System;
using UnityEngine;

public class UnityNode :BaseNode{

    public UnityNode() { }

    // 複製
    public UnityNode(UnityNode Node)
    {
        Imp = new BaseNodeImp((BaseNodeImp)Node.Imp, this);
        Compositer = (CompositeData)Node.Compositer.copy();
        inPoint = new UnityConnectionPoint(this, ConnectionPointType.In, Node.inPoint.OnClickConnectionPoint, null);
        outPoint = new UnityConnectionPoint(this, ConnectionPointType.Out, Node.outPoint.OnClickConnectionPoint, null);
        title = Node.title;
    }
    // 預設新增
    public UnityNode(Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint) : base()
    {
        Imp = new BaseNodeImp(this);
        Compositer = new CompositeData();
        inPoint = new UnityConnectionPoint(this, ConnectionPointType.In, OnClickInPoint, null);
        outPoint = new UnityConnectionPoint(this, ConnectionPointType.Out, OnClickOutPoint, null);
        title = "node";
    }

    // 指定位置創造
    public UnityNode(Rect rect, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint) : base()
    {
        Imp = new BaseNodeImp(rect, this);
        Compositer = new CompositeData();
        inPoint = new UnityConnectionPoint(this, ConnectionPointType.In, OnClickInPoint, null);
        outPoint = new UnityConnectionPoint(this, ConnectionPointType.Out, OnClickOutPoint, null);
        title = "node";
    }

    // 讀取
    public UnityNode(Rect rect ,Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint,string inPointID, string outPointID) :base()
    {
        Imp = new BaseNodeImp(rect,this);
        Compositer = new CompositeData();
        inPoint = new UnityConnectionPoint(this, ConnectionPointType.In, OnClickInPoint, inPointID);
        outPoint = new UnityConnectionPoint(this, ConnectionPointType.Out, OnClickOutPoint, outPointID);
        title = "node";
    }

    public void Draw()
    {
        Imp.Draw();
    }

    public override Type getType()
    {
        return typeof(UnityNode);
    }
    public void SetImp(BaseImp imp)
    {
        Imp = imp;
    }
}
