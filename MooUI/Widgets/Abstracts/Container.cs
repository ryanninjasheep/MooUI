using System;
using System.Collections.Generic;
using System.Text;

namespace MooUI.Widgets
{
    /// <summary>
    /// DO NOT INHERIT - Use Delegator, MonoContainer, or MultiContainer instead!
    /// </summary>
    public abstract class Container : MooWidget
    {
        /// <summary>
        /// Making the constructor internal protects this class from being inherited directly
        /// </summary>
        internal Container(int width, int height) : base(width, height) { }
    }
}
