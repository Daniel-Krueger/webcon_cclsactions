using System;
using WebCon.WorkFlow.SDK.ActionPlugins;
using WebCon.WorkFlow.SDK.ActionPlugins.Model;

namespace CCLSActions.UpdateDocumentType
{
    public class UpdateDocumentType : CustomAction<UpdateDocumentTypeConfig>
    {
        public override void Run(RunCustomActionParams args)
        {            
            if (!int.TryParse(this.Configuration.FormTypeIdAsString,out int formTypeId))
            {
                throw new ApplicationException($"Value '{this.Configuration.FormTypeIdAsString}' could not be parsed to integer.");
            }
            args.Context.CurrentDocument.UpdateDocumentType(formTypeId);
        }
    }
}