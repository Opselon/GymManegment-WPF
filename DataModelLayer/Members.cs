//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModelLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Members
    {
        public Nullable<decimal> MemberCharge { get; set; }
        public Nullable<long> MemberFactorCode { get; set; }
        public string MemberStartDate { get; set; }
        public Nullable<int> MemberFindID { get; set; }
        public Nullable<byte> MemberMounth { get; set; }
        public string MemberLastCharge { get; set; }
        public string MemberDesc { get; set; }
        public Nullable<byte> MemberMountTwo { get; set; }
        public int MemberKeyID { get; set; }
        public Nullable<decimal> UserBedehiRial { get; set; }
        public string BedehKariDesc { get; set; }
        public Nullable<byte> MemberService { get; set; }
        public byte[] QRCode { get; set; }
    
        public virtual Users Users { get; set; }
    }
}