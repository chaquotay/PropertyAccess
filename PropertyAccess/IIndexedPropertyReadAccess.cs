namespace PropertyAccess
{
    public interface IIndexedPropertyReadAccess
    {
        object GetValue(object target, object index);
    }
}