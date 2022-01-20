using WebCon.WorkFlow.SDK.Common;
using WebCon.WorkFlow.SDK.ConfigAttributes;

namespace CCLSActions.UpdateDocumentType
{
    public class UpdateDocumentTypeConfig : PluginConfiguration
    {
        [ConfigEditableText(DisplayName = "Form Type")]
        public string FormTypeIdAsString { get; set; }

    }
}