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

            var tokenBuilder = new AccessToken(appSettings.AppID, appSettings.AppCertificate, request.channel, uid);

            var expireTime = 3600;

            // calculate current time in seconds
            var currentTime = Math.Floor(DateTime.Now.Millisecond / 1000.0);

            // calculate privilege expire time
            var privilegeExpireTime = currentTime + expireTime;

            tokenBuilder.addPrivilege(Privileges.kJoinChannel, 6666666);

            tokenBuilder.addPrivilege(Privileges.kPublishAudioStream, 6666666);

            tokenBuilder.addPrivilege(Privileges.kPublishDataStream, 6666666);

            tokenBuilder.addPrivilege(Privileges.kRtmLogin, 6666666);

            return Ok(new AuthenticateResponse

            {

                channel = request.channel,

                uid = request.uid,

                token = tokenBuilder.build()

            });

        }

    }

}