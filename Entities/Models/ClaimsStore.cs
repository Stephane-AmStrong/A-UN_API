using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.QueryParameters
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>
        {
            new Claim("readAppUser", "Read AppUsers"),
            new Claim("writeAppUser", "Write AppUsers"),

            new Claim("readBranch", "Read Branches"),
            new Claim("writeBranch", "Write Branches"),

            new Claim("readBranchLevel", "Read BranchLevels"),
            new Claim("writeBranchLevel", "Write BranchLevels"),

            new Claim("readPersonalFile", "Read PersonalFiles"),
            new Claim("writePersonalFile", "Write PersonalFiles"),

            new Claim("readObjective", "Read Objectives"),
            new Claim("writeObjective", "Write Objectives"),

            new Claim("readPartner", "Read Partners"),
            new Claim("writePartner", "Write Partners"),

            new Claim("readPayment", "Read Payments"),
            new Claim("writePayment", "Write Payments"),

            new Claim("readPaymentType", "Read PaymentTypes"),
            new Claim("writePaymentType", "Write PaymentTypes"),

            new Claim("readRegistrationForm", "Read RegistrationForms"),
            new Claim("writeRegistrationForm", "Write RegistrationForms"),

            new Claim("readRegistrationFormLine", "Read RegistrationFormLines"),
            new Claim("writeRegistrationFormLine", "Write RegistrationFormLines"),

            new Claim("readSubscription", "Read Subscriptions"),
            new Claim("writeSubscription", "Write Subscriptions"),

            new Claim("readSubscriptionLine", "Read SubscriptionLines"),
            new Claim("writeSubscriptionLine", "Write SubscriptionLines"),

            new Claim("readTechnicalTheme", "Read TechnicalThemes"),
            new Claim("writeTechnicalTheme", "Write TechnicalThemes"),

            new Claim("readUniversity", "Read Universities"),
            new Claim("writeUniversity", "Write Universities"),

            new Claim("readWorkstation", "Read Workstations"),
            new Claim("writeWorkstation", "Write Workstations"),
        };
    }
}
