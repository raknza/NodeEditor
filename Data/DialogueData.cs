using System;

public class DialogueData : BaseData
{
    public string speaker;
    public string body;

    public DialogueData() : base()
    {
    }

    public DialogueData(string speaker,string body) : base()
    {
        this.speaker = speaker;
        this.body = body;
    }

    public override BaseData copy()
    {
        return new DialogueData(speaker, body);
    }

    public override bool equals(BaseData data)
    {
        DialogueData anotherDialogue = (DialogueData)data;
        if (anotherDialogue == null)
            throw new Exception("wrong data type");
        return anotherDialogue.body.Equals(body);
    }

    public override Type getType()
    {
        return typeof(DialogueData); 
    }
}
