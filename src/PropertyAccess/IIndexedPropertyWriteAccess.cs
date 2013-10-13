namespace PropertyAccess
{
    public interface IIndexedPropertyWriteAccess
    {
        void SetValue(object target, object index, object value);
    }
}