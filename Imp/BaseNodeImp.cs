using System;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class BaseNodeImp : BaseImp
{
    [XmlIgnore] BaseNode node;

    public BaseNodeImp() : base() { }

    public BaseNodeImp(BaseNodeImp imp,BaseNode node) : base()
    {
        this.node = node;
        rect = new Rect(imp.rect);
    }

    public BaseNodeImp(Rect rect, BaseNode node) : base() {
        this.node = node;
        //style = new GUIStyle();
        //style.normal.background = (Texture2D)EditorGUIUtility.Load("Assets/NodeEditor/image/node_backround.png") as Texture2D;
        this.rect = rect;
    }
    public BaseNodeImp(BaseNode node) : base()
    {
        this.node = node;
        //style = new GUIStyle(GUI.skin.window);
        //style.normal.background = (Texture2D)EditorGUIUtility.Load("Assets/NodeEditor/image/node_backround.png") as Texture2D;
        rect = new Rect(50, 50, 200, 100);
    }

    public override void Draw()
    {
        node.inPoint.Imp.Draw();
        node.outPoint.Imp.Draw();
    }

    public override Type getType()
    {
        return typeof(BaseNodeImp);
    }
}
