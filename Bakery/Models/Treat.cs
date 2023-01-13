using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bakery.Models
{
  public class Treat
  {
    public int TreatId { get; set; }
    [Required(ErrorMessage = "This field cannot be blank! Try again.")]
    public string Name { get; set; }
    public List<FlavorTreat> JoinEntities { get; }
  }
}