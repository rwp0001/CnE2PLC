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
            Name = node.GetNamedAttributeItemInnerText("Name");
            Type = Enum.Parse<TaskTypes>(node.GetNamedAttributeItemInnerText("Type"));
            
            var descNode = node.SelectSingleNode("Description");
            Description = descNode?.InnerText ?? string.Empty;

            Rate = node.GetNamedAttributeItemInnerTextAsInt("Rate");
            Priority = node.GetNamedAttributeItemInnerTextAsInt("Priority") ?? 0;
            Watchdog = node.GetNamedAttributeItemInnerTextAsInt("Watchdog") ?? 0;

            DisableUpdateOutputs = node.GetNamedAttributeItemInnerTextAsBool("DisableUpdateOutputs");
            InhibitTask = node.GetNamedAttributeItemInnerTextAsBool("InhibitTask") ?? false;

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

    public string Name { get; set; }
    public TaskTypes Type { get; set; }
    public int? Rate { get; set; }
    public int Priority { get; set; }
    public int Watchdog { get; set; }
    public bool? DisableUpdateOutputs { get; set; }
    public bool InhibitTask { get; set; }
    public string? Description { get; set; }
    public List<string> ScheduledPrograms { get; set; } = new();

    public override string ToString() { return $"{Name} {Description} Scheduled Programs: {ScheduledPrograms.Count}"; }

}

public class EventInfo {     
    public EventInfo() { }
    public EventInfo(XmlNode node)
    {
        try
        {
            EventTrigger = node.GetNamedAttributeItemInnerText("EventTrigger");
            EnableTimeout = node.GetNamedAttributeItemInnerTextAsBool("EnableTimeout") ?? false;
            LogHelper.DebugPrint($"INFO: Created event info: {ToString()}");
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: EventInfo: Import node {node.Name} failed with {ex.Message}");
        }
    }
    public string EventTrigger { get; set; } = string.Empty;
    public bool EnableTimeout { get; set; } = false;
    public override string ToString() { return $"{EventTrigger}"; }
}
