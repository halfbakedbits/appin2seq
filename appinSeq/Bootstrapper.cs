using Autofac;

namespace appinSeq
{
  internal class Bootstrapper
  {
    public IContainer Init()
    {
      var builder = new ContainerBuilder();

      builder.RegisterAssemblyTypes(typeof(Program).Assembly)
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();

      return builder.Build();
    }
  }
}
