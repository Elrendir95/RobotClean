using System;

namespace Library.References
{
    [Serializable]
    public class FloatReference : BaseReference<float>
    {
        public FloatReference(float i) : base(i) {}
    }
}
