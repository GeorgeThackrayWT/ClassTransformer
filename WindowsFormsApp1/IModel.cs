using System;
using System.Collections.ObjectModel;

namespace DataObjects
{
    public interface IModel
    {
        ObservableCollection<string> Errors { get; }
        Action<IModel> Validator { get; set; }
        bool IsValid { get; }
        bool IsDirty { get; }
        bool Validate();
        void Revert();
    }
}