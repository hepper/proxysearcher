using System;
using System.Collections.Generic;

namespace ProxySearch.Console.Code.Detectable
{
    public abstract class DetectableBase<InterfaceType, ImplementationType, PropertyPageType> : SimpleDetectableBase<InterfaceType, ImplementationType>
    {
        public DetectableBase(string friendlyName, string description, int order, List<object> defaultSettings)
            : base(friendlyName, description, order)
        {
            this.defaultSettings = defaultSettings;
        }

        public sealed override Type PropertyPage
        {
            get
            {
                return typeof(PropertyPageType);
            }
        }

        private List<object> defaultSettings;
        public sealed override List<object> DefaultSettings
        {
            get
            {
                return defaultSettings;
            }            
        }
    }
}
