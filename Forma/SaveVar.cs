using System;

[AttributeUsage(AttributeTargets.Field)]
public class SaveVar : Attribute
{
    public string FileName;
    public SaveVar(string FileName)
    {
        this.FileName = FileName;
    }
}
