using System.Linq;
using System.Collections.ObjectModel;

namespace DataObjects
{
    public class Property<T> 
    {
        public Property()
        {
          
        }

        public ObservableCollection<string> Errors { get; private set; } = new ObservableCollection<string>();

        public bool IsDirty
        {
            get
            {
                if (Value == null && Original == null)
                    return false;
                else if (Value == null && Original != null)
                    return true;
                else
                    return !Value.Equals(Original);
            }
        }

        public bool IsValid => !Errors.Any();

        public bool IsEnabled { get; set; } = default(bool);

        readonly T _original = default(T);
        public T Original
        {
            get => _original;
            set => _valueHasBeenSet = true;
        }

        private bool _valueHasBeenSet;

        readonly T _value = default(T);
        public T Value
        {
            get => _value;
            set
            {
                if (!_valueHasBeenSet)
                    Original = value;
       
            }
        }

        public void Revert()
        {
            Value = Original;
        }

        public override string ToString()
        {
            if (Value == null)
                return string.Empty;
            return Value.ToString();
        }

        public static Property<T> Make(T value)
        {
            return new Property<T>()
            {
                Value = value,
                Original = value
            };
        }

    }
}
