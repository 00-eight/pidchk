/*
    Copyright (C) 2019, Kristian Lamb

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

    Module: LicenseInfo.cs

    Author: Kristian Lamb

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pidchk
{
    public class LicenseInfo
    {
        public string Status { get; set; }
        public string PkeyFile { get; set; }
        public string ProductKey { get; set; }
        public string KeyStatus { get; set; }
        public string ExtendedPID { get; set; }
        public string ActivationID { get; set; }
        public string EditionType { get; set; }
        public string EditionID { get; set; }
        public string LicenseType { get; set; }
        public string LicenseChannel { get; set; }
        public string ProductDesc { get; set; }
        public string ErrorMessage { get; set; }

        public string Dump()
        {
            if (Status != "success")
            {
                return "{\"status\":\"" + $"{Status}\", \"errormessage\": \"" + $"{ErrorMessage}\"" + "}";
            }
            return "{\"status\":\"" + $"{Status}\"," +
                   "\"pkeyfile\":\"" + $"{PkeyFile}\"," +
                   "\"productkey\":\"" + $"{ProductKey}\"," +
                   "\"keystatus\":\"" + $"{KeyStatus}\"," +
                   "\"extendedpid\":\"" + $"{ExtendedPID}\"," +
                   "\"activationid\":\"" + $"{ActivationID}\"," +
                   "\"editiontype\":\"" + $"{EditionType}\"," +
                   "\"editionid\":\"" + $"{EditionID}\"," +
                   "\"licensetype\":\"" + $"{LicenseType}\"," +
                   "\"licensechannel\":\"" + $"{LicenseChannel}\"," +
                   "\"productdesc\":\"" + $"{ProductDesc}\"" + "}";
        }
    }
}
