using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elie.Tools
{
    [System.Serializable]
    public class Buffer<T>
    {
        public readonly int size;

        [SerializeField]private T[] _values;
        public T[] values => _values;
        public bool isFull => index == size;


        private int index;

        public Buffer(int _size)
        {
            _values = new T[_size];
            size = _size;
            index = 0;
        }

        public void Add(T _value)
        {
            if(index == 0)
            {
                _values[index] = _value;
                index++;
            }

            else if (!isFull)
            {
                _values[index] = _values[index - 1];

                for (int i = index - 1; i > 0; i--)
                {
                    _values[i] = _values[i - 1];
                }

                index++;
            }

            else
            {
                for (int i = size - 1; i > 0; i--)
                {
                    _values[i] = _values[i - 1];
                }
            }

            _values[0] = _value;
        }
    }
}
