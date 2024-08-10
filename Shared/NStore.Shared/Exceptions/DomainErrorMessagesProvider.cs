namespace NStore.Shared.Exceptions
{
    public static class DomainErrorMessagesProvider
    {
        public static string ProperyRequiredErrorMessage(string objectName, string propertyName) => 
            $"{objectName} {propertyName} is required";

        public static string LessOrEqualToErrorMessage(string objectName, string propertyName, string value) => 
            $"{objectName} {propertyName} cannot be less or equal to {value}";

        public static string LessThanErrorMessage(string objectName, string propertyName, string value) =>
            $"{objectName} {propertyName} cannot be less than {value}";

        public static string AlreadyExistsInObjectErrorMessage(string objectName, string propertyName, string identifier, string destObjectName) =>
            $"{objectName} with {propertyName}: {identifier} already exists in the {destObjectName}";

        public static string UnsupportedEnumerationErrorMessage(string objectName) =>
            $"{objectName} is not supported";

        public static string InvalidUUIDErrorMessage() =>
            $"Invalid Id is provided. UUID is required";

        public static string TextIsLongerThanMaxLengthErrorMessage(string objectName, string propertyName, int maxLength) =>
            $"{objectName} property {propertyName} value is longer than allowed max length: {maxLength}";

        public static string ItemUnresolvedInsertOperationErrorMessage() => "Item addition operation was not successful";
    }
}
