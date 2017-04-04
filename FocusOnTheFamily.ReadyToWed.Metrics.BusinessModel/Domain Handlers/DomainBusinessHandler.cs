using Microsoft.EntityFrameworkCore;

namespace FocusOnTheFamily.ReadyToWed.Metrics.BusinessModel {
  public abstract class DomainBusinessHandler<T> where T : DbContext {
    public T Context { get; set; }

    public void Init(T context) {
      this.Context = context;
    }
  }

}
