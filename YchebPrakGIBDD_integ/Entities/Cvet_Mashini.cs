//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YchebPrakGIBDD_integ.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cvet_Mashini
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cvet_Mashini()
        {
            this.Avtomobil = new HashSet<Avtomobil>();
        }
    
        public int Nomer_Cveta { get; set; }
        public string Kod_Cveta { get; set; }
        public string Nazvanie_RU { get; set; }
        public string Nazvanie_Cveta_RU { get; set; }
        public Nullable<int> Metalic { get; set; }
        public string Nazvanie_EN { get; set; }
        public string Nazvanie_Cveta_EN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Avtomobil> Avtomobil { get; set; }
    }
}
