﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Domain.Enums
{
    public enum ResultEditUser
    {
        Success,
        UserNotFound,
        EmailDuplicated,
        UserNameDuplicated
    }
}
