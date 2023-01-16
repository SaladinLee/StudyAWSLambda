using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ReadEnvironmentValueInLambda;

/// <summary>
/// 驗證從環境變數取得設定資料的方法
/// </summary>
public class Function
{
    
    /// <summary>
    /// 驗證 Lambda 可從環境變數中讀取設定
    /// </summary>
    /// <param name="input">輸入資料</param>
    /// <param name="context">lambda 執行環境</param>
    /// <returns>輸出資料拼組後的結果</returns>
    public string FunctionHandler(string input, ILambdaContext context)
    {
        // 從環境變數鍵值為Test1的主鍵取得設定資料
        string? evnironmentValue = Environment.GetEnvironmentVariable("Test1");

        // 與input字串結合，回傳出去
        return $@"{input} and {evnironmentValue} fall in love!!";
    }
}
