using System;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class ConnectionPointImp : BaseImp
{
    ConnectionPoint point;
    BaseNode node;

    public ConnectionPointImp() : base() { }

    public ConnectionPointImp(ConnectionPoint point,BaseNode node) : base()
    {
        this.point = point;
        this.node = node;
        style = new GUIStyle();
        this.style.normal.background = (Texture2D)EditorGUIUtility.Load("Assets/NodeEditor/image/btnleft.png") as Texture2D;
        this.style.active.background = (Texture2D)EditorGUIUtility.Load("Assets/NodeEditor/image/btnlefton.png") as Texture2D;
        this.style.border = new RectOffset(4, 4, 12, 12);
        rect = new Rect(0, 0, 20f, 20f);
    }

    public override void Draw()
    {
        switch (point.type)
        {
            case ConnectionPointType.In:
                Rect newRect = new Rect(node.Imp.rect.x - rect.width, node.Imp.rect.y + (node.Imp.rect.height * 0.5f) - rect.height * 0.9f, rect.width, rect.height);
                SetRect(newRect);
                break;

            case ConnectionPointType.Out:
                newRect = new Rect(node.Imp.rect.x + node.Imp.rect.width, node.Imp.rect.y + (node.Imp.rect.height * 0.5f) + rect.height * 0.9f, rect.width, rect.height);
                SetRect(newRect);
                break;
        }
        if (GUI.Button(rect, "", style))
        {
            if (point.OnClickConnectionPoint != null)
            {
                point.OnClickConnectionPoint(point);
            }
        }
    }
    public override Type getType()
    {
        return typeof(ConnectionPointImp);
    }
}
