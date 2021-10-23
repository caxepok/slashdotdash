using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dashserver.Models.API
{
    /// <summary>
    /// Узел плана для отображения в сводной таблице
    /// </summary>
    public class PlanCompareNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ValueOnDateWithCompare> Values { get; set; }
        public List<PlanCompareNode> Childs { get; set; }

        public PlanCompareNode(int id, string name, List<ValueOnDateWithCompare> values)
        {
            Id = id;
            Name = name;
            Values = values;
        }
    }
}
