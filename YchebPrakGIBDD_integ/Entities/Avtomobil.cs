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
    
    public partial class Avtomobil
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Avtomobil()
        {
            this.Shtrafi = new HashSet<Shtrafi>();
        }
    
        public int ID { get; set; }
        public string VIN_nomer { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public int God { get; set; }
        public int Ves { get; set; }
        public int Cvet_ID { get; set; }
        public int Tip_Dvigatelia_ID { get; set; }
        public int Privod_ID { get; set; }
        public Nullable<int> Cod_Regiona_ID { get; set; }
    
        public virtual Code_Regiona Code_Regiona { get; set; }
        public virtual Cvet_Mashini Cvet_Mashini { get; set; }
        public virtual Privod Privod { get; set; }
        public virtual Tip_Dvigatelia Tip_Dvigatelia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shtrafi> Shtrafi { get; set; }
    }
}
