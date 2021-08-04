using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public abstract class BaseImp {
    // 風格
    [XmlIgnore] public GUIStyle style;

    // 節點區域資訊
    public Rect rect
    {
        get;
        protected set;
    }

    public BaseImp() 
    {
    }
    public void SetRect(Rect rect)
    {
        this.rect = rect;
    }
    public abstract void Draw(); // 繪製
    public abstract Type getType();

}
