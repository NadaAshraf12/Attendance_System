using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Common.Enums
{
    public enum StatusCodesEnum
    {
        Ok = 200,
        Created = 201,
        BadRequest = 400,
        NotAuthorized = 401,
        Notfound = 404,
        NotAllowed = 405,
        ServerError = 500

    }
}
