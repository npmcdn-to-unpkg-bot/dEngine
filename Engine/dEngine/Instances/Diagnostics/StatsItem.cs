// StatsItem.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using dEngine.Services;

#pragma warning disable 1591

namespace dEngine.Instances.Diagnostics
{
    [TypeId(204)]
    [Uncreatable]
    public class StatsItem : Instance
    {
        protected double _value;
        protected string _valueString;

        public StatsItem()
        {
            Archivable = false;
            Parent = Game.Stats;
            ParentLocked = true;
        }

        internal StatsItem(string name, Instance parent)
        {
            Name = name;
            Parent = parent;
        }

        internal double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        internal string ValueString
        {
            get { return _valueString ?? _value.ToString(); }
            set { _valueString = value; }
        }

        [ScriptSecurity(ScriptIdentity.Plugin)]
        public double GetValue()
        {
            ScriptService.AssertIdentity(ScriptIdentity.Plugin);
            return Value;
        }

        [ScriptSecurity(ScriptIdentity.Plugin)]
        public string GetValueString()
        {
            ScriptService.AssertIdentity(ScriptIdentity.Plugin);
            return ValueString;
        }
    }
}