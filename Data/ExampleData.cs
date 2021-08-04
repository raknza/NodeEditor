using System;

public class ExampleData : BaseData
{
    public string state;
    public string action;

    public ExampleData() : base()
    {
    }

    public override BaseData copy()
    {
        ExampleData copydata = new ExampleData();
        copydata.state = this.state;
        copydata.action = this.action;
        return copydata;
    }

    public override bool equals(BaseData data)
    {
        throw new System.NotImplementedException();
    }

    public override Type getType()
    {
        return typeof(ExampleData); 
    }
}
