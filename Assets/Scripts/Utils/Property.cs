using System;
using System.Collections.Generic;
using UnityEngine;

public class Property<T>
{
    class Act<TT>
    {
        public bool CallEvenIfDisabled = false;
        public MonoBehaviour Mb;
        public bool HasMb;

        public Action<TT> Changed = null;
        public Action<TT, TT> ChangedWithPrev = null;
    }

    List<Act<T>> Callbacks = new List<Act<T>>();

    T currentValue;

    public Property() { }

    public Property(T defaultValue)
    {
        currentValue = defaultValue;
    }

    public void AddEvent(Action<T> onChanged, MonoBehaviour mb, bool callEvenIfDisabled = false)
    {
        Callbacks.Add(new Act<T>()
        {
            Mb = mb,
            HasMb = mb != null,
            Changed = onChanged,
            CallEvenIfDisabled = callEvenIfDisabled,
        });
    }

    public void AddEvent(Action<T, T> onChanged, MonoBehaviour mb, bool callEvenIfDisabled = false)
    {
        Callbacks.Add(new Act<T>()
        {
            Mb = mb,
            HasMb = mb != null,
            ChangedWithPrev = onChanged,
            CallEvenIfDisabled = callEvenIfDisabled,
        });
    }

    public void AddEventAndFire(Action<T> onChanged, MonoBehaviour mb, bool callEvenIfDisabled = false)
    {
        AddEvent(onChanged, mb, callEvenIfDisabled);
        onChanged(currentValue);
    }

    public void AddEventAndFire(Action<T, T> onChanged, MonoBehaviour mb, bool callEvenIfDisabled = false)
    {
        AddEvent(onChanged, mb, callEvenIfDisabled);
        onChanged(currentValue, currentValue);
    }

    public void RemoveEvent(Action<T> onChanged)
    {
        Callbacks.RemoveAll(el => el.Changed == onChanged);
    }

    public void RemoveEvent(Action<T, T> onChanged)
    {
        Callbacks.RemoveAll(el => el.ChangedWithPrev == onChanged);
    }

    public void RemoveEvent(MonoBehaviour mb)
    {
        Callbacks.RemoveAll(el => el.Mb == mb);
    }

    public void RemoveAllEvents()
    {
        Callbacks.Clear();
    }

    public void Fire() { ChangeValue(currentValue); }
    public void Fire(MonoBehaviour mb) { ChangeValue(currentValue, mb); }
    public void Fire(T newValue) { Value = newValue; }

    public virtual T Value
    {
        get { return currentValue; }
        set { ChangeValue(value); }
    }

    void ChangeValue(T value, MonoBehaviour mb = null)
    {
        var oldValue = currentValue;
        currentValue = value;

        Callbacks.RemoveAll(el =>
        {
            try
            {
                // Here comes the magic: if monoBehaviour has been already removed we'll have null here
                if (el.HasMb && el.Mb == null)
                    return true;

                if (!el.HasMb || (el.Mb.gameObject.activeInHierarchy && el.Mb.enabled) || el.CallEvenIfDisabled)
                    if (mb == null || el.Mb == mb)
                    {
                        if (el.Changed != null)
                            el.Changed(currentValue);
                        if (el.ChangedWithPrev != null)
                            el.ChangedWithPrev(currentValue, oldValue);
                    }
                return false;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
                return false;
            }
        });
    }
}
