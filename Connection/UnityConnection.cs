using System;


public class UnityConnection:Connection
{


    public UnityConnection():base() { }

    public UnityConnection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection):base()
    {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        this.OnClickRemoveConnection = OnClickRemoveConnection;
        Imp = new ConnectionImp(this);
    }
    public override Type getType()
    {
        return typeof(UnityConnection);
    }
    public void SetImp(BaseImp imp)
    {
        Imp = imp;
    }
}
