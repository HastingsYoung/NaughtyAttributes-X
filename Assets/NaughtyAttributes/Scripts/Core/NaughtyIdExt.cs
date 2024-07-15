namespace NaughtyAttributes
{
    using System.Reflection;
    using NaughtyAttributes;

    public static class NaughtyIdExt
    {
        public static DropdownList<string> GetConstIdsString<T>() where T : INaughtyDropdownId<string>
        {
            var type = typeof(T);
            var list = new DropdownList<string>();
            var fieldInfos = type.GetFields(
                // Gets all public and static fields
                BindingFlags.Public | BindingFlags.Static |
                // This tells it to get the fields from all base types as well
                BindingFlags.FlattenHierarchy);

            // Go through the list and only pick out the constants
            foreach (var fi in fieldInfos)
                if (fi.IsLiteral && !fi.IsInitOnly)
                    list.Add(fi.Name, (string)fi.GetRawConstantValue());
            return list;
        }

        public static DropdownList<int> GetConstIdsInt<T>() where T : INaughtyDropdownId<int>
        {
            var type = typeof(T);
            var list = new DropdownList<int>();
            var fieldInfos = type.GetFields(
                // Gets all public and static fields
                BindingFlags.Public | BindingFlags.Static |
                // This tells it to get the fields from all base types as well
                BindingFlags.FlattenHierarchy);

            // Go through the list and only pick out the constants
            foreach (var fi in fieldInfos)
                if (fi.IsLiteral && !fi.IsInitOnly)
                    list.Add(fi.Name, (int)fi.GetRawConstantValue());
            return list;
        }
    }

    public interface INaughtyDropdownId<T>
    {
    }
}