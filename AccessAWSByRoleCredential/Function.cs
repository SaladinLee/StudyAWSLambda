using Amazon.Lambda.Core;
using Amazon.OpenSearchService;
using Amazon.OpenSearchService.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AccessAWSByRoleCredential;

/// <summary>
/// 驗證如何從環境變數或profile取得角色認證的範例
/// </summary>
public class Function
{
    /// <summary>
    /// 驗證如何從環境變數或profile取得角色認證
    /// </summary>
    /// <param name="context">Lambda 執行環境</param>
    /// <returns>取得DOMAIN個數</returns>
    public string FunctionHandler(ILambdaContext context)
    {
        // 取得 access key
        string? accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");

        // 取得 secret key
        string? secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

        // 取得 token (本次未用到)
        string? token = Environment.GetEnvironmentVariable("AWS_SESSION_TOKEN");

        // 取得所在區域 (本次未用到)
        string? region = Environment.GetEnvironmentVariable("AWS_REGION");

        AWSCredentials credential;
        int domainCounter = 0;

        // 若無法從環境變數取得資料，則改從 profile 取得認證資料
        if (string.IsNullOrEmpty(accessKey))
        {
            CredentialProfileStoreChain chain = new CredentialProfileStoreChain();
            List<CredentialProfile> profiles = chain.ListProfiles();
            foreach (CredentialProfile profile in profiles)
            {
                if (profile.Name.Contains("playground"))
                {
                    credential = profile.GetAWSCredentials(profile.CredentialProfileStore);
                    domainCounter = GetAllDomainNamesCount(credential);
                    break;
                }
            }
        }
        else
        {
            credential = new BasicAWSCredentials(accessKey, secretKey);
            domainCounter = GetAllDomainNamesCount(credential);
        }

        return $"we have {domainCounter} domain name";
    }

    /// <summary>
    /// 取得 OPENSEARCH 的 DOMAIN 數量
    /// </summary>
    /// <param name="credential">aws 憑證</param>
    /// <returns>DOMAIN 數量</returns>
    private static int GetAllDomainNamesCount(AWSCredentials credential)
    {
        // 建立連線設定
        AmazonOpenSearchServiceConfig config
            = new AmazonOpenSearchServiceConfig()
            {
                RegionEndpoint = Amazon.RegionEndpoint.APNortheast1
            };

        // 建立連線到 AWS opensearch 的 client 物件
        AmazonOpenSearchServiceClient client
            = new AmazonOpenSearchServiceClient(credential, config);

        ListDomainNamesRequest request = new ListDomainNamesRequest();
        ListDomainNamesResponse response = client.ListDomainNamesAsync(request).Result;
        if (response == null)
        {
            return 0;
        }

        return response.DomainNames.Count;
    }
}