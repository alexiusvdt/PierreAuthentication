using System.Collections.Generic;

namespace Bakery.Models
{
  public class EngineerMachine
    {       
      public int FlavorTreatId { get; set; }
      public int FlavorId { get; set; }
      public int TreatId { get; set; }
      public Flavor Flavor { get; set; }
      public Treat Treat { get; set; }
    }
}