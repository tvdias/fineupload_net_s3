namespace FineUploadS3.Services
{
    using System;
    using System.Text;
    using Amazon.Runtime;
    using Amazon.Runtime.Internal.Auth;
    using Amazon.Util;
    using Newtonsoft.Json.Linq;

    public class AwsS3Signer : IAwsS3Signer
    {
        public AwsS3Signer(string awsSecretAccessKey, string region)
        {
            this._awsSecretAccessKey = awsSecretAccessKey ?? throw new ArgumentNullException(nameof(awsSecretAccessKey));
            this._region = region ?? throw new ArgumentNullException(nameof(region));
        }

        public (string Signature, string Policy) GetSignature(JToken stringToSign)
        {
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var key = AWS4Signer.ComposeSigningKey(this._awsSecretAccessKey, this._region, date, "s3");
            var policy = Convert.ToBase64String(Encoding.UTF8.GetBytes(stringToSign.ToString()));
            var signatureHash = AWS4Signer.ComputeKeyedHash(SigningAlgorithm.HmacSHA256, key, policy);
            var signature = AWSSDKUtils.ToHex(signatureHash, true);

            return (signature, policy);
        }

        private readonly string _awsSecretAccessKey;
        private readonly string _region;
    }
}