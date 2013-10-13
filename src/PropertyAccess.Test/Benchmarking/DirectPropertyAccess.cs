using PropertyAccess.Test.Support;

namespace PropertyAccess.Test.Benchmarking
{
    internal class DirectPropertyAccess : IClassPropertyAccess
    {
        public object GetValue(object target)
        {
            return ((TestTarget) target).Value;
        }

        public void SetValue(object target, object value)
        {
            ((TestTarget) target).Value = (int) value;
        }
    }
}