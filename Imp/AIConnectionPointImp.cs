using System;
using UnityEditor;
using UnityEngine;

public class AIConnectionPointImp : BaseImp
{
    ConnectionPoint point;
    BaseNode node;

    public AIConnectionPointImp() : base() { }

    public AIConnectionPointImp(ConnectionPoint point,BaseNode node) : base()
    {
        this.point = point;
        this.node = node;
        style = new GUIStyle();
        this.style.normal.background = (Texture2D)EditorGUIUtility.Load("Assets/NodeEditor/image/btnleft.png") as Texture2D;
        this.style.active.background = (Texture2D)EditorGUIUtility.Load("Assets/NodeEditor/image/btnlefton.png") as Texture2D;
        this.style.border = new RectOffset(4, 4, 12, 12);
        rect = new Rect(0, 0, 10f, 10f);
    }

    public override void Draw()
    {
        switch (point.type)
        {
            case ConnectionPointType.In:
                Rect newRect = new Rect(node.Imp.rect.x + node.Imp.rect.width/2-5, node.Imp.rect.y-6 , rect.width, rect.height);
                SetRect(newRect);
                break;

            case ConnectionPointType.Out:
                newRect = new Rect(node.Imp.rect.x + node.Imp.rect.width / 2-5, node.Imp.rect.y + node.Imp.rect.height-3, rect.width, rect.height);
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
        return typeof(AIConnectionPointImp);
    }
}
