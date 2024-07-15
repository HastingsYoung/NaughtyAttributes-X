namespace NaughtyAttributes
{
    public static class ReflectionHelper
    {
        public static T CastTo<T>(object obj)
        {
            return (T)obj;
        }
    }
}