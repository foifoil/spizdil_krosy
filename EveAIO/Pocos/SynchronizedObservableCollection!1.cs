namespace EveAIO.Pocos
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    [Serializable, ComVisible(false), DebuggerDisplay("Count = {Count}")]
    public class SynchronizedObservableCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IReadOnlyList<T>, IReadOnlyCollection<T>, IEnumerable, INotifyPropertyChanged, IDisposable, IList, ICollection, INotifyCollectionChanged
    {
        private readonly SynchronizationContext _context;
        private readonly IList<T> _items;
        private readonly ReaderWriterLockSlim _itemsLock;
        private readonly object _lock;
        [NonSerialized]
        private object _syncRoot;
        private readonly SimpleMonitor<T> _monitor;

        [field: CompilerGenerated]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        [field: CompilerGenerated]
        protected event PropertyChangedEventHandler PropertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                this.PropertyChanged += value;
            }
            remove
            {
                this.PropertyChanged -= value;
            }
        }

        public SynchronizedObservableCollection()
        {
            Class7.RIuqtBYzWxthF();
            this._itemsLock = new ReaderWriterLockSlim();
            this._lock = new object();
            this._monitor = new SimpleMonitor<T>();
            this._context = SynchronizationContext.Current;
            this._items = new List<T>();
        }

        public void Add(T item)
        {
            this._itemsLock.EnterWriteLock();
            int count = this._items.Count;
            try
            {
                this.CheckIsReadOnly();
                this.CheckReentrancy();
                this._items.Insert(count, item);
            }
            finally
            {
                this._itemsLock.ExitWriteLock();
            }
            this.OnPropertyChanged("Count");
            this.OnPropertyChanged("Item[]");
            this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item, count);
        }

        private IDisposable BlockReentrancy()
        {
            this._monitor.Enter();
            return this._monitor;
        }

        private void CheckIndex(int index)
        {
            if ((index < 0) || (index >= this._items.Count))
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckIsReadOnly()
        {
            if (this._items.IsReadOnly)
            {
                throw new NotSupportedException("Collection is readonly");
            }
        }

        private void CheckReentrancy()
        {
            if ((this._monitor.Busy && (this.CollectionChanged != null)) && (this.CollectionChanged.GetInvocationList().Length > 1))
            {
                throw new InvalidOperationException("SynchronizedObservableCollection reentrancy not allowed");
            }
        }

        public void Clear()
        {
            this._itemsLock.EnterWriteLock();
            try
            {
                this.CheckIsReadOnly();
                this.CheckReentrancy();
                this._items.Clear();
            }
            finally
            {
                this._itemsLock.ExitWriteLock();
            }
            this.OnPropertyChanged("Count");
            this.OnPropertyChanged("Item[]");
            this.OnCollectionReset();
        }

        public bool Contains(T item)
        {
            bool flag;
            this._itemsLock.EnterReadLock();
            try
            {
                flag = this._items.Contains(item);
            }
            finally
            {
                this._itemsLock.ExitReadLock();
            }
            return flag;
        }

        public void CopyTo(T[] array, int index)
        {
            this._itemsLock.EnterReadLock();
            try
            {
                this._items.CopyTo(array, index);
            }
            finally
            {
                this._itemsLock.ExitReadLock();
            }
        }

        public void Dispose()
        {
            this._itemsLock.Dispose();
        }

        public IEnumerator<T> GetEnumerator()
        {
            IEnumerator<T> enumerator;
            this._itemsLock.EnterReadLock();
            try
            {
                enumerator = this._items.ToList<T>().GetEnumerator();
            }
            finally
            {
                this._itemsLock.ExitReadLock();
            }
            return enumerator;
        }

        public int IndexOf(T item)
        {
            int index;
            this._itemsLock.EnterReadLock();
            try
            {
                index = this._items.IndexOf(item);
            }
            finally
            {
                this._itemsLock.ExitReadLock();
            }
            return index;
        }

        public void Insert(int index, T item)
        {
            this._itemsLock.EnterWriteLock();
            try
            {
                this.CheckIsReadOnly();
                this.CheckIndex(index);
                this.CheckReentrancy();
                this._items.Insert(index, item);
            }
            finally
            {
                this._itemsLock.ExitWriteLock();
            }
            this.OnPropertyChanged("Count");
            this.OnPropertyChanged("Item[]");
            this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        private static bool IsCompatibleObject(object value) => 
            ((value is T) || ((value == null) && (default(T) == null)));

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
            if (collectionChanged != null)
            {
                using (this.BlockReentrancy())
                {
                    this._context.Send(state => collectionChanged((SynchronizedObservableCollection<T>) this, e), null);
                }
            }
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }

        private void OnCollectionReset()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                this._context.Send(state => propertyChanged((SynchronizedObservableCollection<T>) this, e), null);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public bool Remove(T item)
        {
            bool flag;
            this._itemsLock.EnterUpgradeableReadLock();
            try
            {
                this.CheckIsReadOnly();
                int index = this._items.IndexOf(item);
                if (index >= 0)
                {
                    this._itemsLock.EnterWriteLock();
                    try
                    {
                        this.CheckReentrancy();
                        T local = this._items[index];
                        this._items.RemoveAt(index);
                        this.OnPropertyChanged("Count");
                        this.OnPropertyChanged("Item[]");
                        this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, local, index);
                        return true;
                    }
                    finally
                    {
                        this._itemsLock.ExitWriteLock();
                    }
                }
                flag = false;
            }
            finally
            {
                this._itemsLock.ExitUpgradeableReadLock();
            }
            return flag;
        }

        public void RemoveAt(int index)
        {
            T local;
            this._itemsLock.EnterWriteLock();
            try
            {
                this.CheckIsReadOnly();
                this.CheckIndex(index);
                this.CheckReentrancy();
                local = this._items[index];
                this._items.RemoveAt(index);
            }
            finally
            {
                this._itemsLock.ExitWriteLock();
            }
            this.OnPropertyChanged("Count");
            this.OnPropertyChanged("Item[]");
            this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, local, index);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this._itemsLock.EnterReadLock();
            try
            {
                T[] localArray;
                if (array == null)
                {
                    throw new ArgumentNullException("array", "'array' cannot be null");
                }
                if (array.Rank != 1)
                {
                    throw new ArgumentException("Multidimension arrays are not supported", "array");
                }
                if (array.GetLowerBound(0) == 0)
                {
                    if (index < 0)
                    {
                        throw new ArgumentOutOfRangeException("index", "'index' is out of range");
                    }
                    if ((array.Length - index) < this._items.Count)
                    {
                        throw new ArgumentException("Array is too small");
                    }
                    localArray = array as T[];
                    if (localArray != null)
                    {
                        goto Label_0135;
                    }
                    Type elementType = array.GetType().GetElementType();
                    Type c = typeof(T);
                    if (!elementType.IsAssignableFrom(c) && !c.IsAssignableFrom(elementType))
                    {
                        throw new ArrayTypeMismatchException("Invalid array type");
                    }
                    object[] objArray = array as object[];
                    if (objArray == null)
                    {
                        throw new ArrayTypeMismatchException("Invalid array type");
                    }
                    int count = this._items.Count;
                    try
                    {
                        for (int i = 0; i < count; i++)
                        {
                            objArray[index++] = this._items[i];
                        }
                        return;
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        throw new ArrayTypeMismatchException("Invalid array type");
                    }
                }
                throw new ArgumentException("Non-zero lower bound arrays are not supported", "array");
            Label_0135:
                this._items.CopyTo(localArray, index);
            }
            finally
            {
                this._itemsLock.ExitReadLock();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator enumerator;
            this._itemsLock.EnterReadLock();
            try
            {
                enumerator = this._items.ToList<T>().GetEnumerator();
            }
            finally
            {
                this._itemsLock.ExitReadLock();
            }
            return enumerator;
        }

        int IList.Add(object value)
        {
            T local;
            this._itemsLock.EnterWriteLock();
            int count = this._items.Count;
            try
            {
                this.CheckIsReadOnly();
                this.CheckReentrancy();
                local = (T) value;
                this._items.Insert(count, local);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("'value' is the wrong type");
            }
            finally
            {
                this._itemsLock.ExitWriteLock();
            }
            this.OnPropertyChanged("Count");
            this.OnPropertyChanged("Item[]");
            this.OnCollectionChanged(NotifyCollectionChangedAction.Add, local, count);
            return count;
        }

        bool IList.Contains(object value)
        {
            bool flag;
            if (!SynchronizedObservableCollection<T>.IsCompatibleObject(value))
            {
                return false;
            }
            this._itemsLock.EnterReadLock();
            try
            {
                flag = this._items.Contains((T) value);
            }
            finally
            {
                this._itemsLock.ExitReadLock();
            }
            return flag;
        }

        int IList.IndexOf(object value)
        {
            int index;
            if (!SynchronizedObservableCollection<T>.IsCompatibleObject(value))
            {
                return -1;
            }
            this._itemsLock.EnterReadLock();
            try
            {
                index = this._items.IndexOf((T) value);
            }
            finally
            {
                this._itemsLock.ExitReadLock();
            }
            return index;
        }

        void IList.Insert(int index, object value)
        {
            try
            {
                this.Insert(index, (T) value);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("'value' is the wrong type");
            }
        }

        void IList.Remove(object value)
        {
            if (SynchronizedObservableCollection<T>.IsCompatibleObject(value))
            {
                this.Remove((T) value);
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                IList list = this._items as IList;
                if (list == null)
                {
                    return this._items.IsReadOnly;
                }
                return list.IsFixedSize;
            }
        }

        bool ICollection<T>.IsReadOnly =>
            this._items.IsReadOnly;

        bool IList.IsReadOnly =>
            this._items.IsReadOnly;

        bool ICollection.IsSynchronized =>
            true;

        object ICollection.SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    this._itemsLock.EnterReadLock();
                    try
                    {
                        ICollection is2 = this._items as ICollection;
                        if (is2 != null)
                        {
                            this._syncRoot = is2.SyncRoot;
                        }
                        else
                        {
                            Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
                        }
                    }
                    finally
                    {
                        this._itemsLock.ExitReadLock();
                    }
                }
                return this._syncRoot;
            }
        }

        public int Count
        {
            get
            {
                int count;
                this._itemsLock.EnterReadLock();
                try
                {
                    count = this._items.Count;
                }
                finally
                {
                    this._itemsLock.ExitReadLock();
                }
                return count;
            }
        }

        public T this[int index]
        {
            get
            {
                T local;
                this._itemsLock.EnterReadLock();
                try
                {
                    this.CheckIndex(index);
                    local = this._items[index];
                }
                finally
                {
                    this._itemsLock.ExitReadLock();
                }
                return local;
            }
            set
            {
                T local;
                this._itemsLock.EnterWriteLock();
                try
                {
                    this.CheckIsReadOnly();
                    this.CheckIndex(index);
                    this.CheckReentrancy();
                    local = this[index];
                    this._items[index] = value;
                }
                finally
                {
                    this._itemsLock.ExitWriteLock();
                }
                this.OnPropertyChanged("Item[]");
                this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, local, value, index);
            }
        }

        object IList.this[int index]
        {
            get => 
                this[index];
            set
            {
                try
                {
                    this[index] = (T) value;
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException("'value' is the wrong type");
                }
            }
        }

        private class SimpleMonitor : IDisposable
        {
            private int _busyCount;

            public SimpleMonitor()
            {
                Class7.RIuqtBYzWxthF();
            }

            public void Dispose()
            {
                this._busyCount--;
            }

            public void Enter()
            {
                this._busyCount++;
            }

            public bool Busy =>
                (this._busyCount > 0);
        }
    }
}

