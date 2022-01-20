using WebCon.WorkFlow.SDK.Common;
using WebCon.WorkFlow.SDK.ConfigAttributes;

namespace CCLSActions
{
    public class GetUrlValueConfig : PluginConfiguration
    {
        public GetUrlValueConfig()
        {
        }

        [ConfigEditableEnum(DefaultValue = 0, DisplayName = "Url to use", Description = "Defines which url should be used. For example if the PATH_ID parameter is used in a call a redirection will occure. So we need to use the referer in this case.")]
        public UrlToUse Url { get; set; }

        [ConfigEditableEnum(DefaultValue = 1, DisplayName = "Url part to return", Description = "If QueryParameter or RegExMatch are used a value must be provided.")]
        public UrlPart UrlPart { get; set; }

        [ConfigEditableText(DisplayName = "The value", Description = "<ul>    <li>Query Parameter<br/>         The parameter itself is case insensitive. <br/>         If the parameter does not exist the value will be null, otherwise the value will be decoded.          If the parameter exists multiple times the values a comma separated 'value1,value2'    </li>    <li>RegEx<br/>        The value from the first group will be returned<br/>        RegEx: /app/(\\d*)/<br/>        Url: https://example.local/db/1/app/123/element/234/form?someQuery=parameter&someQuery=parameter2&another=one<br/>        Return: 123<br/>    </li></ul>", DescriptionAsHTML = true)]
        public string Value { get; set; }

        [ConfigEditableBool(DisplayName = "Encode return value", Description = "This is usefull, if you want to pass the returned value as an url parameter.")]
        public bool EncodeValue { get; set; }

        [ConfigEditableText(DisplayName = "Test url", Description = "If you want to test your configuration, set the Url to use to 'DesignerStudio' and provide a value.", DefaultText = "https://example.local/db/1/app/123/element/234/form?someQuery=parameter&someQuery=parameter2&another=one")]
        public string TestUrl { get; set; }
    }

    /// <summary>
    /// Defines which url should be used. If we use the PATH_ID parameter we need to use the Referer
    /// </summary>
    public enum UrlToUse
    {
        /// <summary>
        /// The url of the request will be used.
        /// </summary>
        RequestUrl = 0,

        /// <summary>
        /// The url of the referer header will be used.
        /// </summary>
        RefererUrl = 1,

        /// <summary>
        /// Testing
        /// </summary>
        DesignerStudioTest = 2,

    }


    /// <summary>
    /// Returns a part of the url
    /// The examples below use this url https://example.local:433/db/1/app/8/element/578/form?path_id=123&test=23"
    public enum UrlPart
    {
        /// <summary>
        /// Returns "https://example.local:433/db/1/app/8/element/578/form?path_id=123&test=23"
        /// </summary>
        AbsoluteUri = 0,

        /// <summary>
        /// Returns "https://example.local:433"
        /// </summary>
        AbsoluteUriWithoutPath = 10,

        /// <summary>
        /// Returns example.local:433"
        /// </summary>
        Authority = 20,

        /// <summary>
        /// Returns example.local"
        /// </summary>
        Host = 30,

        /// <summary>
        /// Returns "/db/1/app/8/element/578/form"
        /// </summary>
        LocalPath = 40,

        /// <summary>
        /// Returns "/db/1/app/8/element/578/form?path_id=123&test=23"
        /// </summary>
        PathAndQuery = 50,

        /// <summary>
        /// Returns the value of a query parameter
        /// </summary>
        QueryParameter = 60,

        /// <summary>
        /// Returns a value which matched the regex
        /// </summary>
        RegExMatch = 70,
    }
}