// MemberEntry.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Diagnostics;
using System.Reflection;

namespace dEditor.Widgets.ObjectBrowser
{
    public class MemberEntry : Entry
    {
        protected MemberEntry()
        {
        }

        public MemberEntry(MemberInfo member)
        {
            Name = member.Name;
            Icon = new Uri("/dEditor;component/Content/Icons/Toolbar/Member_16xMD.png", UriKind.Relative);

            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    var fieldType = ((FieldInfo)member).FieldType;
                    if (fieldType.Name.StartsWith("Signal"))
                        Icon = new Uri("/dEditor;component/Content/Icons/Toolbar/Event_orange_16x.png", UriKind.Relative);
                    break;
                case MemberTypes.Method:
                    Icon = new Uri("/dEditor;component/Content/Icons/Toolbar/Method_purple_16x.png", UriKind.Relative);
                    break;
                case MemberTypes.Property:
                    Icon = new Uri("/dEditor;component/Content/Icons/Toolbar/properties_16xMD.png", UriKind.Relative);
                    break;
            }
        }

        private static bool IsAssignableFrom(Type extendType, Type baseType)
        {
            while (!baseType.IsAssignableFrom(extendType))
            {
                if (extendType == typeof(object))
                    return false;
                Debug.Assert(extendType != null, "extendType != null");
                if (extendType.IsGenericType && !extendType.IsGenericTypeDefinition)
                    extendType = extendType.GetGenericTypeDefinition();
                else
                    extendType = extendType.BaseType;
            }
            return true;
        }
    }
}