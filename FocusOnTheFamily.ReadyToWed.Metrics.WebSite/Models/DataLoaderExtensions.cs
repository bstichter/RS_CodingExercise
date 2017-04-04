using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using FocusOnTheFamily.ReadyToWed.Metrics.DataModel;

namespace FocusOnTheFamily.ReadyToWed.Metrics.WebSite {
  public class DataLoaderOptions {
    public string UsersFile { get; set; }
    public string DailyNumbersFile { get; set; }
  }

  public static class DataLoaderExtensions {
    public static void LoadData(this IApplicationBuilder app, DataLoaderOptions options) {
      var context = app.ApplicationServices.GetService<ReadyToWedMetricsContext>();

      context.Users.AddRange(
        from l in File.ReadAllLines(options.UsersFile).Skip(1)
        select l.Split(',') into elements
        select new User {
          Gender = (elements[0] == "M" ? Gender.Male : Gender.Female),
          Age = int.Parse(elements[1])
        }
      );

      context.DailyNumbers.AddRange(
        from l in File.ReadAllLines(options.DailyNumbersFile).Skip(1)
        select l.Split(',') into elements
        select new DailyNumbers {
          Installs = int.Parse(elements[0]),
          Logins = int.Parse(elements[1]),
          Completions = int.Parse(elements[2])
        }
      );

      context.SaveChanges();
    }
  }
}
