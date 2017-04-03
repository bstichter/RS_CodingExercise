#if DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FocusOnTheFamily.ReadyToWed.Metrics.DataModel {
  public static class ReadyToWedMetricsContextExtensions {

    public static void LoadUserMetrics(this ReadyToWedMetricsContext context, string userFile) {
      var users = (
        from l in File.ReadAllLines(userFile).Skip(1)
        select l.Split(',') into elements
        select new User {
          Gender = elements[0],
          Age = int.Parse(elements[1])
        }
      );

      context.Users.AddRange(users);
      context.SaveChanges();
    }

    public static void LoadDailyNumbersMetrics(this ReadyToWedMetricsContext context, string dailyNumbersFile) {
      var dailyNumbers = (
        from l in File.ReadAllLines(dailyNumbersFile).Skip(1)
        select l.Split(',') into elements
        select new DailyNumbers {
          Installs = int.Parse(elements[0]),
          Logins = int.Parse(elements[1]),
          Completions = int.Parse(elements[2])
        }
      );

      context.DailyNumbers.AddRange(dailyNumbers);
      context.SaveChanges();
    }
  }



}
#endif
