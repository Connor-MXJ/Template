﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Domain.Persistence.Generator
{
    /// <summary>
    /// 属性集合
    /// </summary>
   public class PropertyContainer
    {
          private readonly Dictionary<string, object> _ids;
            private readonly Dictionary<string, object> _values;

            #region 属性

            public IEnumerable<string> IdNames
            {
                get { return _ids.Keys; }
            }

            public IEnumerable<string> ValueNames
            {
                get { return _values.Keys; }
            }

            public IEnumerable<string> AllNames
            {
                get { return _ids.Keys.Union(_values.Keys); }
            }

            public IDictionary<string, object> IdPairs
            {
                get { return _ids; }
            }

            public IDictionary<string, object> ValuePairs
            {
                get { return _values; }
            }

            public IEnumerable<KeyValuePair<string, object>> AllPairs
            {
                get { return _ids.Concat(_values); }
            }

            #endregion

            #region Constructor

            public PropertyContainer()
            {
                _ids = new Dictionary<string, object>();
                _values = new Dictionary<string, object>();
            }

            #endregion

            #region Methods

            public void AddId(string name, object value)
            {
                _ids.Add(name, value);
            }

            public void AddValue(string name, object value)
            {
                _values.Add(name, value);
            }

            #endregion
        
    }
}
