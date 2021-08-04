using System.Xml.Serialization;
using System.IO;
using System;
using System.Collections.Generic;

public static class XMLOP
{


    public static void Serialize(object item, string path1,string path2, List<Type> extraTypes, List<string> ConnectionTypesName)
    {
        // 儲存資料本體
        XmlSerializer serializer = new XmlSerializer(item.GetType(),extraTypes.ToArray());
        StreamWriter writer = new StreamWriter(path1);
        serializer.Serialize(writer.BaseStream, item);

        // 儲存 Connection Type String
        serializer = new XmlSerializer(ConnectionTypesName.GetType());
        writer = new StreamWriter(path2+"ConnectionType.xml");
        serializer.Serialize(writer.BaseStream, ConnectionTypesName);
        writer.Close();
    }

    public static void Serialize(object item, string path1,string path2, List<Type> extraTypes, List<string> DataTypesName,  List<string> NodeTypesName, List<string> ImpTypesName,  List<string> ConnectionPointTypesName)
    {

        // 儲存資料本體
        XmlSerializer serializer = new XmlSerializer(item.GetType(), extraTypes.ToArray());
        StreamWriter writer = new StreamWriter(path1);
        serializer.Serialize(writer.BaseStream, item);
  
        // 儲存 Data Type String
        serializer = new XmlSerializer(DataTypesName.GetType());
        writer = new StreamWriter(path2+"DataType.xml");
        serializer.Serialize(writer.BaseStream, DataTypesName);

        // 儲存 Node Type String
        serializer = new XmlSerializer(NodeTypesName.GetType());
        writer = new StreamWriter(path2 + "NodeType.xml");
        serializer.Serialize(writer.BaseStream, NodeTypesName);

        // 儲存 ConnectionPoint Type String
        serializer = new XmlSerializer(ConnectionPointTypesName.GetType());
        writer = new StreamWriter(path2 + "ConnectionPointType.xml");
        serializer.Serialize(writer.BaseStream, ConnectionPointTypesName);

        // 儲存 Imp Type String
        serializer = new XmlSerializer(ImpTypesName.GetType());
        writer = new StreamWriter(path2 + "ImpType.xml");
        serializer.Serialize(writer.BaseStream, ImpTypesName);
        writer.Close();
    }

    public static T Deserialize<T>(string path,string path2)
    {
        // 讀取Type資訊
        XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
        StreamReader reader = new StreamReader(path2 + "DataType.xml");
        List<string> typesName = (List<string>)serializer.Deserialize(reader.BaseStream);
        reader.Close();
        // 依據string對比存入extraTypes中
        List<Type> extraTypes;
        extraTypes = new List<Type>();
        for (int i = 0; typesName!= null && i < typesName.Count; i++)
        {
            extraTypes.Add(Type.GetType(typesName[i]));
        }

        reader = new StreamReader(path2+ "NodeType.xml");
        typesName = (List<string>)serializer.Deserialize(reader.BaseStream);
        for (int i = 0; typesName!=null && i < typesName.Count; i++)
        {
            extraTypes.Add(Type.GetType(typesName[i]));
        }


        reader = new StreamReader(path2 + "ConnectionPointType.xml");
        typesName = (List<string>)serializer.Deserialize(reader.BaseStream);
        for (int i = 0; typesName != null && i < typesName.Count; i++)
        {
            extraTypes.Add(Type.GetType(typesName[i]));
        }


        reader = new StreamReader(path2 + "ConnectionType.xml");
        typesName = (List<string>)serializer.Deserialize(reader.BaseStream);
        for (int i = 0; typesName != null && i < typesName.Count; i++)
        {
            extraTypes.Add(Type.GetType(typesName[i]));
        }


        reader = new StreamReader(path2 + "ImpType.xml");
        typesName = (List<string>)serializer.Deserialize(reader.BaseStream);
        for (int i = 0; typesName != null && i < typesName.Count; i++)
        {
            extraTypes.Add(Type.GetType(typesName[i]));
        }


        // 
        if (extraTypes.Count > 0)
        {
            for (int k = 0; k < extraTypes.Count; k++)
            {
                //Debug.Log(extraTypes[k]);
            }
            serializer = new XmlSerializer(typeof(T), extraTypes.ToArray());

        }
        else
            serializer = new XmlSerializer(typeof(T));

        reader = new StreamReader(path);
        T item = (T)serializer.Deserialize(reader.BaseStream);
        reader.Close();

        return item;
    }
}

