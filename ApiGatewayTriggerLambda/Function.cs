using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ApiGatewayTriggerLambda;

/// <summary>
/// 透過 APIGateway 呼叫 Lambda 的範例
/// </summary>
public class Function
{
    /// <summary>
    /// 透過 APIGateway,把request.body轉成小寫英文
    /// </summary>
    /// <param name="request">透過APIGateway 傳入資料</param>
    /// <param name="context">lambda 執行環境</param>
    /// <returns>回傳給 APIGateway 的處理結果</returns>
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        context.Logger.LogDebug("記錄DEBUG");
        context.Logger.LogTrace("記錄Trace");
        context.Logger.LogInformation("記錄Information");
        context.Logger.LogWarning("記錄Warning");
        context.Logger.LogCritical("記錄Critical");
        return new APIGatewayProxyResponse
        { 
            StatusCode = (int)HttpStatusCode.OK,
            Body = request.Body.ToLower(),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
