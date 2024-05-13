namespace API.Wrapper
{
    public interface IResult
    {
        string Messages
        {
            get; set;
        }

        bool Succeeded
        {
            get; set;
        }
    }

    public interface IResult<out T> : IResult
    {
        T Data
        {
            get;
        }
    }
}