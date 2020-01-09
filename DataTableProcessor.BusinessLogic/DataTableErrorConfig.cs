namespace DataTableProcessorConfig{
public class ErrorConfig{
    public string ErrorMessageWhenColumnNotPresentKey{get; set;}
    public string ErrorMessageWhenColumnNotPresentValue{get; set;}=ErrorMessages.DefaultInvalidColumn;
    public int StartRowNumberForValidationError{get; set;} = 2;

}
}