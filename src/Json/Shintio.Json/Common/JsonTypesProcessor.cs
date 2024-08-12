using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using Shintio.Json.Attributes;

namespace Shintio.Json.Common
{
    public class JsonTypesProcessor<TLibraryConverter>
    {
        private readonly Type _converterAttributeType = typeof(JsonConverterAttribute);
        private readonly Type _shintioConverterClassType = typeof(JsonConverter<>);
        private readonly Type _genericConverterType;

        private readonly Action<TLibraryConverter> _addConverter;

        // Concurrent HashSet alternative
        private readonly ConcurrentDictionary<Type, byte> _typesCache = new ConcurrentDictionary<Type, byte>();
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public JsonTypesProcessor(Type genericConverterType, Action<TLibraryConverter> addConverter)
        {
            _genericConverterType = genericConverterType;
            _addConverter = addConverter;
        }

        public void TryProcessType(Type type)
        {
            if (!_typesCache.ContainsKey(type))
            {
                _lock.EnterUpgradeableReadLock();

                try
                {
                    ProcessType(type);
                }
                finally
                {
                    _lock.ExitUpgradeableReadLock();
                }
            }
        }

        private void ProcessType(Type type)
        {
            _lock.EnterWriteLock();

            try
            {
                if (_typesCache.ContainsKey(type))
                {
                    return;
                }

                if (type.GetCustomAttribute(_converterAttributeType) is Attributes.JsonConverterAttribute attribute)
                {
                    var shintioConverterGenericArguments =
                        FindBaseType(attribute.ConverterType, _shintioConverterClassType)?.GetGenericArguments();
                    if (shintioConverterGenericArguments == null || shintioConverterGenericArguments.Length <= 0)
                    {
                        return;
                    }

                    var baseConverter = Activator.CreateInstance(attribute.ConverterType);
                    if (baseConverter == null)
                    {
                        return;
                    }

                    var converterType =
                        _genericConverterType.MakeGenericType(shintioConverterGenericArguments[0]);
                    if (Activator.CreateInstance(converterType, baseConverter) is TLibraryConverter converter)
                    {
                        _addConverter(converter);
                    }
                }

                _typesCache.TryAdd(type, 0);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        private static Type? FindBaseType(Type? targetType, Type baseType)
        {
            while (targetType != null && targetType != typeof(object))
            {
                if (
                    targetType == baseType ||
                    (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == baseType)
                )
                {
                    return targetType;
                }

                targetType = targetType.BaseType;
            }

            return null;
        }
    }
}