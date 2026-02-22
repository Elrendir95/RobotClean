using System;

namespace Library.References
{
    [Serializable]
    public class IntReference : BaseReference<int>
    {
        public IntReference(int i) : base(i) {}
    }
}
