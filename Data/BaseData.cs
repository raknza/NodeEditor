using System;
using System.Xml.Serialization;

public abstract class BaseData {

    public abstract Type getType();
    public abstract BaseData copy();
    public abstract bool equals(BaseData data);

}
