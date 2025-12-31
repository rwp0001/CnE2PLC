using CnE2PLC.Helpers;
using System.Xml;

namespace CnE2PLC.PLC;

public class Task
{
    public Task() { }
    public Task(XmlNode node)
    {
        try
        {
            int n;

            Name = node.GetNamedAttributeItemInnerText("Name");
            Type = node.GetNamedAttributeItemInnerText("Type");
            Description = node.GetNamedAttributeItemInnerText("Description");

            int.TryParse(node.GetNamedAttributeItemInnerText("Rate"), out n);
            Rate = n;

            int.TryParse(node.GetNamedAttributeItemInnerText("Priority"), out n);
            Priority = n;

            int.TryParse(node.GetNamedAttributeItemInnerText("Watchdog"), out n);
            Watchdog = n;

            // get all the program names
            XmlNode sched = node.SelectSingleNode("ScheduledPrograms", XMLHelper.CreateGenericXmlNode());
            foreach (XmlNode p_node in sched.ChildNodes)
            {
                string it = p_node.GetNamedAttributeItemInnerText("Name");
                if (it.Length > 0) ScheduledPrograms.Add(it);
            }

            LogHelper.DebugPrint($"INFO: Created task: {ToString()}");

        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: Task: Import node {node.Name} failed with {ex.Message}");
        }
    }

    public string? Name { get; set; }
    public string? Type { get; set; }
    public int? Rate { get; set; }
    public int? Priority { get; set; }
    public int? Watchdog { get; set; }
    public bool? DisableUpdateOutputs { get; set; }
    public bool? InhibitTask { get; set; }
    public string? Description { get; set; }
    public List<string> ScheduledPrograms { get; set; } = new();

    public override string ToString() { return $"{Name} {Description} Scheduled Programs: {ScheduledPrograms.Count}"; }

}
