// PluginItemBase.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Caliburn.Micro;
using dEngine.Instances;
using Action = System.Action;

namespace dEditor.Framework.Plugins
{
    public class PluginItemBase : Instance, INotifyPropertyChangedEx
    {
        /// <summary>
        /// Creates an instance of <see cref="T:Caliburn.Micro.PropertyChangedBase" />.
        /// </summary>
        public PluginItemBase()
        {
            IsNotifying = true;
        }

        /// <summary>
        /// Enables/Disables property change notification.
        /// Virtualized in order to help with document oriented view models.
        /// </summary>
        public virtual bool IsNotifying { get; set; }

        /// <summary>Occurs when a property value changes.</summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises a change notification indicating that all bindings should be refreshed.
        /// </summary>
        public virtual void Refresh()
        {
            NotifyOfPropertyChange(string.Empty);
        }

        /// <summary>Notifies subscribers of the property change.</summary>
        /// <param name="propertyName">Name of the property.</param>
        public virtual void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
        {
            // ISSUE: reference to a compiler-generated field
            if (!IsNotifying || (PropertyChanged == null))
                return;
            ((Action)(() => OnPropertyChanged(new PropertyChangedEventArgs(propertyName)))).OnUIThread();
        }

        /// <summary>Notifies subscribers of the property change.</summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The property expression.</param>
        public void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property)
        {
            NotifyOfPropertyChange(property.GetMemberInfo().Name);
        }

        /// <summary>
        /// Raises the <see cref="E:Caliburn.Micro.PropertyChangedBase.PropertyChanged" /> event directly.
        /// </summary>
        /// <param name="e">The <see cref="T:System.ComponentModel.PropertyChangedEventArgs" /> instance containing the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var changedEventHandler = PropertyChanged;
            changedEventHandler?.Invoke(this, e);
        }
    }
}