namespace NStore.Shared.Constants
{
    public static class ErrorMessageConstants
    {
        public static string UnexpectedErrorMessage => "An error occurred. Please try again later.";

        public static string DatabaseInitializationErrorMessage => "An error occurred while initialising the database";

        public static string DatabaseSeedingErrorMessage => "An error occurred while seeding the database";

        public static string InvalidDeleteOperationErrorMessage => "Delete operation cannot be performed";
    }
}
