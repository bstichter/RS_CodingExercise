using System.Linq;

using FocusOnTheFamily.ReadyToWed.Metrics.DataModel;

namespace FocusOnTheFamily.ReadyToWed.Metrics.BusinessModel {
  //I'm using static business handler methods because of the simplicity of the application
  public class DailyNumbersClassBusinessHandler : ClassBusinessHandler {
    //1. As a Marketer, I want to know the average number of people installing the app every day so I can know if my marketing is effective.
    public static double GetAverageDailyInstalls(IQueryable<DailyNumbers> dailyNumbers) {
      return (
        from dn in dailyNumbers
        select dn.Installs
      ).Average();
    }

    //2. As a Marketer, I want to know the average number of people logging into the app every day so I can know if the design of my application is effective.
    public static double GetAverageDailyLogins(IQueryable<DailyNumbers> dailyNumbers) {
      return (
        from dn in dailyNumbers
        select dn.Logins
      ).Average();
    }

    //This isn't necessary, but I left a bug here so I can find it in my unit tests
    public static double GetAverageDailyCompletions(IQueryable<DailyNumbers> dailyNumbers) {
      return (
        from dn in dailyNumbers
        select dn.Logins
      ).Average();
    }
  }
}
