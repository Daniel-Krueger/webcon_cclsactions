using System;
using WebCon.WorkFlow.SDK.ActionPlugins;
using WebCon.WorkFlow.SDK.ActionPlugins.Model;
using WebCon.WorkFlow.SDK.Documents.Model.ItemLists;

namespace CCLSActions
{
    public class AddRowToItemList : CustomAction<AddRowToItemListConfig>
    {
        readonly IndentTextLogger logger = new IndentTextLogger();

        public override void Run(RunCustomActionParams args)
        {
            try
            {
                logger.Log($"Fetching document with id '{this.Configuration.TargetConfiguration.InstanceId}'");
                var document = WebCon.WorkFlow.SDK.Documents.DocumentsManager.GetDocumentByID(int.Parse(this.Configuration.TargetConfiguration.InstanceId), true);

                var list = document.ItemsLists.GetByID(int.Parse(Configuration.TargetConfiguration.TargetItemListId));
                logger.Log($"Creating a new row in item list {list.DisplayName}");
                logger.Indent();
                var newRow = list.Rows.AddNewRow();
                foreach (var mapping in Configuration.SourceConfiguration.Mapping)
                {
                    var column = list.Columns.GetByDbColumnName(mapping.SourceColName);
                    object targetValue = null;
                    if (!string.IsNullOrEmpty(mapping.Value))
                    {
                        var type = (TargetValueType)int.Parse(mapping.Type);
                        switch (type)
                        {
                            case TargetValueType.Boolean:
                                targetValue = bool.Parse(mapping.Value);
                                break;
                            case TargetValueType.Choose:
                                targetValue = mapping.Value;
                                break;
                            case TargetValueType.DateTime:
                                targetValue = mapping.Value;
                                break;
                            case TargetValueType.Decimal:
                                targetValue = decimal.Parse(mapping.Value);
                                break;
                            case TargetValueType.Text:
                                targetValue = mapping.Value;
                                break;
                            case TargetValueType.Picker:
                                targetValue = mapping.Value;
                                break;
                            default:
                                throw new ApplicationException($"An unknown '{nameof(TargetValueType)}' with id '{mapping.Type}' has been defined.");
                        }
                    }
                    logger.Log($"Adding value '{(targetValue == null ? "NULL" : targetValue)}' for column '{column.DisplayName}'");
                    newRow.Cells.GetByDbColumnName(mapping.SourceColName).SetValue(targetValue);
                }
                WebCon.WorkFlow.SDK.Documents.DocumentsManager.UpdateDocument(new WebCon.WorkFlow.SDK.Documents.Model.UpdateDocumentParams(document));
                logger.Outdent();
            }
            catch (System.Exception ex)
            {
                logger.Indent();
                logger.Log($"Error executing {nameof(AddRowToItemList)}", ex,args.Context.CurrentWorkflowID);
                // HasErrors property is responsible for detection whether action has been executed properly or not. When set to "true"
                // whole path transition will be marked as faulted and all the actions on it will be rollbacked. User will be notified
                // about failure by display of error window.
                args.HasErrors = true;
                // Message property is responsible for error message content.
                args.Message = ex.Message;
            }
            finally
            {
                args.LogMessage = logger.ToString();
                logger.Dispose();
            }

        }
    }
}