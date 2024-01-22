namespace DeviceServer.Api.Common.Utils;

public static class Executor
{
    public static void Try(Action action, Action<Exception> onError)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            onError(ex.InnerException ?? ex);
        }
    }

    public static async Task TryAsync(Func<Task> action, Action<Exception> onError)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            onError(ex.InnerException ?? ex);
        }
    }

    public static async Task<TResult> TryAsync<TResult>(Func<Task<TResult>> action, Func<Exception, TResult> onError)
    {
        TResult result;

        try
        {
            result = await action();
        }
        catch (Exception ex)
        {
            result = onError(ex.InnerException ?? ex);
        }

        return result;
    }
}
