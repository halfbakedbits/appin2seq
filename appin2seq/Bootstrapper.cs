using Autofac;

namespace appin2seq
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
