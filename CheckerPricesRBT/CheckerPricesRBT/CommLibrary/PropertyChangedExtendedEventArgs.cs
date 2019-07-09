using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerPricesRBT.CommLibrary
{
    public class PropertyChangedExtendedEventArgs : PropertyChangedEventArgs
    {
        public object Value { get; }
        public PropertyChangedExtendedEventArgs(string propertyName, object value)
            : base(propertyName)
            => Value = value;
    }
}
