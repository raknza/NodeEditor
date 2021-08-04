using System;
using System.Collections.Generic;

public class CompositeData : BaseData {

    public List<BaseData> Data{
        get;
        private set;
    }


    public CompositeData():base()
    {
        Data = new List<BaseData>();
    }

    public void AddData(BaseData data)
    {
        Data.Add(data);
    }

    public void DeleteData(BaseData data)
    {
        for (int i = 0; i < Data.Count; i++)
        {
            if (Data[i] == data == true)
                Data.Remove(Data[i]);
        }
    }

    public BaseData GetData(int index)
    {
        if(index < Data.Count )
            return Data[index];
        return null;
    }

    public BaseData GetData(BaseData data)
    {
        for(int i = 0; i < Data.Count; i++)
        {
            if (Data[i] == data)
                return Data[i];
        }
        return null;
    }

    public override bool equals(BaseData data)
    {
        for (int i = 0; i < Data.Count; i++)
        {
            if (Data[i].equals(data) != true)
                return false;
        }
        return true;
    }

    public override Type getType()
    {
        return typeof(CompositeData);
    }

    public override BaseData copy()
    {
        CompositeData copyComp = new CompositeData();
        for (int i = 0; i < Data.Count; i++)
        {
            BaseData copydata = Data[i].copy();
            copyComp.AddData(copydata);
            
        }
        return copyComp;
    }
}
