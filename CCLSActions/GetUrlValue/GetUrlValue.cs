using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using WebCon.WorkFlow.SDK.BusinessRulePlugins;
using WebCon.WorkFlow.SDK.BusinessRulePlugins.Model;

namespace CCLSActions
{
    public class GetQueryParameter : CustomBusinessRule<GetUrlValueConfig>
    {
        public GetQueryParameter()
        {
        }
        readonly StringBuilder _logger = new StringBuilder();
        public override EvaluationResult Evaluate(CustomBusinessRuleParams args)
        {
            HttpRequest request = (new HttpContextAccessor()).HttpContext.Request;
            try
            {
                Uri uri = null;
                switch (this.Configuration.Url)
                {
                    case UrlToUse.RequestUrl:
                        uri = new Uri(Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(request));
                        break;
                    case UrlToUse.RefererUrl:
                        uri = new Uri(request.Headers["Referer"]);
                        break;
                    case UrlToUse.DesignerStudioTest:
                        uri = new Uri(this.Configuration.TestUrl);
                        break;
                    default:
                        throw new ApplicationException($"An unsported {nameof(UrlToUse)} has been set.");
                }
                string returnValue = null;
                switch (this.Configuration.UrlPart)
                {
                    case UrlPart.AbsoluteUri:
                        returnValue = uri.AbsoluteUri;
                        break;
                    case UrlPart.AbsoluteUriWithoutPath:
                        returnValue = uri.AbsoluteUri.Substring(0, uri.AbsoluteUri.Length - uri.PathAndQuery.Length);
                        break;
                    case UrlPart.Authority:
                        returnValue = uri.Authority;
                        break;
                    case UrlPart.Host:
                        returnValue = uri.Host;
                        break;
                    case UrlPart.LocalPath:
                        returnValue = uri.LocalPath;
                        break;
                    case UrlPart.PathAndQuery:
                        returnValue = uri.PathAndQuery;
                        break;
                    case UrlPart.QueryParameter:
                        var parameters = System.Web.HttpUtility.ParseQueryString(uri.Query);
                        if (string.IsNullOrEmpty(this.Configuration.Value))
                        {
                            throw new ApplicationException("A query parameter value should be returned but the name of the parameter was not provided");
                        }
                        returnValue = parameters[this.Configuration.Value];
                        break;
                    case UrlPart.RegExMatch:
                        if (string.IsNullOrEmpty(this.Configuration.Value))
                        {
                            throw new ApplicationException("A RegEx should be applied to the url but no RegEx was defined.");
                        }
                        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(this.Configuration.Value);
                        var result = regex.Match(uri.AbsoluteUri); 
                        if (result.Success)
                        {
                            returnValue = result.Groups[1].Value;                            
                        }
                        break;
                    default:
                        throw new ApplicationException($"An unsported {nameof(UrlPart)} has been set.");
                }
                var test = new Microsoft.AspNetCore.Http.Extensions.QueryBuilder();

                _logger.AppendLine($"Used url:'{uri.AbsoluteUri}', result: {returnValue}");
                if (returnValue != null)
                {
                    return EvaluationResult.CreateStringResult(this.Configuration.EncodeValue ? Uri.EscapeDataString(returnValue) : returnValue);
                }
                return EvaluationResult.CreateStringResult(null);
            }
            catch (System.Exception ex)
            {
                _logger.AppendLine(ex.ToString());
                throw ex;
            }
            finally
            {
                args.Context.PluginLogger.AppendInfo(_logger.ToString());
            }
            
        }
    }
}