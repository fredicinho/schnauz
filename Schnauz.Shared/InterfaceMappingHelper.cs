
namespace Schnauz.Shared;

public static class InterfaceMappingHelper
{
    public static IEnumerable<(Type iFace, Type implementation)> MappingByConventionBasedOnInterface(Type interfaceType, Type? pivot = null)
    {
        return TypesFromAssembly(pivot ?? interfaceType)
            .Where(o => IsAssignableToGenericType(o, interfaceType)).Select(o => new
            {
                Type = o,
                iFace = o.GetInterfaces().Single(x => IsAssignableToGenericType(x, interfaceType))
            }).Select(o => (o.iFace, o.Type));
    }

    private static IEnumerable<Type> TypesFromAssembly(params Type[] pivots)
        => pivots.Select(o => o.Assembly).SelectMany(a => a.GetExportedTypes()).Where(x => !x.IsAbstract && x.IsClass);

    private static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        if (!genericType.IsGenericType)
        {
            return genericType.IsAssignableFrom(givenType);
        }

        var interfaceTypes = givenType.GetInterfaces();

        if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
        {
            return true;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        var baseType = givenType.BaseType;
        return baseType != null && IsAssignableToGenericType(baseType, genericType);
    }
}
