using System.Reflection;
using AutoMapper;

namespace Tiktok.API.Application.Common.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingFromAssembly(Assembly assembly)
    {
        var mapFromType = typeof(IMapFrom<>);
        const string mappingMethodName = nameof(IMapFrom<object>.Mapping);
        // bool HasInterface(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == mapFromType;
        Func<Type, bool> hasInterface = type => type.IsGenericType && type.GetGenericTypeDefinition() == mapFromType;
        var types = assembly.GetExportedTypes().Where(type => type.GetInterfaces().Any(hasInterface));
        var argumentTypes = new[] { typeof(Profile) };

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod(mappingMethodName);
            if (methodInfo != null)
            {
                methodInfo.Invoke(instance, new object[] { this });
            }
            else
            {
                var interfaces = type.GetInterfaces().Where(hasInterface).ToList();

                if (interfaces.Count <= 0) continue;

                foreach (var @interface in interfaces)
                {
                    var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);
                    interfaceMethodInfo?.Invoke(instance, new object[] { this });
                }
            }
        }
    }
}