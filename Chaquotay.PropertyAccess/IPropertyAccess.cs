namespace Chaquotay.PropertyAccess
{
    public interface IPropertyAccess
    {
        object GetValue(object target);
        void SetValue(object target, object value);
    }
}