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
    
    public partial class FotoFyrnitury
    {
        public int IdFoto { get; set; }
        public int Id { get; set; }
        public string MimeType { get; set; }
        public byte[] Imaging { get; set; }
    
        public virtual Furnitura Furnitura { get; set; }
    }
}
