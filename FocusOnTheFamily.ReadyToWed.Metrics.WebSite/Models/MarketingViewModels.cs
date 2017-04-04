using System.ComponentModel;
using System.Collections.Generic;

namespace FocusOnTheFamily.ReadyToWed.Metrics.WebSite.MarketingViewModels {
  public class MetricsViewModel {
    //1. As a Marketer, I want to know the average number of people installing the app every day so I can know if my marketing is effective.
    [DisplayName("Average Installs Per Day")]
    public decimal AverageInstallsPerDay { get; set; }

    //2. As a Marketer, I want to know the average number of people logging into the app every day so I can know if the design of my application is effective.
    [DisplayName("Average Logins Per Day")]
    public decimal AverageLoginsPerDay { get; set; }

    //3. As a Marketer, I want to know which gender is installing my app more often so I can know how to improve my marketing.
    public int PercentOfUsersMale { get; set; }
    public int PercentOfUsersFemale { get; set; }

    //4. As a Marketer, I want to know the average age of people installing the application so I can know which demographics my marketing is working best in.
    [DisplayName("Average Age of Users")]
    public decimal AverageAgeOfUsers { get; set; }
  }
}
