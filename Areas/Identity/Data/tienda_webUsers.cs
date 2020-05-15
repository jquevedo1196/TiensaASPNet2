using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace tienda_web.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the tienda_webUsers class
    public class tienda_webUsers : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Nombres { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string ApPat { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string ApMat { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(15)")]
        public string RFC { get; set; }
    }
}
