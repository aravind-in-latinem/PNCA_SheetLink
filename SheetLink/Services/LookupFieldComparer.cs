using System.Collections.Generic;
using Autodesk.Revit.DB;
using PNCA_SheetLink.SheetLink.Model;

public class LookupFieldComparer : IEqualityComparer<LookupField>
{
    public bool Equals(LookupField x, LookupField y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (x is null || y is null)
            return false;

        int xParamId = x.ParameterElement?.Id?.IntegerValue ?? -1;
        int yParamId = y.ParameterElement?.Id?.IntegerValue ?? -1;

        return xParamId == yParamId
               && x.FieldName == y.FieldName
               && x.ParameterType == y.ParameterType;
    }

    public int GetHashCode(LookupField obj)
    {
        unchecked
        {
            int hash = 17;

            int paramId = obj.ParameterElement?.Id?.IntegerValue ?? -1;

            hash = hash * 23 + paramId.GetHashCode();
            hash = hash * 23 + (obj.FieldName?.GetHashCode() ?? 0);
            hash = hash * 23 + (obj.ParameterType?.GetHashCode() ?? 0);

            return hash;
        }
    }
}