namespace FineUploadS3.Controllers
{
    using FineUploadS3.Services;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;

    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        private readonly IAwsS3Signer _awsSigner;

        public UploadsController(IAwsS3Signer awsSigner)
        {
            this._awsSigner = awsSigner;
        }

        [HttpPost("authorize")]
        public IActionResult Authorize([FromBody]JToken stringToSign)
        {
            var (Signature, Policy) = this._awsSigner.GetSignature(stringToSign);

            return this.Ok(new { Signature, Policy });
        }
    }
}