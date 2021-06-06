using System.Collections.Generic;
using System.Dynamic;

namespace FuncResult
{
    public class ExternalProperty : DynamicObject
    {
        private readonly Dictionary<string, object> dictionary
            = new Dictionary<string, object>();

        public int Count
        {
            get { return dictionary.Count; }
        }


        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            var name = binder.Name.ToLower();
            return dictionary.TryGetValue(name, out result);
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            dictionary[binder.Name.ToLower()] = value;

            return true;
        }
    }
}