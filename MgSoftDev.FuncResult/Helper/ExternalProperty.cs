using System.Collections.Generic;
using System.Dynamic;

namespace FuncResult.Helper
{
    public class ExternalProperty : DynamicObject
    {
        private readonly Dictionary<string, object> _Dictionary
            = new Dictionary<string, object>();

        public int Count=>_Dictionary.Count;


        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            var name = binder.Name.ToLower();
            return _Dictionary.TryGetValue(name, out result);
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            _Dictionary[binder.Name.ToLower()] = value;

            return true;
        }
    }
}