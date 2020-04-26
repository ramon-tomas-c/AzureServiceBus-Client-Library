namespace SB.WebAPI
{
    /// <summary>
    /// API constants
    /// </summary>
    public static class ApiConstants
    {
        public static class Messages
        {
            public const string ModelStateValidation = "Please refer to the errors property for additional details.";
        }

        public static class ContentTypes
        {
            public const string ProblemJson = "application/problem+json";
            public const string ProblemXml = "application/problem+xml";
        }
    }
}
