// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Vivero.Models
{
    public partial class Jardineros
    {
        public Jardineros()
        {
            Plantas = new HashSet<Plantas>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }

        public virtual ICollection<Plantas> Plantas { get; set; }
    }
}