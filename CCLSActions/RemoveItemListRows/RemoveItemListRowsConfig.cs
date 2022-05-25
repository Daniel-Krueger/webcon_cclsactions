using WebCon.WorkFlow.SDK.Common;
using WebCon.WorkFlow.SDK.ConfigAttributes;

namespace CCLSActions.RemoveItemListRows
{
    public class RemoveItemListRowsConfig : PluginConfiguration
    {
        [ConfigEditableText(DisplayName = "Item list", Description = "The item list from which the rows should be deleted")]
        public string ItemListId { get; set; }

        [ConfigEditableText(DisplayName = "Column name", Description = "The name of the column which contains the DET_ID returned by the column name", IsRequired = true, Multiline = false,DefaultText ="DET_ID")]
        public string ColumnName{ get; set; }

        [ConfigEditableText(DisplayName = "SQL Command", Description = "This command will be executed to return the rows which should be deleted in the selected item list.<br/><pre><code>select DET_ID from WFElementDetails where DET_WFCONID = ITEMLISTID and DET_WFDID = INSTANCEID and .... </code></pre>", IsRequired = true, Multiline = true, DescriptionAsHTML = true)]
        public string SQLCommand { get; set; }
    }
}