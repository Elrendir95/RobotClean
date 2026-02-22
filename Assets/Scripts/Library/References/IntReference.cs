using System;

[Serializable]
public class IntReference : BaseReference<int>
{
    public IntReference(int i) : base(i) {}
}
