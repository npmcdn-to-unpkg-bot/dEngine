// Extensions.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Assimp;
using NLog;

namespace dEngine.Utility.Extensions
{
    internal static class Extensions
    {
        private const long OneKb = 1024;
        private const long OneMb = OneKb*1024;
        private const long OneGb = OneMb*1024;
        private const long OneTb = OneGb*1024;

        public static string ToPrettySize(this double value, int decimalPlaces = 0)
        {
            return ((long)value).ToPrettySize(decimalPlaces);
        }

        public static string ToPrettySize(this int value, int decimalPlaces = 0)
        {
            return ((long)value).ToPrettySize(decimalPlaces);
        }

        public static string ToPrettySize(this long value, int decimalPlaces = 0)
        {
            var asTb = Math.Round((double)value/OneTb, decimalPlaces);
            var asGb = Math.Round((double)value/OneGb, decimalPlaces);
            var asMb = Math.Round((double)value/OneMb, decimalPlaces);
            var asKb = Math.Round((double)value/OneKb, decimalPlaces);
            var chosenValue = asTb > 1
                ? $"{asTb}TB"
                : asGb > 1
                    ? $"{asGb}GB"
                    : asMb > 1
                        ? $"{asMb}MB"
                        : asKb > 1
                            ? $"{asKb}KB"
                            : $"{Math.Round((double)value, decimalPlaces)} bytes";
            return chosenValue;
        }

        public static string UppercaseFirst(this string s)
        {
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// Copies data from a non-<see cref="MemoryStream" /> stream to an array of bytes.
        /// </summary>
        public static byte[] ToArray(this Stream stream)
        {
            var bytes = new byte[(int)stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            return bytes;
        }

        public static Vector2[] GetTexCoords(this Mesh mesh)
        {
            var uvs = new Vector2[mesh.VertexCount];

            for (var i = 0; i < mesh.VertexCount; i++)
                if (i >= mesh.TextureCoordinateChannels[0].Count)
                    uvs[i] = Vector2.Zero;
                else
                    uvs[i] = new Vector2(mesh.TextureCoordinateChannels[0][i].X, mesh.TextureCoordinateChannels[0][i].Y);

            return uvs;
        }

        public static string ReadString(this Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static string GetDigits(this string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }

        public static void ErrorPlus(this Logger logger, Exception exception, string message = null)
        {
            if (message != null)
                logger.Error(message);

            logger.Error(exception.Message);

            if (exception.StackTrace != null)
                foreach (var item in exception.StackTrace.Split(new[] {"\r\n", "\n"}, 0))
                    logger.Trace(item);

            if (exception.InnerException != null)
                ErrorPlus(logger, exception.InnerException);
        }

        public static Type GetMemberType(this MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo)
                return ((FieldInfo)memberInfo).FieldType;
            if (memberInfo is PropertyInfo)
                return ((PropertyInfo)memberInfo).PropertyType;
            return ((MethodInfo)memberInfo).ReturnType;
        }

        public static object GetValue(this MemberInfo memberInfo, object obj)
        {
            if (memberInfo is FieldInfo)
                return ((FieldInfo)memberInfo).GetValue(obj);
            if (memberInfo is PropertyInfo)
                return ((PropertyInfo)memberInfo).GetValue(obj, null);
            return null;
        }

        public static void SetValue(this MemberInfo memberInfo, object obj, object value)
        {
            if (memberInfo is FieldInfo)
                ((FieldInfo)memberInfo).SetValue(obj, value);
            else
                ((PropertyInfo)memberInfo).SetValue(obj, value);
        }

        private static IEnumerable<Type> GetTypesSafely(this Assembly assembly, Predicate<Type> predicate)
        {
            try
            {
                return assembly.GetTypes().Where(x => predicate(x));
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(x => (x != null) && predicate(x));
            }
        }

        /// <summary>
        /// Returns all descendant types.
        /// </summary>
        public static IEnumerable<Type> GetDescendantTypes(this Type type)
        {
            return Assembly.GetExecutingAssembly().GetTypesSafely(type.IsAssignableFrom);
        }

        /// <summary>
        /// Returns direct descendant types.
        /// </summary>
        public static IEnumerable<Type> GetDirectDescendantTypes(this Type baseType)
        {
            return Assembly.GetExecutingAssembly().GetTypesSafely(t => t.BaseType == baseType);
        }
    }
}