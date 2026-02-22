using System;
using Library.Variables;

namespace Library.References
{
    [Serializable]
    public class BaseReference<Type>
    {
        public bool UseConstante = true;
        public Type ConstanteValue;
        public BaseVariable<Type> variable;

        public  BaseReference()
        {}

        public BaseReference(Type value)
        {
            UseConstante = true;
            ConstanteValue = value;
        }

        public Type Value
        {
            get { return UseConstante ? ConstanteValue : variable.Value; }
            set
            {
                if (UseConstante) ConstanteValue = value;
                else variable.Value = value;
            }
        }

        public static implicit operator Type(BaseReference<Type> reference)
        {
            return reference.Value;
        }
    }
}
