using System;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class LineConnectionImp : BaseImp
{

    // 連線是否被選取
    [XmlIgnore] Connection connection;

    public LineConnectionImp() : base() { }

    public LineConnectionImp(Connection connection) : base()
    {
        this.connection = connection;
        style = new GUIStyle();
    }

    public override void Draw()
    {
        Handles.DrawBezier(
            connection.inPoint.Imp.rect.center,
            connection.outPoint.Imp.rect.center,
            connection.inPoint.Imp.rect.center ,
            connection.outPoint.Imp.rect.center,
            connection.selected == false ? Color.gray : Color.red, // 被選取時繪製為紅色 平時為灰色
            null,
            3f
        );
        // 移除相連
        if (Handles.Button((connection.inPoint.Imp.rect.center + connection.outPoint.Imp.rect.center) * 0.5f, Quaternion.identity, 8, 16, Handles.RectangleCap))
        {
            if (connection.OnClickRemoveConnection != null)
            {
                connection.OnClickRemoveConnection(connection);
            }
        }
    }
    public override Type getType()
    {
        return typeof(LineConnectionImp);
    }
}
