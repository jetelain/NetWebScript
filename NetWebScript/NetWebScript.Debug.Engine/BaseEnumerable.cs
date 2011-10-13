using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NetWebScript.Debug.Engine
{
    abstract class BaseEnumerable<T, I> where I : class
    {
        readonly T[] m_data;
        uint m_position;

        public BaseEnumerable(T[] data)
        {
            m_data = data;
            m_position = 0;
        }

        public int Clone(out I ppEnum)
        {
            ppEnum = null;
            return Constants.E_NOTIMPL;
        }

        public int GetCount(out uint pcelt)
        {
            pcelt = (uint)m_data.Length;
            return Constants.S_OK;
        }

        public int Next(uint celt, T[] rgelt, out uint celtFetched)
        {
            return Move(celt, rgelt, out celtFetched);
        }

        public int Reset()
        {
            lock (this)
            {
                m_position = 0;

                return Constants.S_OK;
            }
        }

        public int Skip(uint celt)
        {
            uint celtFetched;

            return Move(celt, null, out celtFetched);
        }

        private int Move(uint celt, T[] rgelt, out uint celtFetched)
        {
            lock (this)
            {
                int hr = Constants.S_OK;
                celtFetched = (uint)m_data.Length - m_position;

                if (celt > celtFetched)
                {
                    hr = Constants.S_FALSE;
                }
                else if (celt < celtFetched)
                {
                    celtFetched = celt;
                }

                if (rgelt != null)
                {
                    for (int c = 0; c < celtFetched; c++)
                    {
                        rgelt[c] = m_data[m_position + c];
                    }
                }

                m_position += celtFetched;

                return hr;
            }
        }
    }
}
