using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Helpers
{
    public static class CRUDHelper
    {
        public static void CopyNoneNull(object source, object target)
        {
            var dtoProps = source.GetType().GetProperties();
            var actualProps = target.GetType().GetProperties();
            var sameProperties = dtoProps.Where(x => actualProps.Select(a => a.Name).Contains(x.Name)).ToDictionary(x => x, x => actualProps.First(p => p.Name == x.Name));
            foreach (var (dtoProp, actualProp) in sameProperties)
            {
                var val = dtoProp.GetValue(source);
                if (val != null) actualProp.SetValue(target, val);
            }
        }
    }
}