// EditorBase.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Generic;
using System.Linq;
using dEditor.Framework;
using dEngine;
using dEngine.Serializer.V1;
using dEngine.Services;
using dEngine.Utility;

namespace dEditor.Widgets.Properties.Inspectors
{
    /// <summary>
    /// Base class for 'inspectors' - controls that represent value editors.
    /// </summary>
    public abstract class EditorBase<TValue> : InspectorBase, IEditor
    {
        protected EditorBase(object obj, Inst.CachedProperty propDesc)
        {
            Objects = new Dictionary<object, BoundPropertyInfo>();

            FirstObject = new KeyValuePair<object, BoundPropertyInfo>(obj, AddObject(obj, propDesc));

            DisplayName = FirstObject.Value.DisplayName;

            var instance = obj as dEngine.Instances.Instance;

            if (instance != null)
                instance.Changed.Event += propertyName =>
                {
                    if (propertyName == propDesc.Name)
                        NotifyOfPropertyChange(() => Value);
                };
        }

        public bool EnableHistory { get; protected set; } = true;

        public dynamic Value
        {
            get
            {
                if (!AreMultipleValuesSame) return default(TValue);
                var value = FirstObject.Value.Value;
                return value;
            }
            set
            {
                var action = new SetPropertyAction(Objects.Values, value);
                if (PropertiesWidget.UseHistoryService && EnableHistory)
                {
                    HistoryService.ExecuteAction(action);
                    HistoryService.Waypoint(FirstObject.Value.DisplayName);
                    //NotifyOfPropertyChange(() => Value);
                }
                else
                {
                    action.Execute();
                }
            }
        }

        /// <summary>
        /// Gets whether or not all objects share the same value.
        /// </summary>
        public bool AreMultipleValuesSame
        {
            get { return Objects.Values.All(x => Equals(x.Value, FirstObject.Value.Value)); }
        }

        public KeyValuePair<object, BoundPropertyInfo> FirstObject { get; }

        public override string Name => DisplayName;

        public PropertiesViewModel PropertiesWidget { get; set; }

        public string DisplayName { get; set; }

        public override bool IsReadOnly => FirstObject.Value.IsReadOnly;

        public override string Description
        {
            get
            {
                var desc = Name;
                var prop = FirstObject.Value.Property;
                Comments.Comment comment;
                API.Comments.Get($"P:{prop.DeclaringType}.{prop.Name}", out comment);
                desc = comment.Summary;
                return desc;
            }
        }

        public Dictionary<object, BoundPropertyInfo> Objects { get; }

        public BoundPropertyInfo AddObject(object obj, Inst.CachedProperty property)
        {
            var bound = new BoundPropertyInfo(obj, property);
            Objects.Add(obj, bound);
            return bound;
        }

        public void RemoveObject(object obj)
        {
            BoundPropertyInfo descriptor;

            Objects.TryGetValue(obj, out descriptor);
            Objects.Remove(obj);
        }

        public void ApplyValueWithHistory()
        {
            EnableHistory = true;
            Value = Value;
            EnableHistory = false;
        }

        public int CompareTo(IEditor other)
        {
            return string.CompareOrdinal(DisplayName, other.DisplayName);
        }

        private class SetPropertyAction : HistoryService.HistoryAction
        {
            private readonly object _newValue;
            private readonly List<object> _originalValues;
            private readonly IEnumerable<BoundPropertyInfo> _props;

            public SetPropertyAction(IEnumerable<BoundPropertyInfo> props, object value)
            {
                _originalValues = new List<object>(16);
                foreach (var prop in props)
                    _originalValues.Add(prop.Value);

                _newValue = value;
                _props = props;
            }

            public override void Undo()
            {
                var i = 0;
                foreach (var prop in _props)
                    prop.Value = _originalValues[i++];
            }

            public override void Execute()
            {
                foreach (var prop in _props)
                    prop.Value = _newValue;
            }
        }
    }
}