using System.Linq;
using System.Collections.Generic;

using FocusOnTheFamily.ReadyToWed.Metrics.DataModel;

namespace FocusOnTheFamily.ReadyToWed.Metrics.BusinessModel {
  public class MarketingDomainBusinessHandler : DomainBusinessHandler<ReadyToWedMetricsContext> {
    public class UserMetrics {
      public double AverageAgeOfUsers { get; set; }
      public IList<UserClassBusinessHandler.GenderCount> NumberOfUsersByGender { get; set; }
    }

    public UserMetrics GetUserMetrics() {
      IQueryable<User> users = this.Context.Users;

      return new UserMetrics {
        AverageAgeOfUsers = UserClassBusinessHandler.GetAverageAgeOfUsers(users),
        NumberOfUsersByGender = UserClassBusinessHandler.GetNumberOfUsersByGender(users),
      };
    }

    public class DailyMetrics {
      public double AverageLogins { get; set; }
      public double AverageInstalls { get; set; }
      public double AverageCompletions { get; set; }
    }

    public DailyMetrics GetDailyMetrics() {
      IQueryable<DailyNumbers> dailyNumbers = this.Context.DailyNumbers;

      return new DailyMetrics {
        AverageLogins = DailyNumbersClassBusinessHandler.GetAverageDailyLogins(dailyNumbers),
        AverageInstalls = DailyNumbersClassBusinessHandler.GetAverageDailyInstalls(dailyNumbers),
        AverageCompletions = DailyNumbersClassBusinessHandler.GetAverageDailyCompletions(dailyNumbers)
      };
    }
  }
}
