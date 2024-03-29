namespace MinimalAPIsMovies.Validations
{
    public static class ValidationUtilities
    {
        public static string NonEmptyMessage = "The field {PropertyName} is required";
        public static string MaximumLengthMessage = "The field {PropertyName} should be less than {MaxLength} characters";
        public static string FirstLetterIsUpperCaseMessage = "The field {PropertyName} should start with uppercase";
        public static string EmailAddressMessage = "The field {PropertyName} is not a valid email address";

        public static string GreaterThanDate(DateTime value) => "The field {PropertyName} should be greater than " + value.ToString("yyyy-MM-dd");

        public static bool FirstLetterIsUppercase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            var firstLetter = value[0].ToString();
            return firstLetter == firstLetter.ToUpper();
        }
    }
}
