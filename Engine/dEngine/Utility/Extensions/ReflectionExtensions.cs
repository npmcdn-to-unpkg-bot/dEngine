// ReflectionExtensions.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Dynamitey;

namespace dEngine.Utility.Extensions
{
    internal static class ReflectionExtensions
    {
        private static readonly Dictionary<Type, Func<object>> _constructorCache = new Dictionary<Type, Func<object>>();

        public static object FastConstruct(this Type type)
        {
            Func<object> constructor;
            if (!_constructorCache.TryGetValue(type, out constructor))
            {
                var lambda = Expression.Lambda<Func<object>>(Expression.New(type.GetConstructor(Type.EmptyTypes)));
                constructor = lambda.Compile();
                _constructorCache[type] = constructor;
            }
            return constructor.FastDynamicInvoke();
        }

        public static Expression<Func<object>> GetCacheableMethod(this Type type, string name, BindingFlags flags)
        {
            var method = type.GetMethod(name, flags);
            return () => method.Invoke(null, new object[] {});
        }

        public static Expression<Func<object, object>> GetValueGetter(this PropertyInfo propertyInfo)
        {
            var getter = propertyInfo.GetGetMethod(true);
            return obj => getter.Invoke(obj, new object[] {});
        }

        public static Expression<Action<object, object>> GetValueSetter(this PropertyInfo propertyInfo)
        {
            var setter = propertyInfo.GetSetMethod(true);
            return (obj, val) => setter.Invoke(obj, new[] {val});
        }

        public static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}