namespace VideoEditor;

public readonly record struct BooleanResponse(bool Success, string ResponseMsg)
{
    public static readonly BooleanResponse Successful = 
        new BooleanResponse(true, "Operation successful.");
}