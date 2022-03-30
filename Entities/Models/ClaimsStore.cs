using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>
        {
            new Claim("readAbout", "Read Abouts"),
            new Claim("writeAbout", "Write Abouts"),
            new Claim("manageAbout", "Manage Abouts"),
            
            new Claim("readAcademicYear", "Read AcademicYears"),
            new Claim("writeAcademicYear", "Write AcademicYears"),
            new Claim("manageAcademicYear", "Manage AcademicYears"),

            new Claim("readAppUser", "Read AppUsers"),
            new Claim("writeAppUser", "Write AppUsers"),
            new Claim("manageAppUser", "Manage AppUsers"),

            new Claim("readBanner", "Read Banners"),
            new Claim("writeBanner", "Write Banners"),

            new Claim("readCategory", "Read Categories"),
            new Claim("writeCategory", "Write Categories"),

            new Claim("readFormation", "Read Formations"),
            new Claim("writeFormation", "Write Formations"),
            new Claim("manageFormation", "Manage Formations"),

            new Claim("readFormationLevel", "Read FormationLevels"),
            new Claim("writeFormationLevel", "Write FormationLevels"),
            new Claim("manageFormationLevel", "Manage FormationLevels"),

            new Claim("readPersonalFile", "Read PersonalFiles"),
            new Claim("writePersonalFile", "Write PersonalFiles"),
            new Claim("managePersonalFile", "Manage PersonalFiles"),

            new Claim("readPartner", "Read Partners"),
            new Claim("writePartner", "Write Partners"),
            new Claim("managePartner", "Manage Partners"),

            new Claim("readPayment", "Read Payments"),
            new Claim("writePayment", "Write Payments"),
            new Claim("managePayment", "Manage Payments"),

            new Claim("readPaymentType", "Read PaymentTypes"),
            new Claim("writePaymentType", "Write PaymentTypes"),
            new Claim("managePaymentType", "Manage PaymentTypes"),

            new Claim("readFormationLevelFile", "Read FormationLevelFiles"),
            new Claim("writeFormationLevelFile", "Write FormationLevelFiles"),
            new Claim("manageFormationLevelFile", "Manage FormationLevelFiles"),

            new Claim("readSubscription", "Read Subscriptions"),
            new Claim("writeSubscription", "Write Subscriptions"),
            new Claim("manageSubscription", "Manage Subscriptions"),

            new Claim("readSubscriptionLine", "Read SubscriptionLines"),
            new Claim("writeSubscriptionLine", "Write SubscriptionLines"),
            new Claim("manageSubscriptionLine", "Manage SubscriptionLines"),

            new Claim("readUniversity", "Read Universities"),
            new Claim("writeUniversity", "Write Universities"),
            new Claim("manageUniversity", "Manage Universities"),

            new Claim("readWorkstation", "Read Workstations"),
            new Claim("writeWorkstation", "Write Workstations"),
            new Claim("manageWorkstation", "Manage Workstations"),
        };

        public static List<Claim> EtudiantClaims = new List<Claim>
        {
            new Claim("readAcademicYear", "Read AcademicYears"),
            new Claim("readCategory", "Read Categories"),

            new Claim("readAppUser", "Read AppUsers"),
            new Claim("writeAppUser", "Write AppUsers"),

            new Claim("readFormation", "Read Formations"),

            new Claim("readFormationLevel", "Read FormationLevels"),

            new Claim("readPersonalFile", "Read PersonalFiles"),
            new Claim("writePersonalFile", "Write PersonalFiles"),

            new Claim("readPartner", "Read Partners"),

            new Claim("readPayment", "Read Payments"),
            new Claim("writePayment", "Write Payments"),

            new Claim("readPaymentType", "Read PaymentTypes"),

            new Claim("readFormationLevelFile", "Read FormationLevelFiles"),

            new Claim("readSubscription", "Read Subscriptions"),
            new Claim("writeSubscription", "Write Subscriptions"),

            new Claim("readSubscriptionLine", "Read SubscriptionLines"),
            new Claim("writeSubscriptionLine", "Write SubscriptionLines"),

            new Claim("readUniversity", "Read Universities"),
        };

        public static List<Claim> UniversityClaims = new List<Claim>
        {
            new Claim("readCategory", "Read Categories"),
            
            new Claim("readAppUser", "Read AppUsers"),
            new Claim("writeAppUser", "Write AppUsers"),

            new Claim("readFormation", "Read Formations"),
            new Claim("manageFormation", "Manage Formations"),

            new Claim("readFormationLevel", "Read FormationLevels"),
            new Claim("manageFormationLevel", "Manage FormationLevels"),

            new Claim("readPersonalFile", "Read PersonalFiles"),
            new Claim("writePersonalFile", "Write PersonalFiles"),

            new Claim("readPartner", "Read Partners"),

            new Claim("readPayment", "Read Payments"),
            new Claim("writePayment", "Write Payments"),

            new Claim("readPaymentType", "Read PaymentTypes"),

            new Claim("readFormationLevelFile", "Read FormationLevelFiles"),
            new Claim("manageFormationLevelFile", "Manage FormationLevelFiles"),

            new Claim("readSubscription", "Read Subscriptions"),
            new Claim("writeSubscription", "Write Subscriptions"),

            new Claim("readSubscriptionLine", "Read SubscriptionLines"),
            new Claim("writeSubscriptionLine", "Write SubscriptionLines"),

            new Claim("readUniversity", "Read Universities"),
            new Claim("writeUniversity", "Write Universities"),
        };


        public static List<Claim> EducationalConsultantClaims = new List<Claim>
        {
            new Claim("readAcademicYear", "Read AcademicYears"),
            new Claim("writeAcademicYear", "Write AcademicYears"),
            new Claim("manageAcademicYear", "Manage AcademicYears"),

            new Claim("readCategory", "Read Categories"),
            
            new Claim("readAppUser", "Read AppUsers"),
            new Claim("writeAppUser", "Write AppUsers"),
            new Claim("manageAppUser", "Manage AppUsers"),

            new Claim("readPartner", "Read Partners"),
            new Claim("writePartner", "Write Partners"),
            new Claim("managePartner", "Manage Partners"),

            new Claim("readPayment", "Read Payments"),
            new Claim("writePayment", "Write Payments"),
            new Claim("managePayment", "Manage Payments"),

            new Claim("readPaymentType", "Read PaymentTypes"),

            new Claim("readSubscription", "Read Subscriptions"),
            new Claim("manageSubscription", "Manage Subscriptions"),

            new Claim("readSubscriptionLine", "Read SubscriptionLines"),
            new Claim("manageSubscriptionLine", "Manage SubscriptionLines"),

            new Claim("readUniversity", "Read Universities"),

            new Claim("readWorkstation", "Read Workstations"),
        };

    }
}
