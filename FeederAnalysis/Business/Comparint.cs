using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FeederAnalysis.Business
{
    public class Comparint<T> : IEqualityComparer<T> where T : class, new()
    {
        private string[] comparintFiledName = new string[0];

        public Comparint()
        {
        }

        public Comparint(params string[] comparintFiledName) => this.comparintFiledName = comparintFiledName;

        bool IEqualityComparer<T>.Equals(T x, T y)
        {
            if ((object)x == null && (object)y == null)
                return false;
            if (this.comparintFiledName.Length == 0)
                return x.Equals((object)y);
            bool flag = true;
            Type type1 = x.GetType();
            Type type2 = y.GetType();
            foreach (string str in this.comparintFiledName)
            {
                string filedName = str;
                PropertyInfo propertyInfo1 = ((IEnumerable<PropertyInfo>)type1.GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>)(p => p.Name.Equals(filedName))).FirstOrDefault<PropertyInfo>();
                PropertyInfo propertyInfo2 = ((IEnumerable<PropertyInfo>)type2.GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>)(p => p.Name.Equals(filedName))).FirstOrDefault<PropertyInfo>();
                flag = flag && propertyInfo1 != (PropertyInfo)null && propertyInfo2 != (PropertyInfo)null && propertyInfo1.GetValue((object)x, (object[])null).ToString().Equals(propertyInfo2.GetValue((object)y, (object[])null));
            }
            return flag;
        }

        int IEqualityComparer<T>.GetHashCode(T obj) => obj.ToString().GetHashCode();
    }
}
