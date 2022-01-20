using System.Collections.Generic;
using WebCon.WorkFlow.SDK.Common;
using WebCon.WorkFlow.SDK.ConfigAttributes;

namespace CCLSActions
{
    public class AddRowToItemListConfig : PluginConfiguration
    {
        [ConfigGroupBox(DisplayName = "Target configuration")]
        public TargetConfiguration TargetConfiguration { get; set; }

        [ConfigGroupBox(DisplayName = "Source configuration")]
        public SourceConfiguration SourceConfiguration { get; set; }
    }

    public class SourceConfiguration
    {
        [ConfigEditableGrid(DisplayName = "New values")]
        public List<Mapping> Mapping { get; set; }
    }

    public class Mapping
    {
        [ConfigEditableGridColumn(DisplayName = "Target column", Description = "DB name of target item list column.", IsRequired = true)]
        public string SourceColName { get; set; }

        [ConfigEditableGridColumn(DisplayName = "Target value", Description = "The value which will be written to the column.")]
        public string Value { get; set; }

        [ConfigEditableGridColumn(DisplayName = "Target value type", Description = "The target type of the value.<br/><ul><li>Boolean = 0</li><li>Choose = 10</li><li>DateTime = 20</li><li>Decimal = 30</li><li>Text = 40</li><li>Picker = 50</li></ul>", IsRequired = true, DescriptionAsHTML = true)]
        public string Type { get; set; }


    }

    public class TargetConfiguration
    {
        [ConfigEditableText(DisplayName = "Instance Id", IsRequired = true)]
        public string InstanceId { get; set; }
        [ConfigEditableText(DisplayName = "Target item list id")]
        public string TargetItemListId { get; set; }
    }

    public enum TargetValueType
    {
        Boolean = 0,
        Choose = 10,
        DateTime = 20,
        Decimal = 30,
        Text = 40,
        Picker = 50,
    }
}