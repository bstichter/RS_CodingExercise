using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace FocusOnTheFamily.ReadyToWed.Metrics.DataModel {
  public class ReadyToWedMetricsContext : DbContext {
    public ReadyToWedMetricsContext() : base() { }
    public ReadyToWedMetricsContext(DbContextOptions<ReadyToWedMetricsContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<DailyNumbers> DailyNumbers { get; set; }
  }
}
