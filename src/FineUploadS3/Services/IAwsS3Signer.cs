namespace FineUploadS3.Services
{
    using Newtonsoft.Json.Linq;

    public interface IAwsS3Signer
    {
        (string Signature, string Policy) GetSignature(JToken stringToSign);
    }
}