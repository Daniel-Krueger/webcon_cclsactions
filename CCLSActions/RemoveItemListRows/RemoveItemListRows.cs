using System;
using System.Collections.Generic;
using System.Data;
using WebCon.WorkFlow.SDK.ActionPlugins;
using WebCon.WorkFlow.SDK.ActionPlugins.Model;

namespace CCLSActions.RemoveItemListRows
{
    public class RemoveItemListRows : CustomAction<RemoveItemListRowsConfig>
    {
        readonly IndentTextLogger logger = new IndentTextLogger();

        public override void Run(RunCustomActionParams args)
        {
            try
            {
                if (string.IsNullOrEmpty(Configuration.SQLCommand))
                {
                    throw new ApplicationException("No sql command has been provided");
                }

                if (string.IsNullOrEmpty(Configuration.ItemListId))
                {
                    throw new ApplicationException("No item list has been selected.");
                }

                if (!int.TryParse(Configuration.ItemListId, out int itemListid))
                {
                    throw new ApplicationException($"Value '{Configuration.ItemListId}' could not be converted to item.");
                }
                if (string.IsNullOrEmpty(Configuration.ColumnName))
                {
                    throw new ApplicationException("No column name has been defined");
                }

                if (!args.Context.CurrentDocument.ItemsLists.TryGetByID(itemListid, out WebCon.WorkFlow.SDK.Documents.Model.ItemLists.ItemsList itemList))
                {
                    throw new ApplicationException($"There's no item list with id '{itemListid}'");
                }
                logger.Log("Going to execute the SQL command");
                DataTable result;
                try
                {
                    result = WebCon.WorkFlow.SDK.Tools.Data.SqlExecutionHelper.GetDataTableForSqlCommand(Configuration.SQLCommand, args.Context);
                   
                }
                catch ( Exception ex)
                {
                    logger.Log($"Could not execute SQL command '{Configuration.SQLCommand}'");
                    throw;
                }
                
                if (!result.Columns.Contains(Configuration.ColumnName))
                {
                    throw new ApplicationException($"The result of the sql command '{Configuration.SQLCommand}' did not contain a column with the name '{Configuration.ColumnName}'");
                }

                var existingRows = new Dictionary<int, WebCon.WorkFlow.SDK.Documents.Model.ItemLists.ItemRowData>();

                foreach (var row in itemList.Rows)
                {
                    existingRows.Add(row.ID, row);
                }

                logger.Log($"Going to remove '{result.Rows.Count}' item list rows");
                logger.Indent();
                foreach (DataRow row in result.Rows)
                {
                    var detId = (int)row[Configuration.ColumnName];
                    logger.Log($"Going to remove item list row with id '{detId}'");
                    itemList.Rows.Remove(existingRows[detId]);
                }
                logger.Outdent();
                logger.Log("Removed all item list rows.");
            }

            catch (System.Exception ex)
            {
                logger.Indent();
                logger.Log($"Error executing {nameof(AddRowToItemList)}", ex, args.Context.CurrentWorkflowID);
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