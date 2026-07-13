namespace Tests
{
    public abstract class BaseTest
    {
        protected void SetPrivateValue<T>(Task obj, string propertyName, object value)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName, System.Reflection.BindingFlags.NonPublic);
            propertyInfo.SetValue(obj, value);
        }
    }
}
