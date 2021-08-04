using System;


public class UnityConnectionPoint:ConnectionPoint
{
    public UnityConnectionPoint() : base() { }

    public UnityConnectionPoint(BaseNode node, ConnectionPointType type, Action<ConnectionPoint> OnClickConnectionPoint, string id) : base(node, type, OnClickConnectionPoint, id)
    {
        Imp = new ConnectionPointImp(this, node);

    }


    public override Type getType()
    {
        return typeof(UnityConnectionPoint);
    }
    public void SetImp(BaseImp imp)
    {
        Imp = imp;
    }
}