using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DataObjects;

namespace Abstractions
{
    public interface IChangeTracking
    {
        Property<DateTime> LMDT { get; set; }

        Property<int?> LMUID { get; set; }

        Property<DateTime?> CRDT { get; set; }

        Property<int?> CRUID { get; set; }

        Property<DateTime?> DLDT { get; set; }

        Property<int?> DLUID { get; set; }
    }


    public interface IRecord
    {
        Property<int> ID { get; set; }
        Property<bool> Deleted { get; set; }
    }

    public interface IProperty : INotifyPropertyChanged
    {
        ObservableCollection<string> Errors { get; }
        bool IsValid { get; }
        bool IsDirty { get; }
        void Revert();
    }
}