using jay.school.contracts.Contracts;
using jay.school.contracts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using jay.school.Utils;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net;

using System.Text.Json;

using AgoraIO.Media;


namespace jay.school.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubhouseController : ControllerBase
    {

        private readonly AppSettings appSettings;
        public ClubhouseController(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        [HttpPost]

        public ActionResult<AuthenticateResponse> index(AuthenticateRequest request)

        {

            if (string.IsNullOrEmpty(appSettings.AppID) || string.IsNullOrEmpty(appSettings.AppCertificate))

            {

                return new StatusCodeResult((int)HttpStatusCode.PreconditionFailed);

            }

            var uid = request.uid.ValueKind == JsonValueKind.Number ? request.uid.GetUInt64().ToString() : request.uid.GetString();

            var tokenBuilder = new AccessToken(appSettings.AppID, appSettings.AppCertificate, request.channel, uid, 676767, 123456789);

            tokenBuilder.addPrivilege(Privileges.kJoinChannel, request.expiredTs);

            tokenBuilder.addPrivilege(Privileges.kPublishAudioStream, request.expiredTs);

            tokenBuilder.addPrivilege(Privileges.kPublishVideoStream, request.expiredTs);

            tokenBuilder.addPrivilege(Privileges.kPublishDataStream, request.expiredTs);

            tokenBuilder.addPrivilege(Privileges.kRtmLogin, request.expiredTs);

            return Ok(new AuthenticateResponse

            {

                channel = request.channel,

                uid = request.uid,

                token = tokenBuilder.build()

            });

        }

    }

}