using System.Linq;
using System.Collections.Generic;

using FocusOnTheFamily.ReadyToWed.Metrics.DataModel;

namespace FocusOnTheFamily.ReadyToWed.Metrics.BusinessModel {
  //I'm using static business handler methods because of the simplicity of the application
  public class UserClassBusinessHandler : ClassBusinessHandler {
    public class GenderCount {
      public Gender Gender { get; set; }
      public int Count { get; set; }
    }

    //3. As a Marketer, I want to know which gender is installing my app more often so I can know how to improve my marketing.
    public static IList<GenderCount> GetNumberOfUsersByGender(IQueryable<User> users) {
      //I'm taking this approach because this will automatically expand if the
      //number of Gender options expands (like to include 'NotAvailable')
      return (
        from u in users
        group u by u.Gender into genderGroup
        select new GenderCount {
          Gender = genderGroup.Key,
          Count = genderGroup.Count(),
        }
      ).ToList();
    }

    //4. As a Marketer, I want to know the average age of people installing the application so I can know which demographics my marketing is working best in.
    public static double GetAverageAgeOfUsers(IQueryable<User> users) {
      //Calculating the average using Linq will ensure that if the IQuerable is
      //a database then the resultant query will leverage the database features
      //instead of pulling the entire record set into memory.
      return (
        from u in users
        select u.Age
      ).Average();
    }

  }
}
