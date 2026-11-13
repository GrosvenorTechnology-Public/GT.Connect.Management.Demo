namespace GT.Connect.Management.Demo.ConnectApi.Core;

public class PagingResponse<T>
{
    public int Count { get; set; }
    public IEnumerable<T> Data { get; set; }

    public PagingResponse()
    {
        Count = 0;
        Data = [];
    }

    public PagingResponse(int count, IEnumerable<T> data)
    {
        Count = count;
        Data = data;
    }

    public void Deconstruct(out int count, out IEnumerable<T> data)
    {
        count = Count;
        data = Data;
    }
}
