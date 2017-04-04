using System;

namespace FocusOnTheFamily.ReadyToWed.Metrics.DataModel {
  public enum Gender { Male, Female  }

  public class User {
    public int Id { get; set; }
    public Gender Gender { get; set; }
    public int Age { get; set; }
  }
}
