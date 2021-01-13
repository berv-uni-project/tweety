using System.ComponentModel.DataAnnotations;

namespace Tweety.Models
{
    public class Tags
    {
        [Required(ErrorMessage = "Required")]
        public string Name { set; get; }
        public string DinasKesehatan { set; get; }
        public string DinasSosial { set; get; }
        public string DinasPendidikan { set; get; }
        public string DinasBinamarga { set; get; }
        public string DinasPemuda { set; get; }
        [Required(ErrorMessage = "Required")]
        public bool? IsKMP { set; get; }
    }
}