namespace BatchValidatorMockTest.Helpers
{
    public class BatchValidator
    {
        public static ValidationResult ValidateJsonLog(BatchResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (response.ExitCode == 0)
            {
                return new ValidationResult
                {
                    IsSuccess = true,
                    Message = "Batch executed successfully",
                    Output = response.Output
                };
            }
            else
            {
                return new ValidationResult
                {
                    IsSuccess = false,
                    Message = response.Error ?? "Batch execution failed",
                    Output = response.Output
                };
            }
        }        

        public static ValidationResult ValidateTextLog(BatchResponse response)
        {
            return response == null
                ? throw new ArgumentNullException(nameof(response))
                : new ValidationResult
                {
                    Output = response.Output
                };
        }
    }
}
