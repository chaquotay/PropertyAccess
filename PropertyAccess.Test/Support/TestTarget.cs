namespace PropertyAccess.Test.Support
{
    public class TestTarget
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
