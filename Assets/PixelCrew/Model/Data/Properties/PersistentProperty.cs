﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Data.Properties
{
    [Serializable]
    public abstract class PersistentProperty<TPropertyType>
    {
        [SerializeField] protected TPropertyType _value;
        protected TPropertyType _stored;
        private TPropertyType _defaultValue;

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);

        public event OnPropertyChanged OnChanged;

        public IDisposable Subscribe(OnPropertyChanged call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public PersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public TPropertyType Value
        {
            get => _stored;
            set
            {
                var isEqual = _stored.Equals(value);
                if (isEqual) return;

                var oldValue = _stored;
                Write(value);
                _stored = _value = value;

                OnChanged?.Invoke(value, oldValue);
            }
        }

        protected void Init()
        {
            _stored = _value = Read(_defaultValue);
        }

        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);

        public void Validate()
        {
            if (!_stored.Equals(_value))
                Value = _value;
        }
    }
}
