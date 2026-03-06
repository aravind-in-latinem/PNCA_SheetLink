using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace PNCA_SheetLink.SheetLink.Model
{
    public class ScheduledElement

    {
        public ElementId RowElementId { get; set; }
        public List<ScheduledField> ScheduledFields { get; set; } = new List<ScheduledField>();

    }

}
