namespace appin2seq
{
  public interface IMapper<TSource, TTarget>
  {
    TTarget Map(TSource source);
  }
}
