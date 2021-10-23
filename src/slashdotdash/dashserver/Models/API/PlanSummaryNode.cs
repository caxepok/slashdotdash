using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashserver.Models.API
{
    /// <summary>
    /// Узел плана для отображения в сводной таблице
    /// </summary>
    public class PlanSummaryNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasWarning => Warning != null;
        public string Warning { get; set; }
        public List<ValueOnDate> Values { get; set; }
        public List<PlanSummaryNode> Childs { get; set; }

        public PlanSummaryNode(int id, string name, string warning, List<ValueOnDate> values)
        {
            Id = id;
            Name = name;
            Warning = warning;
            Values = values;
        }
    }
}
