//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DALdv1
{
    using System;
    using System.Collections.Generic;
    
    public partial class SeoVhodnuhDverei
    {
        public int Id { get; set; }
        public string TitleDveri { get; set; }
        public string KeywordsDveri { get; set; }
        public string DescriptionDveri { get; set; }
    
        public virtual VhodnyeDveri VhodnyeDveri { get; set; }
    }
}
